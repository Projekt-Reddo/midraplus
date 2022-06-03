using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        }
    }
}