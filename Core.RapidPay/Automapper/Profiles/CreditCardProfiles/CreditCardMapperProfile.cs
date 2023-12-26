using AutoMapper;
using DAL.RapidPay.DTO.CreditCards;
using DAL.RapidPay.Entities;

namespace Core.RapidPay.Automapper.Profiles.CreditCardProfiles
{
    public class CreditCardMapperProfile : Profile
    {
        public CreditCardMapperProfile()
        {
            CreateMap<CreditCard, CreditCardDTO>().ReverseMap();

            CreateMap<CreditCard, CreateCreditCardResponse>().ReverseMap();

            CreateMap<CreateCreditCardRequest, CreditCard>();

            CreateMap<CreditCard, CreditCardBalanceResponse>()
                .ForMember(dest => dest.PreviousBalance, cfg => cfg.MapFrom(src => src.Balance))
                .ReverseMap();
        }
    }
}
