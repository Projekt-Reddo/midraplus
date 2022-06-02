using AutoMapper;
using BoardService.Dtos;
using BoardService.Models;
using UserService;

namespace BoardService.Profiles
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Board, BoardReadDto>();

            CreateMap<BoardCreateDto, Board>();

            CreateMap<Board, BoardUpdateDto>();
            CreateMap<BoardUpdateDto, Board>();

            CreateMap<UserGrpc, User>();
        }
    }
}