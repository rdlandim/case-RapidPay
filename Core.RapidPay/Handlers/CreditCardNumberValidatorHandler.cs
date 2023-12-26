using DAL.RapidPay.DTO.CreditCards;
using Interfaces.RapidPay.CreditCards;

namespace Core.RapidPay.Handlers
{
    public class CreditCardNumberValidatorHandler : PaymentHandlerBase
    {
        private readonly ICreditCardValidationService _validationService;

        public CreditCardNumberValidatorHandler(IPaymentHandler handler, ICreditCardValidationService validationService) : base(handler)
        {
            if (handler != null)
                SetNextHandler(handler);

            _validationService = validationService;
        }

        protected override bool CanHandleRequest(CreditCardPaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            return !_validationService.ValidateCreditCardNumber(request);
        }

        protected override PaymentResponse ProcessRequest(CreditCardPaymentRequest request)
        {
            return new PaymentResponse
            {
                GUID = Guid.NewGuid().ToString(),
                Success = false,
                Message = "Credit Card number is invalid"
            };
        }
    }
}
