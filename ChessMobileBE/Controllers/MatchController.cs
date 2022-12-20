using ChessMobileBE.Contracts;
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
        public MatchController(IMatchService matchService, IPendingMatchService pendingMatchService)
        {
            _matchService = matchService;
            _pendingMatchService = pendingMatchService;
        }
        [HttpGet]
        [Route("FindRoom")]
        public IActionResult FindRoom(string userId)
        {
            var emptyMatch = _pendingMatchService.GetEmptyMatch();
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
            var match = _matchService.Add(emptyMatch.UserId, userId, emptyMatch.PuzzleIndex);
            return Ok(new FindMatchResponse
            {
                IsPending = false,
                MatchModel = match,
                PendingMatchModel = null
            });
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
            var roomUpdated = _matchService.AddMove(model.UserId, model.RoomId, model.CorrectMove);
            return Ok(roomUpdated);
        }
    }
}
