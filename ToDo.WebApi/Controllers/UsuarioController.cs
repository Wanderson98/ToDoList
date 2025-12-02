using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("perfil")]
        public async Task<IActionResult> ObterPerfilUsuario()
        {
            var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int userId)) return Unauthorized();

            userId = int.Parse(idClaim);
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(userId);
            if (usuario == null) return NotFound();

            return Ok(new {usuario.Id, usuario.Nome, usuario.Email});
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDTO request)
        {
            var usuario = await _usuarioService.CriarUsuarioAsync(request);
            return CreatedAtAction(nameof(ObterUsuarioPorId), new { id = usuario.Id }, usuario);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(int id)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);  
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosUsuarios()
        {
            var usuarios = await _usuarioService.ObterTodosUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverUsuario(int id)
        {
            try
            {
                await _usuarioService.RemoverUsuarioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] AtualizarUsuarioDTO atualizarUsuarioDto)
        {
            try
            {
                var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int userId)) return Unauthorized();

                if (userId != id) return Forbid();

                await _usuarioService.AtualizarUsuarioAsync(userId, atualizarUsuarioDto);
                return NoContent();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new {erros = ex.Errors});
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}