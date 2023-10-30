using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DebtsCompass.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfigurationSection configurationSection;

        private readonly IUserRepository userRepository;
        public JwtService(IConfiguration configuration, IUserRepository userRepository)
        {
            this.configurationSection = configuration.GetSection("JwtConfig");
            this.userRepository = userRepository;
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

        public async Task<RefreshTokenResponse> GetRefreshToken(string email, RefreshTokenRequest refreshTokenRequest)
        {
            User user = await userRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new UserNotFoundException(email);
            }

            if (user.RefreshToken != refreshTokenRequest.RefreshToken || user.RefreshTokenExpireTime <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid token");
            }

            string accesToken = GenerateToken(email);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            return new RefreshTokenResponse
            {
                AccessToken = GenerateToken(email),
                RefreshToken = GenerateRefreshToken()
            };

        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task UpdateRefreshToken(string email, string token)
        {
            User user = await userRepository.GetUserByEmail(email);

            user.RefreshToken = token;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(Convert.ToDouble(configurationSection["RefreshTokenValidityInDays"]));

            await userRepository.Update(user);
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
