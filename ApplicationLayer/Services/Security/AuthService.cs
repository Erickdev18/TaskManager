using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;



namespace ApplicationLayer.Services.Security
{
    public class AuthService : IAuthService   
    {
        public Task<string> GetToken(string correo, int edad)
        {
            string key = "67bbc9dadad5def605adeeb96292a9ff45788778b45ed6ec772b8936103679769a188f484d3824b13e8f2ca64a9e78fdface9bdbb0d8271aa47a77c5f0bc2429";
            //deberia de ir en el appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenHandler = new JwtSecurityTokenHandler();
           

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Correo", correo),
                    new Claim("Edad", edad.ToString()),
                }),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = credentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.Run(() => tokenHandler.WriteToken(token));
        }
    }
}
