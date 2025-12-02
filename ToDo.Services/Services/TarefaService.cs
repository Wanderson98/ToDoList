namespace ToDo.Services.Services
{
    using ToDo.Domain.Models;
    using ToDo.Services.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ToDo.Domain.Interfaces;
    using ToDo.Services.Interfaces;
    using FluentValidation;
    using Microsoft.Extensions.Caching.Distributed;
    using System.Text.Json;

    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IDistributedCache _cache;
        private readonly IValidator<CriarTarefaDTO> _validator;

        private const string CacheKeyTodasTarefas = "tarefas_todas";
        public TarefaService(ITarefaRepository tarefaRepository, IValidator<CriarTarefaDTO> validator, IDistributedCache cache)
        {
            _tarefaRepository = tarefaRepository;
            _validator = validator;
            _cache = cache;
        }

        public async Task<Tarefa> CriarTarefaAsync(CriarTarefaDTO criarTarefaDto)
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
            await _cache.RemoveAsync(CacheKeyTodasTarefas);
            return await _tarefaRepository.AdicionarAsync(tarefa);
        }

        public async Task<Tarefa> ObterTarefaPorIdAsync(int id)
        {
            return await _tarefaRepository.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<Tarefa>> ObterTodasTarefasAsync()
        {
            string? jsonTarefas = await _cache.GetStringAsync(CacheKeyTodasTarefas);

            if(!string.IsNullOrEmpty(jsonTarefas))
            {
                return JsonSerializer.Deserialize<IEnumerable<Tarefa>>(jsonTarefas) ?? new List<Tarefa>();
                
            }

            var tarefas = await _tarefaRepository.ObterTodasAsync();
            var opcoesCache = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            string json = JsonSerializer.Serialize(tarefas);
            await _cache.SetStringAsync(CacheKeyTodasTarefas, json, opcoesCache);

            return tarefas;
        }

        public async Task<Tarefa> MarcarTarefaComoConcluidaAsync(int id)
        { 
            var tarefa =  await _tarefaRepository.ObterPorIdAsync(id);
            if (tarefa == null) throw new KeyNotFoundException("Tarefa não encontrada.");

            tarefa.MarcarComoConcluida();
            await _tarefaRepository.AtualizarAsync(tarefa);
            await _cache.RemoveAsync(CacheKeyTodasTarefas);
            return tarefa;
        }

        public async Task RemoverTarefaAsync(int id)
        {
            await _cache.RemoveAsync(CacheKeyTodasTarefas);
            await  _tarefaRepository.RemoverAsync(id);
        }
    }
}