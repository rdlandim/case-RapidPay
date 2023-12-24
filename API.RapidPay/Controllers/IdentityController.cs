using DAL.RapidPay.DTO.Identity;
using Interfaces.RapidPay.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.RapidPay.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("token")]
        [AllowAnonymous] 
        public IActionResult GenerateToken([FromBody] TokenRequest request)
        {
            try
            {
                var response = _identityService.GenerateJwtToken(request);

                if (response == null)
                    return BadRequest(new { Message = "User not found. Provide a valid email and/or password" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error ocurred while trying to authenticate the user." });
            }
        }
    }
}
