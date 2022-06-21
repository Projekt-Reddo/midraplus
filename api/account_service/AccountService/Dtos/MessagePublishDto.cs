using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Dtos
{
    public class MessageCreateBoardPublishDto
    {
        // user's Id
        public string Id { get; set; }
        // name of the board
        public string Name { get; set; }
        public string Event { get; set; }

    }

    public class MessageAddSiginPublishDto
    {
        public string Event { get; set; }
    }
}