using FluentValidation;
using FluentValidation.Results;
using ToDo.Domain.Interfaces;
using ToDo.Domain.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IValidator<CriarUsuarioDTO> _validator;

        public UsuarioService(IUsuarioRepository usuarioRepository, IValidator<CriarUsuarioDTO> validator)
        {
            _usuarioRepository = usuarioRepository;
            _validator = validator;
        }
        public Task AtualizarUsuarioAsync(int id, CriarUsuarioDTO atualizarUsuarioDto)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario> AutenticarUsuarioAsync(LoginDTO loginDto)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(loginDto.Email!);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash))
            {
                throw new ValidationException(new[] { new ValidationFailure("Autenticação", "Email ou senha inválidos.") });
            }
            return usuario;

        }

        public async Task<Usuario> CriarUsuarioAsync(CriarUsuarioDTO criarUsuarioDto)
        {
            var validationResult = _validator.Validate(criarUsuarioDto);
            if (validationResult.IsValid == false)
            {
                throw new ValidationException(validationResult.Errors); 
            }

            if(await EmailJaCadastrado(criarUsuarioDto.Email))
            {
                throw new ValidationException(new [] {new ValidationFailure("Email", "O email já está cadastrado.")});
            }
            var usuario = new Usuario(
                criarUsuarioDto.Nome,
                criarUsuarioDto.Email,
                BCrypt.Net.BCrypt.HashPassword(criarUsuarioDto.Senha)
            );

            return await _usuarioRepository.AdicionarUsuarioAsync(usuario);
        }

        public Task<IEnumerable<Usuario>> ObterTodosUsuariosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> ObterUsuarioPorEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> ObterUsuarioPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoverUsuarioAsync(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> EmailJaCadastrado(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
            return usuario != null;
        }
    }
}