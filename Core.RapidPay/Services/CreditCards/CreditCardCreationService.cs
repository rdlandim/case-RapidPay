using Interfaces.RapidPay.CreditCards;

namespace Core.RapidPay.Services.CreditCards
{
    public class CreditCardCreationService : ICreditCardCreationService
    {
        public int GenerateCardCheckDigit(string cardNumber)
        {
            IsValidLength(cardNumber);

            if (cardNumber.Length == 15)
                cardNumber = cardNumber[..^1];

            var sum = 0;
            var multiplier = 2;
            var signal = 1;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                var value = int.Parse(cardNumber[i].ToString()) * multiplier;

                if (value > 9)
                    value -= 9;

                sum += value;

                signal *= -1;
                multiplier += signal;
            }

            return (sum % 10 == 0) ? 0 : (10 - sum % 10);
        }

        public string GenerateCardNumber()
        {
            var rnd = new Random();
            var creditCardNumber = string.Empty;

            for (int i = 0; i < 14; i++)
                creditCardNumber += rnd.Next(0, 10).ToString();

            return creditCardNumber;
        }

        public int GenerateCardCVC()
        {
            var rnd = new Random();
            return rnd.Next(100, 1000);
        }

        private static void IsValidLength(string cardNumber)
        {
            if (cardNumber.Length < 14 || cardNumber.Length > 15)
                throw new ArgumentException("Card number length is invalid");
        }
    }
}
