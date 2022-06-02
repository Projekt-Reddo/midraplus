using AccountService.Dtos;
using AccountService.Models;
using AutoMapper;

namespace AccountService.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<User, AuthReadDto>();
            CreateMap<User, AuthDto>();
        }
    }
}