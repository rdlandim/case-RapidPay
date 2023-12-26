using AutoMapper;
using Core.RapidPay.Options;
using DAL.RapidPay.Context;
using DAL.RapidPay.DTO.Identity;
using DAL.RapidPay.Entities;
using Interfaces.RapidPay.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.RapidPay.Security;

namespace Core.RapidPay.Services.Identity
{
    public class IdentityService : TokenGenerator, IIdentityService
    {
        private readonly RapidPayContext _context;
        private readonly IMapper _mapper;

        public IdentityService(RapidPayContext context, IOptions<JwtOptions> jwtOptions, IMapper mapper) : base(jwtOptions)
        {
            _context = context;
            _mapper = mapper;
        }

        public UserResponse CreateUser(CreateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var user = _mapper.Map<User>(request);

            if (IsEmailAlreadyRegistered(user.Email))
                throw new InvalidOperationException("Email already registered!");

            _context.Users.Add(user);
            _context.SaveChanges();

            return _mapper.Map<UserResponse>(user);
        }

        public TokenResponse GenerateJwtToken(TokenRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var hashedPassword = Encryption.ToSha256(request.Password);
            var user = _context.Users.FirstOrDefault(u => EF.Functions.Like(u.Email, request.Email) && EF.Functions.Like(u.Password, hashedPassword));

            if (user == null) return null;

            return GenerateTokenResponse(user);
        }

        private bool IsEmailAlreadyRegistered(string email)
        {
            return _context.Users.Any(u => EF.Functions.Like(u.Email, email));
        }
    }
}
