using AutoMapper;
using Core.RapidPay.Handlers;
using DAL.RapidPay.Context;
using DAL.RapidPay.DTO.CreditCards;
using DAL.RapidPay.Entities;
using Interfaces.RapidPay.CreditCards;
using Interfaces.RapidPay.UFE;
using Microsoft.EntityFrameworkCore;

namespace Core.RapidPay.Services.CreditCards
{
    public class CreditCardService : ICreditCardService
    {
        private readonly RapidPayContext _context;
        private readonly IMapper _mapper;
        private readonly IUFEService _ufeService;
        private readonly ICreditCardCreationService _creationService;

        public CreditCardService(RapidPayContext context, IMapper mapper, IUFEService ufeService, ICreditCardCreationService creditCardCreationService)
        {
            _context = context;
            _mapper = mapper;
            _ufeService = ufeService;
            _creationService = creditCardCreationService;
        }

        public CreateCreditCardResponse CreateCard(CreateCreditCardRequest request)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            var user = _context.Users.FirstOrDefault(u => u.Id == request.UserId) ?? throw new InvalidOperationException("User not found");

            var validUntil = DateTime.Today.AddYears(5);

            var creditCard = new CreditCard(user.Id, CreateCreditCardNumber(), $"{validUntil.Year}-{validUntil.Month}", _creationService.GenerateCardCVC().ToString());

            _context.CreditCards.Add(creditCard);
            _context.SaveChanges();

            return _mapper.Map<CreateCreditCardResponse>(creditCard);
        }

        public CreditCardBalanceResponse GetBalance(CreditCardBalanceRequest request)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            var creditCard = _context.CreditCards.FirstOrDefault(cc => cc.Id == request.Id) ?? throw new InvalidOperationException("Credit card not found");

            return _mapper.Map<CreditCardBalanceResponse>(creditCard);
        }

        public CreditCardBalanceResponse Pay(CreditCardPaymentRequest request)
        {
            var user = _context.Users
                .Include(u => u.CreditCards)
                .FirstOrDefault(u => u.Id == request.UserId) ?? throw new InvalidOperationException("User not found");

            var creditCard = user.CreditCards.FirstOrDefault(cc => cc.Number == request.CreditCardNumber) ?? throw new InvalidOperationException("Credit card not found");

            var previousBalance = creditCard.Balance;
            var valueWithFee = (_ufeService.GetFee(request.ForceUpdateFee) * request.Value) + request.Value;

            creditCard.Balance += decimal.Round(valueWithFee, 2, MidpointRounding.AwayFromZero);

            _context.Update(creditCard);
            _context.SaveChanges();

            return new CreditCardBalanceResponse
            {
                Balance = creditCard.Balance,
                PreviousBalance = previousBalance
            };
        }

        public string CreateCreditCardNumber()
        {
            var creditCardNumber = _creationService.GenerateCardNumber();
            creditCardNumber += _creationService.GenerateCardCheckDigit(creditCardNumber);

            return creditCardNumber;
        }
    }
}
