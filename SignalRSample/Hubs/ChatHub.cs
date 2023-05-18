using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Data;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace SignalRSample.Hubs
{
	public class ChatHub : Hub
	{

		private readonly ApplicationDbContext _db; 

		public ChatHub( ApplicationDbContext db)
		{
			_db= db;
		}

        public override Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(userId))
            {
                var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserConnected", userId, userName);  
                HubConnections.AddUserConnection(userId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (HubConnections.HasUserConnection(userId, Context.ConnectionId))
            {
                var userConnections = HubConnections.Users[userId];
                userConnections.Remove(Context.ConnectionId);

                HubConnections.Users.Remove(userId);
                if (userConnections.Any())
                {
                    HubConnections.Users.Add(userId, userConnections);
                }
            }

            if (!String.IsNullOrEmpty(userId))
            {
                var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserDisconnected", userId, userName);
                HubConnections.AddUserConnection(userId, Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendAddRoomMessage(int maxRoom, int roomId, string roomName)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;

            await Clients.All.SendAsync("RecieveAddRoomMessage", maxRoom, roomId, roomName, userId, userName);
        }

        //public async Task SendMessageToAll(string user, string message)
        //{
        //	await Clients.All.SendAsync("MessageRecieved", user, message);
        //}

        //[Authorize]
        //public async Task SendMessageToReceiver(string sender, string receiver, string message)
        //{
        //	var userId = _db.Users.FirstOrDefault( u => u.Email.ToLower() == receiver.ToLower()).Id;
        //	if (!string.IsNullOrEmpty(userId))
        //	{
        //		await Clients.User(userId).SendAsync("MessageRecieved", sender, message);
        //	}
        //}

    }
}
