using AdminService.Dtos;
using AutoMapper;
using BoardService;
using Google.Protobuf.WellKnownTypes;


namespace AdminService.Profiles
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<BoardLoadByTimeGrpc, BoardLoadByTime>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime()));
        }
    }
}