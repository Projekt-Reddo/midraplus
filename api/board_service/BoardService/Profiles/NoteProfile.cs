using AutoMapper;
using BoardService.Models;

namespace BoardService.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<NoteGrpc, Note>();
        }
    }
}