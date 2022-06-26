using AutoMapper;
using BoardService;
using DrawService.Dtos;
using DrawService.Models;

namespace DrawService.Profiles
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<BoardLoadDataResponse, BoardReadDto>();
        }
    }
}