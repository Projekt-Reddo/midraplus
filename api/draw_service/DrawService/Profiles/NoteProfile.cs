using AutoMapper;
using BoardService;
using DrawService.Dtos;
using DrawService.Models;

namespace DrawService.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteReadDto>();
            CreateMap<NoteCreateDto, Note>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<NoteCreateDto, NoteReadDto>();

            CreateMap<NoteReadDto, NoteGrpc>();
            CreateMap<NoteGrpc, NoteReadDto>();
        }
    }
}