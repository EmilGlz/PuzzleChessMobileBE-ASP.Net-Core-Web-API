using ChessMobileBE.Contracts;
using ChessMobileBE.Helpers;
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
        IUserService _userService;
        public GameHub(IMatchService matchService, IUserService userService)
        {
            _matchService = matchService;
            _userService = userService;
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
            var rooms = _matchService.GetRoomsByUserId(userId);
            // add to db for both users
            for (int i = 0; i < rooms.Count; i++)
            {
                var oppId = Helpers.Helpers.OppIdOfRoom(rooms[i], userId);
                _userService.AddMatchWinState(WinState.Lose, userId, rooms[i].Id);
                _userService.AddMatchWinState(WinState.Win, oppId, rooms[i].Id);
                _matchService.Delete(rooms[i].Id);
            }
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
            var room = _matchService.Get(roomId);
            _matchService.Delete(roomId);
            _userService.AddMatchWinState(WinState.Lose, userId, roomId);
            _userService.AddMatchWinState(WinState.Win, receiverId, roomId);
            return Clients.User(receiverId).SendAsync("GaveUp", userId);
        }
    }
}
