namespace ToDo.Services.Services
{
    using ToDo.Domain.Models;
    using ToDo.Services.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ToDo.Domain.Interfaces;
    using ToDo.Services.Interfaces;
    using FluentValidation;

    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IValidator<CriarTarefaDTO> _validator;
        public TarefaService(ITarefaRepository tarefaRepository, IValidator<CriarTarefaDTO> validator)
        {
            _tarefaRepository = tarefaRepository;
            _validator = validator;
        }

        public Task<Tarefa> CriarTarefaAsync(CriarTarefaDTO criarTarefaDto)
        {
            var validationResult = _validator.Validate(criarTarefaDto);
            if(validationResult.IsValid == false)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
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

        public async Task<Tarefa> MarcarTarefaComoConcluidaAsync(int id)
        { 
            var tarefa =  await _tarefaRepository.ObterPorIdAsync(id);
            if (tarefa == null) throw new KeyNotFoundException("Tarefa não encontrada.");

            tarefa.MarcarComoConcluida();
            await _tarefaRepository.AtualizarAsync(tarefa);

            return tarefa;
        }

        public Task RemoverTarefaAsync(int id)
        {
            return _tarefaRepository.RemoverAsync(id);
        }
    }
}