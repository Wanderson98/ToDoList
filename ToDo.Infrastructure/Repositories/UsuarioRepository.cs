using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Interfaces;
using ToDo.Domain.Models;
using ToDo.Infrastructure.Data;

namespace ToDo.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> AdicionarUsuarioAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public Task AtualizarUsuarioAsync(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Task ExcluirUsuarioAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> ObeterPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}