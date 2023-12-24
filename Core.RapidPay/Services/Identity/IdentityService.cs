using Core.RapidPay.Options;
using DAL.RapidPay.Context;
using DAL.RapidPay.DTO.Identity;
using Interfaces.RapidPay.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Core.RapidPay.Services.Identity
{
    public class IdentityService : TokenGenerator, IIdentityService
    {
        private RapidPayContext _context;

        public IdentityService(RapidPayContext context, IOptions<JwtOptions> jwtOptions) : base(jwtOptions)
        {
            _context = context;
        }

        public TokenResponse GenerateJwtToken(TokenRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var user = _context.Users.FirstOrDefault(u => EF.Functions.Like(u.Email, request.Email));

            if (user == null) return null;

            return GenerateTokenResponse(user);
        }
    }
}
