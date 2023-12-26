using DAL.RapidPay.DTO.CreditCards;
using Interfaces.RapidPay.CreditCards;

namespace Core.RapidPay.Handlers
{
    public class CreditCardPaymentHandler : PaymentHandlerBase
    {
        private readonly ICreditCardService _creditCardService;

        public CreditCardPaymentHandler(IPaymentHandler handler, ICreditCardService creditCardService) : base(handler)
        {
            if (handler != null)
                SetNextHandler(handler);

            _creditCardService = creditCardService;
        }

        protected override bool CanHandleRequest(CreditCardPaymentRequest request)
        {
            return true;
        }

        protected override PaymentResponse ProcessRequest(CreditCardPaymentRequest request)
        {
            var response = _creditCardService.Pay(request);

            return new PaymentResponse
            {
                GUID = Guid.NewGuid().ToString(),
                Success = true,
                Message = "Payment Sucessful!",
                Balance = response
            };
        }
    }
}
