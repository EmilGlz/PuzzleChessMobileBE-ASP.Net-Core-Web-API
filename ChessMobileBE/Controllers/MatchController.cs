﻿using ChessMobileBE.Contracts;
using Microsoft.AspNetCore.Http;
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
                return Ok(newMatch);
            }
            _pendingMatchService.DeleteMatch(emptyMatch.Id);
            var match = _matchService.Add(emptyMatch.UserId, userId, emptyMatch.PuzzleIndex);
            return Ok(match);
        }
    }
}
