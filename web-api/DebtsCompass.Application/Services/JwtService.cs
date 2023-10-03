using DebtsCompass.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DebtsCompass.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfigurationSection configurationSection;

        public JwtService(IConfiguration configuration)
        {
            this.configurationSection = configuration.GetSection("JwtConfig");
        }

        public string GenerateToken(string email)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var signingCredentials = GetSigningCredentials();
            var claims = SetClaims(email);
            var tokenDescriptor = GenerateTokenDescriptor(signingCredentials, claims);

            SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GenerateTokenDescriptor(SigningCredentials signingCredentials, List<Claim> claims)
        {
            return new SecurityTokenDescriptor
            {
                Issuer = "Backend",
                Audience = "Frontend",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(Convert.ToDouble(configurationSection["ExpiresIn"])),
                SigningCredentials = signingCredentials
            };
        }

        private List<Claim> SetClaims(string email)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(configurationSection["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            return signingCredentials;
        }
    }
}
