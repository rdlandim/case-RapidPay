using DAL.RapidPay.DTO.CreditCards;

namespace Interfaces.RapidPay.CreditCards
{
    public interface ICreditCardValidationService
    {
        bool ValidateCreditCardNumber(CreditCardPaymentRequest request);
        bool ValidateCardOwner(CreditCardPaymentRequest request);
        bool ValidateCreditCardCVC(CreditCardPaymentRequest request);
        bool ValidateCardExpiryDate(CreditCardPaymentRequest request);
    }
}
