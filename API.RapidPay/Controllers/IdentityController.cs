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

        /// <summary>
        /// Endpoint responsible for authenticating Users using
        /// the provided email and password and generating a JWT Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns> JSON envelope with the generated Token </returns>
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
            catch (Exception)
            {
                return BadRequest(new { Message = "An error ocurred while trying to authenticate the user." });
            }
        }

        /// <summary>
        /// Endpoint responsible for the creation of users
        /// </summary>
        /// <param name="request"></param>
        /// <returns> The created user </returns>
        [HttpPost("user")]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                var response = _identityService.CreateUser(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
