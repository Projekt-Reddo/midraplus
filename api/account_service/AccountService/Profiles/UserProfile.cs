using AccountService.Dtos;
using AccountService.Models;
using AutoMapper;
using UserService;

namespace AccountService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();

            CreateMap<User, UserGrpc>();
        }
    }
}