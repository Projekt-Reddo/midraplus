using AutoMapper;
using DrawService.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace DrawService.Hubs
{
    public class BoardHub : Hub
    {
        private readonly IDictionary<string, DrawConnections> _connections; // Connection list map to board
        private readonly IMapper _mapper;
        private readonly IDictionary<string, ICollection<ShapeReadDto>> _shapeList; // Shape list map to board

        public BoardHub(
            IDictionary<string, DrawConnections> connections,
            IMapper mapper,
            IDictionary<string, ICollection<ShapeReadDto>> shapeList)
        {
            _connections = connections;
            _mapper = mapper;
            _shapeList = shapeList;
        }

        /// <summary>
        /// User join room and add info to connection list
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        public async Task JoinRoom(DrawConnections connections)
        {
            _connections[Context.ConnectionId] = connections;

            await Groups.AddToGroupAsync(Context.ConnectionId, connections.BoardId);
        }

        /// <summary>
        /// Leave room and remove info from connection list
        /// </summary>
        /// <returns></returns>
        public async Task LeaveRoom()
        {
            if (_connections.TryGetValue(Context.ConnectionId, out var connections))
            {
                _connections.Remove(Context.ConnectionId);

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connections.BoardId);
            }
        }
    }
}