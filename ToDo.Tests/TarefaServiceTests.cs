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


namespace Todo.Tests
{
    public class TarefaServiceTests
    {   
        [Fact]
        public async Task Deve_Criar_Tarefa_Com_Sucesso()
        {
            // =================================================
            // 1. ARRANGE (Preparar o cenário)
            // =================================================
        
            // Criamos os Mocks (os dublês) das interfaces que o serviço exige
            var mockRepository = new Mock<ITarefaRepository>();
            var mockValidator = new Mock<IValidator<CriarTarefaDTO>>();

            // Configurando o Validador para dizer "SIM, está tudo válido"
            // Quando Validate for chamado com qualquer DTO, retorne um ValidationResult vazio (que significa válido)
            mockValidator.Setup(v => v.Validate(It.IsAny<CriarTarefaDTO>()))
                     .Returns(new ValidationResult());
        
            // Configurando o Repositório para fingir que salvou
            // Quando AdicionarAsync for chamado, apenas retorne a tarefa (fingindo que o banco gerou ID)
            mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Tarefa>()))
                .ReturnsAsync((Tarefa t) => {
                    // Aqui poderíamos simular o banco gerando um ID, se quiséssemos
                    return t; 
                });
            // Instanciamos o Serviço Real passando os Dublês (.Object)
            var service = new TarefaService(mockRepository.Object, mockValidator.Object);

            // Criamos o dado de entrada
            var dto = new CriarTarefaDTO { Titulo = "Testar Moq", Descricao = "Aprender Mocks" };

            // =================================================
            // 2. ACT (A hora da ação)
            // =================================================
        
            // Chamamos o método que queremos testar
            var resultado = await service.CriarTarefaAsync(dto);

            // =================================================
            // 3. ASSERT (Verificações)
            // =================================================

            // Verificamos se o resultado está correto
            Assert.NotNull(resultado);
            Assert.Equal("Testar Moq", resultado.Titulo);
            Assert.Equal("Aprender Mocks", resultado.Descricao);
            // Verificamos se o repositório foi chamado corretamente
            mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Tarefa>()), Times.Once);
            // Verificamos se o validador foi chamado corretamente
            mockValidator.Verify(v => v.Validate(It.IsAny<CriarTarefaDTO>()), Times.Once);

            
        }

        [Fact]
        public async Task Deve_Lancar_Erro_Quando_Titulo_Invalido()
        {
            var mockRepository = new Mock<ITarefaRepository>();
            var mockValidator = new Mock<IValidator<CriarTarefaDTO>>();
            var resultadoComErro = new ValidationResult(new[] { new ValidationFailure("Titulo", "Título é obrigatório") });

            mockValidator.Setup(v => v.Validate(It.IsAny<CriarTarefaDTO>()))
                     .Returns(resultadoComErro);
            
            var service = new TarefaService(mockRepository.Object, mockValidator.Object);

            var dto = new CriarTarefaDTO { Titulo = "Test", Descricao = "Aprender Mocks" };
            await Assert.ThrowsAsync<ValidationException>(async () => await service.CriarTarefaAsync(dto));

            mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Tarefa>()), Times.Never);
            mockValidator.Verify(v => v.Validate(It.IsAny<CriarTarefaDTO>()), Times.Once);
        }
    
    }

}