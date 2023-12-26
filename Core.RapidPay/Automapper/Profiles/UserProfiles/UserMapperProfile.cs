using AutoMapper;
using DAL.RapidPay.DTO.Identity;
using DAL.RapidPay.DTO.Users;
using DAL.RapidPay.Entities;

namespace Core.RapidPay.Automapper.Profiles.UserProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<CreateUserRequest, User>();

            CreateMap<User, UserResponse>();
        }
    }
}
