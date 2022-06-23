using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrawService.Dtos
{
    public class BoardReadDto
    {
        public string Id { get; set; } = "";
        public ICollection<ShapeReadDto> Shapes { get; set; } = null!;

        public ICollection<NoteReadDto> Notes { get; set; } = null!;
    }
}