using AutoMapper;
using DAL.RapidPay.DTO.Identity;
using DAL.RapidPay.DTO.Users;
using DAL.RapidPay.Entities;
using Shared.RapidPay.Security;

namespace Core.RapidPay.Automapper.Profiles.UserProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Password, m => m.MapFrom(src => src.Password.ToSha256()));

            CreateMap<User, UserResponse>();
        }
    }
}
