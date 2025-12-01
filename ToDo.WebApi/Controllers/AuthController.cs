using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CriarUsuarioDTO criarUsuarioDto)
        {
            var usuario = await _usuarioService.CriarUsuarioAsync(criarUsuarioDto);
            return CreatedAtAction(nameof(CadastrarUsuario), new { id = usuario.Id }, usuario);
        }
    }


}