using AccountService.Dtos;
using AccountService.Models;
using AutoMapper;

namespace AccountService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
        }
    }
}