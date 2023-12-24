using Core.RapidPay.Options;
using DAL.RapidPay.DTO.Identity;
using DAL.RapidPay.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.RapidPay.Services.Identity
{
    public class TokenGenerator
    {
        private readonly JwtOptions _options;

        public TokenGenerator(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public TokenResponse GenerateTokenResponse(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_options.Secret);
            List<Claim> claims = GenerateClaims(user);
            SecurityTokenDescriptor descriptor = GenerateTokenDescriptor(key, claims);

            var token = handler.CreateToken(descriptor);

            var jwt = handler.WriteToken(token);

            return new TokenResponse(jwt);
        }

        private SecurityTokenDescriptor GenerateTokenDescriptor(byte[] key, List<Claim> claims)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.Add(TimeSpan.FromHours(_options.Lifetime)),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
        }

        private static List<Claim> GenerateClaims(User user)
        {
            return new List<Claim> {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Name, user.Name),
                new("userId", user.Id.ToString()),
            };
        }
    }
}
