using ChessMobileBE.Models.DTOs.SignalRModels;
using Microsoft.AspNetCore.SignalR;

namespace ChessMobileBE.Hubs
{
    public class GameHub: Hub
    {
        public Task SendJoinedRoomToUser(string hostId, string clientId)
        {
            return Clients.User(hostId).SendAsync("JoinedTheRoom", clientId);
        }
        public Task SendMoveToUser(MoveModel model)
        {
            return Clients.User(model.OpponentId).SendAsync("Moved", model.CorrectMove);
        }
        public Task SendGiveUpToUser(string receiverId, string requesterId)
        {
            return Clients.User(receiverId).SendAsync("GaveUp", requesterId);
        }
    }
}
