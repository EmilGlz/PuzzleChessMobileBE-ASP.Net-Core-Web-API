using ChessMobileBE.Contracts;
using ChessMobileBE.Helpers;
using ChessMobileBE.Models.DTOs.Requests;
using ChessMobileBE.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChessMobileBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IPendingMatchService _pendingMatchService;
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;
        public MatchController(IMatchService matchService, IPendingMatchService pendingMatchService, IUserService userServoce)
        {
            _matchService = matchService;
            _pendingMatchService = pendingMatchService;
            _userService = userServoce;
        }
        [HttpGet]
        [Route("FindRoom")]
        public IActionResult FindRoom(string userId)
        {
            var emptyMatch = _pendingMatchService.GetEmptyMatch(userId);
            if (emptyMatch == null) // add new room and wait...
            {
                var newMatch = _pendingMatchService.AddNewEmptyMatch(userId);
                if (newMatch == null)
                {
                    return BadRequest("Cannot add new match as you are already waiting for opponent to join");
                }
                return Ok(new FindMatchResponse
                {
                    IsPending = true,
                    MatchModel = null,
                    PendingMatchModel = newMatch
                });
            }
            _pendingMatchService.Delete(emptyMatch.Id);
            var match = _matchService.Add(emptyMatch, userId);
            return Ok(new FindMatchResponse
            {
                IsPending = false,
                MatchModel = match,
                PendingMatchModel = null
            });
        }

        [HttpGet]
        [Route("GetMatchById")]
        public IActionResult GetMatchById(string id)
        {
            var match = _matchService.Get(id);
            if (match == null)
            {
                return NotFound("Match not found");
            }
            return Ok(match);
        }

        [HttpDelete]
        [Route("CancelPendingMatch")]
        public IActionResult CancelPendingMatch(string matchId)
        {
            _pendingMatchService.Delete(matchId);
            return Ok();
        }

        [HttpPut]
        [Route("AddMove")]
        public IActionResult AddMove(AddMoveDTO model)
        {
            var room = _matchService.Get(model.RoomId);
            if (room == null)
                return NotFound("Room not found");
            var roomUpdated = _matchService.AddMove(model);
            return Ok(roomUpdated);
        }

        [HttpPut]
        [Route("FinishMatch")]
        public IActionResult FinishMatch(FinishMatchDTO model)
        {
            // check time, if time is finished:
            // check correct moves count
            // return result of moves and add to user records
            var room = _matchService.Get(model.RoomId);
            if (room == null)
                return NotFound("Room not found");
            if (!Helpers.Helpers.IsHostInCurrentOnlineMatch(room, model.UserId).HasValue)
                return NotFound("User not found in the room");
            var hostCorrectMoveCount = Helpers.Helpers.HostCorrectMoveCount(room);
            var clientCorrectMoveCount = Helpers.Helpers.ClientCorrectMoveCount(room);
            bool currentUserIsHost = Helpers.Helpers.IsHostInCurrentOnlineMatch(room, model.UserId).Value;
            WinState winState;
            if (currentUserIsHost)
            {
                if (hostCorrectMoveCount > clientCorrectMoveCount)
                {
                    winState = WinState.Win;
                }
                else if (hostCorrectMoveCount == clientCorrectMoveCount)
                {
                    winState = WinState.Draw;
                }
                else
                {
                    winState = WinState.Lose;
                }
            }
            else
            {
                if (hostCorrectMoveCount < clientCorrectMoveCount)
                {
                    winState = WinState.Win;
                }
                else if (hostCorrectMoveCount == clientCorrectMoveCount)
                {
                    winState = WinState.Draw;
                }
                else
                {
                    winState = WinState.Lose;
                }
            }
            var user = _userService.AddMatchWinState(winState, model.UserId);
            var res = new FinishMatchResponse
            {
                RoomId = room.Id,
                UserCorrectMoveCount = currentUserIsHost ? hostCorrectMoveCount : clientCorrectMoveCount,
                OpponentCorrectMoveCount = currentUserIsHost ? clientCorrectMoveCount : hostCorrectMoveCount,
                VictoryState = winState,
                UserModel = user
            };
            return Ok(res);
        }

    }
}
