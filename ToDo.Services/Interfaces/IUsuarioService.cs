namespace ToDo.Services.Interfaces 
{
    using ToDo.Domain.Models;
    using ToDo.Services.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsuarioService
    {
        Task<Usuario> CriarUsuarioAsync(CriarUsuarioDTO criarUsuarioDto);
        Task<Usuario> ObterUsuarioPorIdAsync(int id);
        Task<Usuario> ObterUsuarioPorEmailAsync(string email);
        Task<IEnumerable<Usuario>> ObterTodosUsuariosAsync();
        Task AtualizarUsuarioAsync(int id, CriarUsuarioDTO atualizarUsuarioDto);
        Task RemoverUsuarioAsync(int id);
        Task<Usuario> AutenticarUsuarioAsync(LoginDTO loginDto);
    }
}