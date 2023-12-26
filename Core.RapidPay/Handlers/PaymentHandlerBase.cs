using DAL.RapidPay.DTO.CreditCards;
using Interfaces.RapidPay.CreditCards;

namespace Core.RapidPay.Handlers
{
    public abstract class PaymentHandlerBase : IPaymentHandler
    {
        private IPaymentHandler _next;

        public PaymentHandlerBase(IPaymentHandler handler)
        {
            SetNextHandler(handler);
        }

        public IPaymentHandler SetNextHandler(IPaymentHandler handler)
        {
            _next = handler;

            return handler;
        }

        public virtual PaymentResponse Handle(CreditCardPaymentRequest request)
        {
            PaymentResponse response;

            if (CanHandleRequest(request))
                response = ProcessRequest(request);
            else
                response = _next?.Handle(request);

            return response;
        }

        protected abstract bool CanHandleRequest(CreditCardPaymentRequest request);
        protected abstract PaymentResponse ProcessRequest(CreditCardPaymentRequest request);
    }
}
