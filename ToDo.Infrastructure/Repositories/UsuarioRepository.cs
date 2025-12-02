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

        public async Task<Usuario> AtualizarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await  _context.SaveChangesAsync();
            return usuario;
        }

        public async Task ExcluirUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) throw new KeyNotFoundException("Usuário não encontrado.");
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> ObeterPorIdAsync(int id)
        {
            return await _context.Usuarios.Where(x=> x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            return await _context.Usuarios.AsNoTracking().ToListAsync();
        }
    }
}