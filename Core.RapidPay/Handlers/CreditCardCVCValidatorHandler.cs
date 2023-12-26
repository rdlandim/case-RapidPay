using DAL.RapidPay.DTO.CreditCards;
using Interfaces.RapidPay.CreditCards;

namespace Core.RapidPay.Handlers
{
    public class CreditCardCVCValidatorHandler : PaymentHandlerBase
    {
        private readonly ICreditCardValidationService _validationService;

        public CreditCardCVCValidatorHandler(IPaymentHandler handler, ICreditCardValidationService validationService) : base(handler)
        {
            if (handler != null)
                SetNextHandler(handler);

            _validationService = validationService;
        }

        protected override bool CanHandleRequest(CreditCardPaymentRequest request)
        {
            return !_validationService.ValidateCreditCardCVC(request);
        }

        protected override PaymentResponse ProcessRequest(CreditCardPaymentRequest request)
        {
            return new PaymentResponse
            {
                GUID = Guid.NewGuid().ToString(),
                Success = false,
                Message = "Credit Card CVC is invalid"
            };
        }
    }
}
