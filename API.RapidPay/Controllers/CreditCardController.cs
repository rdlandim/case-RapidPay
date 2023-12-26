using DAL.RapidPay.DTO.CreditCards;
using Interfaces.RapidPay.CreditCards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.RapidPay.Controllers
{
    [Route("api/credit-cards")]
    [ApiController]
    [Authorize]
    public class CreditCardController : ControllerBase
    {
        private readonly ICreditCardService _service;
        private readonly IPaymentHandler _paymentHandler;

        public CreditCardController(ICreditCardService service, IPaymentHandler paymentHandler)
        {
            _service = service;
            _paymentHandler = paymentHandler;
        }

        [HttpGet("balance/{Id}")]
        public IActionResult GetBalance([FromRoute] CreditCardBalanceRequest request)
        {
            try
            {
                var response = _service.GetBalance(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateCard([FromBody] CreateCreditCardRequest request)
        {
            try
            {
                return Ok(_service.CreateCard(request));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("payment")]
        public IActionResult Pay([FromBody] CreditCardPaymentRequest request)
        {
            try
            {
                var result = _paymentHandler.Handle(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
