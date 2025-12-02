using ToDo.Domain.Interfaces;
using ToDo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Models;

namespace ToDo.Infrastructure.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tarefa> AdicionarAsync(Tarefa tarefa)
        {
            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();
            return tarefa;
        }

        public Task AtualizarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            return _context.SaveChangesAsync();
        }

        public async Task<Tarefa> ObterPorIdAsync(int id)
        {
           return await _context.Tarefas.FindAsync(id);
        }

        public async Task<IEnumerable<Tarefa>> ObterTodasAsync()
        {
            return await _context.Tarefas.AsNoTracking().ToListAsync();
        }
        

        public async Task RemoverAsync(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) throw new KeyNotFoundException("Tarefa não encontrada.");
            
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();  
            
        }
    }
}