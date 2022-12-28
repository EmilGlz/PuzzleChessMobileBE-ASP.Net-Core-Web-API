﻿using ChessMobileBE.Models.DTOs.SignalRModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChessMobileBE.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameHub: Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public Task SendJoinedRoomToUser(JoinRoomModel joinRoomModel)
        {
            return Clients.User(joinRoomModel.HostId).SendAsync("JoinedTheRoom", joinRoomModel);
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
