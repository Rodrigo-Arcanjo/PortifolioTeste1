using Microsoft.IdentityModel.Tokens;
using PortifolioTeste1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PortifolioTeste1.Security
{
    public class TokenService
    {
        public string Generate (User user) 
        {
            //  Cria uma instÂncia do JwtSecurityTokenHandler
            var handler = new JwtSecurityTokenHandler ();

            var key = Encoding.UTF8.GetBytes(Configuration.privateKey);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
            };

            //  Gera um token
            var token = handler.CreateToken(tokenDescriptor);

            //  Gera uma string do Token
            var strToken = handler.WriteToken(token);

            return strToken;
        }

        private static ClaimsIdentity GenerateClaims (User user)
        {
            var ci = new ClaimsIdentity ();

            ci.AddClaim(new Claim(
                ClaimTypes.Name, 
                user.MatriculaAgente));

            ci.AddClaim(new Claim(
                ClaimTypes.Name,
                user.senha));

            foreach (var role in user.Roles)
                ci.AddClaim(new Claim(
                    ClaimTypes.Role, role));

            //  EXEMPLO DE RETORNO
            ci.AddClaim(new Claim("Carro", "Prisma"));

            return ci;
        }

    }
}
