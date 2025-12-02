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
        public async Task AtualizarUsuarioAsync(int id, AtualizarUsuarioDTO atualizarUsuarioDto)
        {
            var usuario = await _usuarioRepository.ObeterPorIdAsync(id);
            if (usuario == null) throw new KeyNotFoundException("Usuário não encontrado.");

            if(!usuario.Email.Equals(atualizarUsuarioDto.Email))
                if(await EmailJaCadastrado(atualizarUsuarioDto.Email))
                    throw new ValidationException(new[] { new ValidationFailure("Email", "O email já está cadastrado.") });
                
            

            if(!BCrypt.Net.BCrypt.Verify(atualizarUsuarioDto.Senha, usuario.SenhaHash))
                throw new ValidationException(new[] { new ValidationFailure("Senha", "Senha atual incorreta.") });
            
            
            if(!string.IsNullOrEmpty(atualizarUsuarioDto.NovaSenha))           
                usuario.AtualizarSenha(BCrypt.Net.BCrypt.HashPassword(atualizarUsuarioDto.NovaSenha));
            
            if(!usuario.Nome.Equals(atualizarUsuarioDto.Nome))
                usuario.AtualizarNome(atualizarUsuarioDto.Nome);

            if(!usuario.Email.Equals(atualizarUsuarioDto.Email))
                usuario.AtualizarEmail(atualizarUsuarioDto.Email);

            await _usuarioRepository.AtualizarUsuarioAsync(usuario);
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

        public async Task<IEnumerable<Usuario>> ObterTodosUsuariosAsync()
        {
            var usuarios =  await _usuarioRepository.ObterTodosAsync();
            return usuarios;
        }

        public async Task<Usuario> ObterUsuarioPorEmailAsync(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
            return usuario;
        }

        public async Task<Usuario> ObterUsuarioPorIdAsync(int id)
        {
             var usuario = await _usuarioRepository.ObeterPorIdAsync(id);
             return usuario;
        }

        public async Task RemoverUsuarioAsync(int id)
        {
            await _usuarioRepository.ExcluirUsuarioAsync(id);
        }

        private async Task<bool> EmailJaCadastrado(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
            return usuario != null;
        }
    }
}