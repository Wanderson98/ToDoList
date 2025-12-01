using ToDo.Domain.Models;

namespace ToDo.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObeterPorIdAsync(int id);
        Task<Usuario> ObterPorEmailAsync(string email);
        Task<IEnumerable<Usuario>> ObterTodosAsync();
        Task<Usuario> AdicionarUsuarioAsync(Usuario usuario);
        Task AtualizarUsuarioAsync(Usuario usuario);
        Task ExcluirUsuarioAsync(int id);
    }
}