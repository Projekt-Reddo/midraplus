using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrawService.Helpers
{
    public static class Constant
    {
        public static class HubReturnMethod
        {
            public const string ReceiveShape = "ReceiveShape";
            public const string ReceiveMouse = "ReceiveMouse";
            public const string OnlineUsers = "OnlineUsers";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string ReceiveNewNote = "ReceiveNewNote";
            public const string ReceiveUpdateNote = "ReceiveUpdateNote";
            public const string ReceiveDeleteNote = "ReceiveDeleteNote";
            public const string LoadNotes = "LoadNotes";
            public const string ClearAll = "ClearAll";
            public const string ReceiveUndo = "ReceiveUndo";
            public const string ReceiveRedo = "ReceiveRedo";
        }
    }
}