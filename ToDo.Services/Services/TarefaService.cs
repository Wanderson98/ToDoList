namespace ToDo.Services.Services
{
    using ToDo.Domain.Models;
    using ToDo.Services.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ToDo.Domain.Interfaces;
    using ToDo.Services.Interfaces;

    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }


        public Task<Tarefa> CriarTarefaAsync(CriarTarefaDTO criarTarefaDto)
        {
            var tarefa = new Tarefa(
                criarTarefaDto.Titulo,
                criarTarefaDto.Descricao
            );

            return _tarefaRepository.AdicionarAsync(tarefa);
        }

        public Task<Tarefa> ObterTarefaPorIdAsync(int id)
        {
            return _tarefaRepository.ObterPorIdAsync(id);
        }

        public Task<IEnumerable<Tarefa>> ObterTodasTarefasAsync()
        {
            return _tarefaRepository.ObterTodasAsync();
        }

        public async Task MarcarTarefaComoConcluidaAsync(int id)
        {
            var tarefa =  await _tarefaRepository.ObterPorIdAsync(id);
            if (tarefa == null) throw new KeyNotFoundException("Tarefa não encontrada.");

            tarefa.MarcarComoConcluida();
            await _tarefaRepository.AtualizarAsync(tarefa);
        }

        public Task RemoverTarefaAsync(int id)
        {
            return _tarefaRepository.RemoverAsync(id);
        }
    }
}