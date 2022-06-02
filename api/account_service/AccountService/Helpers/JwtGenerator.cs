using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AccountService.Models;
using static Constant;

namespace AccountService.Helpers
{
    public interface IJwtGenerator
    {
        Claim[] GenerateClaims(User user);
        string GenerateJwtToken(IEnumerable<Claim> claims);
    }

    public class JwtGenerator : IJwtGenerator
    {
        private readonly string key = "this is my custom Secret key for authnetication";
        public JwtGenerator(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// Generate claims list for user from DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Claim[] GenerateClaims(User user)
        {
            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };

            if (user.IsAdmin == true)
            {
                claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, SystemAuthority.ADMIN)
                };
            }

            return claims;
        }

        /// <summary>
        /// Generate jwt token with input claims in token payload
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            // Declare token and properties
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature
                )
            };

            // Generate token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
