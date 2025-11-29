namespace ToDo.Services.Interfaces
{
    using ToDo.Domain.Models;
    using ToDo.Services.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITarefaService
    {
        Task<Tarefa> CriarTarefaAsync(CriarTarefaDTO criarTarefaDto);
        Task<Tarefa> ObterTarefaPorIdAsync(int id);
        Task<IEnumerable<Tarefa>> ObterTodasTarefasAsync();
        Task MarcarTarefaComoConcluidaAsync(int id);
        Task RemoverTarefaAsync(int id);
    }
}