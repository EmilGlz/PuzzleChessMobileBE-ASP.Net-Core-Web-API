using Microsoft.AspNetCore.SignalR;

namespace ChessMobileBE.Hubs
{
    public class GameHub: Hub
    {
        public Task SendJoinedRoomToUser(string hostId, string clientId)
        {
            return Clients.User(hostId).SendAsync("JoinedTheRoom", clientId);
        }
    }
}
