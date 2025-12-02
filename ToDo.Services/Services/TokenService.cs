using ToDo.Domain.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;


namespace ToDo.Services.Services
{
    public class TokenService 
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GerarToken(Usuario usuario)
        {
            var keyString = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("Chave JWT não está configurada.");
            
            var key = Encoding.ASCII.GetBytes(keyString);

            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity( new []{
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Email!) 
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            return tokenHandler.WriteToken(token);
        }
    }
}