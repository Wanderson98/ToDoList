using System;
using ToDo.Domain.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;
using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using ToDo.Domain.Interfaces;
using ToDo.Services.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace ToDo.Tests;


public class UsuarioServiceTests
{
    [Fact]
    public async Task Deve_Cadastrar_E_Verificar_Email_Duplicado()
    {
        var mockCache = new Mock<IDistributedCache>();
        var mockRepository = new Mock<IUsuarioRepository>();
        var mockValidator = new Mock<IValidator<CriarUsuarioDTO>>();

        mockValidator.Setup(v => v.Validate(It.IsAny<CriarUsuarioDTO>()))
                     .Returns(new ValidationResult());

        mockRepository.Setup(r => r.AdicionarUsuarioAsync(It.IsAny<Usuario>()))
                      .ReturnsAsync((Usuario u) => 
                      {
                            return u;
                      });
        
        var service = new UsuarioService(mockRepository.Object, mockValidator.Object);

        var dto = new CriarUsuarioDTO 
        { 
            Nome = "Usuário Teste", 
            Email = "wm@gmail.com",
            Senha = "senha1234"
        };

        var resultado = await service.CriarUsuarioAsync(dto);

        Assert.NotNull(resultado);
        Assert.Equal("Usuário Teste", resultado.Nome);

        mockRepository.Verify(r => r.AdicionarUsuarioAsync(It.IsAny<Usuario>()), Times.Once);
        mockValidator.Verify(v => v.Validate(It.IsAny<CriarUsuarioDTO>()), Times.Once);
        
    }

    [Fact]
public async Task Deve_Lancar_Erro_Quando_Email_Ja_Existe()
{
    // 1. ARRANGE
    var mockRepo = new Mock<IUsuarioRepository>();
    var mockValidator = new Mock<IValidator<CriarUsuarioDTO>>();

    // Validação de formato passa (Ok)
    mockValidator.Setup(v => v.Validate(It.IsAny<CriarUsuarioDTO>()))
                 .Returns(new ValidationResult());

    // Simulando que o banco JÁ TEM esse usuário
    mockRepo.Setup(r => r.ObterPorEmailAsync("duplicado@teste.com"))
            .ReturnsAsync(new Usuario("Já Existe", "duplicado@teste.com", "hashQualquer"));


    var service = new UsuarioService(mockRepo.Object, mockValidator.Object);

    var dto = new CriarUsuarioDTO 
    { 
        Nome = "Tentativa", 
        Email = "duplicado@teste.com",
        Senha = "123"
    };

    // 2. ACT & ASSERT
    // Esperamos que o serviço EXPLODA com erro de validação
    await Assert.ThrowsAsync<ValidationException>(() => service.CriarUsuarioAsync(dto));

    // 3. VERIFY
    // Garantimos que o serviço NÃO tentou salvar no banco (proteção de integridade)
    mockRepo.Verify(r => r.AdicionarUsuarioAsync(It.IsAny<Usuario>()), Times.Never);
}
    [Fact]
public async Task Deve_Criptografar_Senha_Ao_Cadastrar()
{
    // 1. ARRANGE
    var mockRepo = new Mock<IUsuarioRepository>();
    var mockValidator = new Mock<IValidator<CriarUsuarioDTO>>();

    mockValidator.Setup(v => v.Validate(It.IsAny<CriarUsuarioDTO>())).Returns(new ValidationResult());
    
    // Ninguém com esse email (caminho livre para criar)
    mockRepo.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync((Usuario?)null);

    var service = new UsuarioService(mockRepo.Object, mockValidator.Object);
    
    var senhaPura = "MinhaSenhaSecreta123";
    var dto = new CriarUsuarioDTO { Nome = "Teste", Email = "a@a.com", Senha = senhaPura };

    // 2. ACT
    await service.CriarUsuarioAsync(dto);

    // 3. ASSERT (Inspeção Avançada)
    // Verificamos: O método Adicionar foi chamado com um usuário...
    // ...onde a SenhaHash NÃO É IGUAL à senha pura?
    mockRepo.Verify(r => r.AdicionarUsuarioAsync(It.Is<Usuario>(u => 
        u.SenhaHash != senhaPura && 
        !string.IsNullOrEmpty(u.SenhaHash) // E também não está vazia
    )), Times.Once);
}

}