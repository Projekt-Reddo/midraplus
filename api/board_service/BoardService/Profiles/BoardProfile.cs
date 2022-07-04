using AutoMapper;
using BoardService.Dtos;
using BoardService.Models;
using Google.Protobuf.WellKnownTypes;
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

            CreateMap<Board, BoardLoadDataResponse>().ForMember(dest => dest.BoardId, opt => opt.MapFrom(src => src.Id));

            CreateMap<BoardLoadByTimeRequest, BoardLoadByTime>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateTime()));

            CreateMap<Board, BoardLoadByTimeGrpc>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => (DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)).ToTimestamp()));

        }
    }
}