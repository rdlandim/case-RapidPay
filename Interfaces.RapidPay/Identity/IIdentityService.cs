using DAL.RapidPay.DTO.Identity;

namespace Interfaces.RapidPay.Identity
{
    public interface IIdentityService
    {
        TokenResponse GenerateJwtToken(TokenRequest request);
    }
}
