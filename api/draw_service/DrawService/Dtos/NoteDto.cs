using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrawService.Dtos
{
    public class NoteReadDto
    {
        public string Id { get; set; } = "";
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public string Text { get; set; } = "";
    }

    public class NoteCreateDto
    {
        public string Id { get; set; } = "";
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public string Text { get; set; } = "";
    }

    public class NoteUpdateDto
    {
        public string Id { get; set; } = "";
        public string Text { get; set; } = "";
    }
}