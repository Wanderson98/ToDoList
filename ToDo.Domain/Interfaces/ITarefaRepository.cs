namespace ToDo.Domain.Interfaces
{
    using ToDo.Domain.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITarefaRepository
    {
        Task<Tarefa> AdicionarAsync(Tarefa tarefa);
        Task<Tarefa> ObterPorIdAsync(int id);
        Task<IEnumerable<Tarefa>> ObterTodasAsync();
        Task AtualizarAsync(Tarefa tarefa);
        Task RemoverAsync(int id);
    }
}