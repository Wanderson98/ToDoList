using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDo.Domain.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;
        public ToDoController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CriarTarefa([FromBody] CriarTarefaDTO request)
        {
            var tarefa = await _tarefaService.CriarTarefaAsync(request);
            return CreatedAtAction(nameof(ObterTarefaPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterTarefaPorId(int id)
        {
            var tarefa = await _tarefaService.ObterTarefaPorIdAsync(id);
            if (tarefa == null)
                return NotFound();
            return Ok(tarefa);  
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasTarefas()
        {
            var tarefas = await _tarefaService.ObterTodasTarefasAsync();
            return Ok(tarefas);
        }

        [HttpPatch("{id}/concluir")]
        public async Task<IActionResult> ConcluirTarefa(int id)
        {
            try
            {
                 Tarefa tarefaConcluida = await _tarefaService.MarcarTarefaComoConcluidaAsync(id);
                 return Ok(tarefaConcluida);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
  
        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverTarefa(int id)
        {
             try
             {
                 await _tarefaService.RemoverTarefaAsync(id);
                 return NoContent();
             }
             catch (KeyNotFoundException)
             {
                 return NotFound();
             }
        }
    }
}