using DrawService.Dtos;
using Microsoft.AspNetCore.SignalR;
using static DrawService.Helpers.Constant;

namespace DrawService.Hubs
{

    public class ChatHub : Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string, ChatConnection> _connections;
        public ChatHub(IDictionary<string, ChatConnection> connections)
        {
            _botUser = "What I'm I doing here?";
            _connections = connections;
        }

        #region Jon & Leave room

        /// <summary>
        /// Join a room
        /// </summary>
        /// <param name="drawConnection"></param>
        /// <returns></returns>
        public async Task JoinRoom(ChatConnection drawConnection)
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
            await HandleUserLeaveRoom();
        }

        /// <summary>
        /// Out put the user leave room when connection is lost
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await HandleUserLeaveRoom();

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Handle leave room and remove the user from the store dictionary
        /// </summary>
        /// <returns></returns>
        private async Task HandleUserLeaveRoom()
        {
            if (_connections.TryGetValue(Context.ConnectionId, out ChatConnection? userConnection))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userConnection.BoardId);

                _connections.Remove(Context.ConnectionId);
            }
        }

        #endregion

        #region Chat

        /// <summary>
        /// Send message to group of user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(UserConnectionInfo user, string message)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out ChatConnection? userConnection))
            {
                await Clients.Group(userConnection.BoardId).SendAsync(HubReturnMethod.ReceiveMessage, user, message, DateTime.Now);
            }
        }

        #endregion
    }
}