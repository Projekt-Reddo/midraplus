using AutoMapper;
using BoardService;
using DrawService.Dtos;
using DrawService.Services;
using Microsoft.AspNetCore.SignalR;
using static DrawService.Helpers.Constant;

namespace DrawService.Hubs
{
    public class BoardHub : Hub
    {
        private readonly IDictionary<string, DrawConnection> _connections; // Connection list map to board
        private readonly IMapper _mapper;
        private readonly IDictionary<string, ICollection<ShapeReadDto>> _shapeList; // Shape list map to board
        private readonly IDictionary<string, List<NoteReadDto>> _noteList; // Note list map to board
        private readonly IGrpcBoardClient _grpcBoardClient; // Grpc client to Board service

        public BoardHub(
            IDictionary<string, DrawConnection> connections,
            IMapper mapper,
            IDictionary<string, ICollection<ShapeReadDto>> shapeList,
            IDictionary<string, List<NoteReadDto>> noteList,
            IGrpcBoardClient grpcBoardClient)
        {
            _connections = connections;
            _mapper = mapper;
            _shapeList = shapeList;
            _noteList = noteList;
            _grpcBoardClient = grpcBoardClient;
        }

        #region Join & Leave Room

        /// <summary>
        /// User join room and add info to connection list
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        public async Task JoinRoom(DrawConnection connection)
        {
            _connections[Context.ConnectionId] = connection;

            // Load old notes data & create temp note list for that user
            await LoadNotesFromDb(connection.BoardId);

            // Create new list to save shapes for that room
            NewShapeList(connection.BoardId);

            // Create new list to save notes for that room
            NewNoteList(connection.BoardId);

            await Groups.AddToGroupAsync(Context.ConnectionId, connection.BoardId);
        }

        /// <summary>
        /// Leave room and remove info from connection list
        /// </summary>
        /// <returns></returns>
        public async Task LeaveRoom()
        {
            await HandleUserLeaveRoom();
        }

        /// <summary>
        /// Handle out room when user lost connection
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await HandleUserLeaveRoom();

            await base.OnConnectedAsync();
        }

        protected async Task HandleUserLeaveRoom()
        {
            if (_connections.TryGetValue(Context.ConnectionId, out var connection))
            {
                _connections.Remove(Context.ConnectionId);

                // Save Board Info To Db
                await SaveBoardInfoToDb(connection);

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.BoardId);
            }

            if (connection != null)
            {
                await CurrentOnlineUser(connection.BoardId);
            }
        }

        #endregion

        #region Current online user

        /// <summary>
        /// Number of online people
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task CurrentOnlineUser(string boardId)
        {
            var users = _connections.Values.Where(user => user.BoardId == boardId).Select(user => user.User);
            await Clients.Group(boardId).SendAsync(HubReturnMethod.OnlineUsers, users);
        }

        #endregion

        #region Shape

        /// <summary>
        /// When user draw to board
        /// </summary>
        /// <param name="shape">What user draw (line, text, eraser)</param>
        /// <returns></returns>
        public async Task DrawShape(ShapeReadDto shape)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out DrawConnection? drawConnection))
            {
                var existShape = _shapeList[drawConnection.BoardId].FirstOrDefault(s => s.Id == shape.Id);

                if (existShape is null)
                {
                    _shapeList[drawConnection.BoardId].Add(shape);
                }

                await Clients.OthersInGroup(drawConnection.BoardId).SendAsync(HubReturnMethod.ReceiveShape, shape);
            }
        }

        #endregion

        #region Note

        /// <summary>
        /// Load old notes of a board from active noteList
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task LoadNotes(string boardId)
        {
            if (_noteList.TryGetValue(boardId, out List<NoteReadDto>? notes))
            {
                await Clients.Caller.SendAsync(HubReturnMethod.LoadNotes, _noteList[boardId]);
            }
        }

        /// <summary>
        /// When user create new note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public async Task NewNote(NoteCreateDto note)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out DrawConnection? drawConnection))
            {
                var existNote = _noteList[drawConnection.BoardId].FirstOrDefault(s => s.Id == note.Id);

                if (existNote is null)
                {
                    var newNote = _mapper.Map<NoteReadDto>(note);

                    _noteList[drawConnection.BoardId].Add(newNote);
                }

                await Clients.OthersInGroup(drawConnection.BoardId).SendAsync(HubReturnMethod.ReceiveNewNote, note);
            }
        }

        /// <summary>
        /// When user change something in a note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public async Task UpdateNote(NoteUpdateDto note)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out DrawConnection? drawConnection))
            {
                _noteList[drawConnection.BoardId].ForEach(n =>
                {
                    if (n.Id == note.Id)
                    {
                        n.Text = note.Text;
                    }
                });

                await Clients.OthersInGroup(drawConnection.BoardId).SendAsync(HubReturnMethod.ReceiveUpdateNote, note);
            }
        }

        /// <summary>
        /// When user delete a note
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public async Task DeleteNote(string noteId)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out DrawConnection? drawConnection))
            {
                _noteList[drawConnection.BoardId] = _noteList[drawConnection.BoardId].Where((s) => s.Id != noteId).ToList();

                await Clients.OthersInGroup(drawConnection.BoardId).SendAsync(HubReturnMethod.ReceiveDeleteNote, noteId);
            }
        }

        #endregion

        #region Database

        /// <summary>
        /// Load old notes from Database
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task LoadBoardFromDb(string boardId)
        {
            await LoadShapesFromDb(boardId);
            await LoadNotesFromDb(boardId);
        }

        /// <summary>
        /// Load old shapes from Database
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task LoadShapesFromDb(string boardId)
        {
            if (!_shapeList.ContainsKey(boardId))
            {
                _shapeList[boardId] = new List<ShapeReadDto>();
            }
        }

        /// <summary>
        /// Load old notes from Database
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task LoadNotesFromDb(string boardId)
        {
            if (!_noteList.ContainsKey(boardId))
            {
                _noteList[boardId] = new List<NoteReadDto>();
            }
        }

        /// <summary>
        /// Save current board info to Database
        /// </summary>
        /// <param name="drawConnection"></param>
        /// <returns></returns>
        protected async Task SaveBoardInfoToDb(DrawConnection drawConnection)
        {
            var remainingConnections = _connections.Values.Where(x => x.BoardId == drawConnection.BoardId);

            // Last connection of a room
            if (remainingConnections.Count() == 0)
            {
                // Code to handle DB here
                var shapeGrpcs = _mapper.Map<ICollection<ShapeGrpc>>(_shapeList[drawConnection.BoardId]);
                var noteGrpcs = _mapper.Map<ICollection<NoteGrpc>>(_noteList[drawConnection.BoardId]);

                // Save shapes and notes to Board service
                var rs = await _grpcBoardClient.SaveBoardData(drawConnection.BoardId, shapeGrpcs, noteGrpcs);

                // Remove active shape and node
                _shapeList.Remove(drawConnection.BoardId);
                _noteList.Remove(drawConnection.BoardId);
            }
        }

        #endregion

        #region New storage dictionary for shapes and notes

        /// <summary>
        /// Create new element of ShapeList with boardId is key
        /// </summary>
        /// <param name="boardId"></param>
        protected void NewShapeList(string boardId)
        {
            if (!_shapeList.ContainsKey(boardId))
            {
                _shapeList[boardId] = new List<ShapeReadDto>();
            }
        }

        /// <summary>
        /// Create new element of NoteList with boardId is key
        /// </summary>
        /// <param name="noteId"></param>
        protected void NewNoteList(string noteId)
        {
            if (!_noteList.ContainsKey(noteId))
            {
                _noteList[noteId] = new List<NoteReadDto>();
            }
        }

        #endregion
    }
}