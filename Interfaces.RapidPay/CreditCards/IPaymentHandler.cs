using DAL.RapidPay.DTO.CreditCards;

namespace Interfaces.RapidPay.CreditCards
{
    public interface IPaymentHandler
    {
        IPaymentHandler SetNextHandler(IPaymentHandler handler);
        PaymentResponse Handle(CreditCardPaymentRequest request);
    }
}
