using AutoMapper;
using DAL.RapidPay.DTO;
using DAL.RapidPay.Entities;

namespace Core.RapidPay.Automapper
{
    public class CreditCardMapperProfile : Profile
    {
        public CreditCardMapperProfile()
        {
            CreateMap<CreditCard, CreditCardDTO>().ReverseMap();
        }
    }
}
