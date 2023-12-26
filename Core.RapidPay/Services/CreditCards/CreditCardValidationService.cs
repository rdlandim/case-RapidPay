using DAL.RapidPay.Context;
using DAL.RapidPay.DTO.CreditCards;
using Interfaces.RapidPay.CreditCards;
using Microsoft.EntityFrameworkCore;

namespace Core.RapidPay.Services.CreditCards
{
    public class CreditCardValidationService : ICreditCardValidationService
    {
        private readonly RapidPayContext _context;
        private readonly ICreditCardCreationService _creationService;

        public CreditCardValidationService(RapidPayContext context, ICreditCardCreationService creationService)
        {
            _context = context;
            _creationService = creationService;
        }

        public bool ValidateCreditCardNumber(CreditCardPaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(nameof(request.CreditCardNumber));

            IsValidLength(request.CreditCardNumber);

            var checkDigit = _creationService.GenerateCardCheckDigit(request.CreditCardNumber).ToString();

            if (request.CreditCardNumber[^1..] != checkDigit)
                return false;

            return true;
        }

        public bool ValidateCardOwner(CreditCardPaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            var user = _context.Users
                .Include(u => u.CreditCards)
                .FirstOrDefault(u => u.Id == request.UserId) ?? throw new InvalidOperationException("User not found");

            return user.IsCreditCardOwner(request.CreditCardNumber);
        }

        public bool ValidateCreditCardCVC(CreditCardPaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            var user = _context.Users
                .Include(u => u.CreditCards)
                .FirstOrDefault(u => u.Id == request.UserId) ?? throw new InvalidOperationException("User not found");

            if (!user.IsCreditCardOwner(request.CreditCardNumber))
                throw new InvalidOperationException("User is not the owner of the provided credit card");

            var creditCard = user.CreditCards.FirstOrDefault(cc => cc.Number.Equals(request.CreditCardNumber)) ?? throw new InvalidOperationException("Credit Card not found");

            return creditCard.Cvc == request.CVC.ToString();
        }

        public bool ValidateCardExpiryDate(CreditCardPaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            var user = _context.Users
                .Include(u => u.CreditCards)
                .FirstOrDefault(u => u.Id == request.UserId) ?? throw new InvalidOperationException("User not found");

            if (!user.IsCreditCardOwner(request.CreditCardNumber))
                throw new InvalidOperationException("User is not the owner of the provided credit card");

            var creditCard = user.CreditCards.FirstOrDefault(cc => cc.Number.Equals(request.CreditCardNumber)) ?? throw new InvalidOperationException("Credit Card not found");

            return creditCard.IsExpired(request.ValidUntil);
        }

        private static void IsValidLength(string cardNumber)
        {
            if (cardNumber.Length < 14 || cardNumber.Length > 15)
                throw new ArgumentException("Card number length is invalid");
        }
    }
}
