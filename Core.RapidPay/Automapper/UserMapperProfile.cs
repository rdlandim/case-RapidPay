using AutoMapper;
using DAL.RapidPay.DTO;
using DAL.RapidPay.Entities;

namespace Core.RapidPay.Automapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
