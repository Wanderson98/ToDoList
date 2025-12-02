using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;
using ToDo.Services.Services;

namespace ToDo.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly TokenService _tokenService;

        public AuthController(IUsuarioService usuarioService, TokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CriarUsuarioDTO criarUsuarioDto)
        {
            var usuario = await _usuarioService.CriarUsuarioAsync(criarUsuarioDto);
            return CreatedAtAction(nameof(CadastrarUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var usuario = await _usuarioService.AutenticarUsuarioAsync(loginDto);
                var token = _tokenService.GerarToken(usuario);
                return Ok(new { Token = token });
            }
            catch (ValidationException)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }
            
        }
    }


}