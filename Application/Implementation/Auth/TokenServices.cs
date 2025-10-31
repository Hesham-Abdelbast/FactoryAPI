using Application.Interface.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Implementation.Auth
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _config;
        public TokenServices(IConfiguration configuration)
        {
            _config = configuration;
        }
        public string CreateJWTToken(IdentityUser user, List<string>? roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            #region Check JWT Values
            var keyString = _config["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT Key is missing in configuration.");

            var issuerString = _config["Jwt:Issuer"];
            if (string.IsNullOrEmpty(issuerString))
                throw new Exception("JWT Issuer is missing in configuration.");

            var audienceString = _config["Jwt:Audience"];
            if (string.IsNullOrEmpty(audienceString))
                throw new Exception("JWT Audience is missing in configuration.");
            #endregion

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: issuerString,
                audience: audienceString,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
