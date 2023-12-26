using DAL.RapidPay.DTO.CreditCards;

namespace Interfaces.RapidPay.CreditCards
{
    public interface ICreditCardService
    {
        CreateCreditCardResponse CreateCard(CreateCreditCardRequest request);
        CreditCardBalanceResponse Pay(CreditCardPaymentRequest request);
        CreditCardBalanceResponse GetBalance(CreditCardBalanceRequest request);
    }
}
