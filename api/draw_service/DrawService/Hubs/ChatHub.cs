using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrawService.Dtos;
using Microsoft.AspNetCore.SignalR;
using static DrawService.Helpers.Constant;

namespace DrawService.Hubs
{

    public class ChatHub : Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string, DrawConnection> _connections;
        public ChatHub(IDictionary<string, DrawConnection> connections)
        {
            _botUser = "What I'm I doing here?";
            _connections = connections;
        }

        /// <summary>
        /// Join a room
        /// </summary>
        /// <param name="drawConnection"></param>
        /// <returns></returns>
        public async Task JoinRoom(DrawConnection drawConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, drawConnection.BoardId);

            _connections[Context.ConnectionId] = drawConnection;
        }

        /// <summary>
        /// Leave a room
        /// </summary>
        /// <returns></returns>
        public async Task LeaveRoom()
        {
            if (_connections.TryGetValue(Context.ConnectionId, out DrawConnection? userConnection))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userConnection.BoardId);

                _connections.Remove(Context.ConnectionId);
            }
        }

        /// <summary>
        /// Send message to group of user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(UserConnectionInfo user, string message)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out DrawConnection? userConnection))
            {
                await Clients.Group(userConnection.BoardId).SendAsync(HubReturnMethod.ReceiveMessage, user, message, DateTime.Now);
            }
        }
    }
}