namespace Interfaces.RapidPay.CreditCards
{
    public interface ICreditCardCreationService
    {
        string GenerateCardNumber();
        int GenerateCardCheckDigit(string cardNumber);
        int GenerateCardCVC();
    }
}
