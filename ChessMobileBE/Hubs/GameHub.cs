using ChessMobileBE.Contracts;
using ChessMobileBE.Models.DTOs.SignalRModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChessMobileBE.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameHub: Hub
    {
        IMatchService _matchService;
        public GameHub(IMatchService matchService)
        {
            _matchService = matchService;
        }
        public override Task OnConnectedAsync()
        {
            //string authorId = Context.User.FindFirst("PlayGamesId").Value;
            return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //_matchService.LoseAllRooms(userId);
            await Clients.All.SendAsync("OnDisconnected", userId);
        }
        public Task SendJoinedRoomToUser(JoinRoomModel joinRoomModel)
        {
            return Clients.User(joinRoomModel.HostId).SendAsync("JoinedTheRoom", joinRoomModel);
        }
        public Task SendMoveToUser(MoveModel model)
        {
            return Clients.User(model.OpponentId).SendAsync("Moved", model);
        }
        public Task SendGiveUpToUser(string receiverId, string roomId)
        {
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var room = _matchService.GiveUp(userId, roomId);
            return Clients.User(receiverId).SendAsync("GaveUp", userId);
        }
    }
}
