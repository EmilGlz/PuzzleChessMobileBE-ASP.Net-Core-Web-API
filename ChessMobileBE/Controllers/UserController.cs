using AutoMapper;
using ChessMobileBE.Contracts;
using ChessMobileBE.Models.DTOs.Requests;
using ChessMobileBE.Models.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessMobileBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRankingService _rankService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, IRankingService rankService)
        {
            _userService = userService;
            _mapper = mapper;
            _rankService = rankService;
        }

        [HttpPost]
        [Route("LoginAsGuest")]
        [AllowAnonymous]
        public IActionResult LoginAsGuest()
        {
            Random rnd = new Random();
            int num = rnd.Next(1000);
            var user = _userService.Add(new UserDTO {
                Email = "Guest",
                PlayGamesId = "Guest",
                Username = "Player" + num
            });
            return Ok(user);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login(UserDTO model)
        {
            var existingUser = _userService.Login(model.PlayGamesId);
            if(existingUser == null)
            {
                // create user
                var newUser = _userService.Add(model);
                return Ok(newUser);
            }
            return Ok(existingUser);
        }

        [HttpGet]
        [Route("GetByPlayGamesId")]
        public IActionResult GetByPlayGamesId(string userId)
        {
            var existingUser = _userService.GetByPGId(userId);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }
            var res = _mapper.Map<UserViewModel>(existingUser);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetTopRankedUsers")]
        public IActionResult GetTopRankedUsers(int count)
        {
            var users = _userService.GetTopRankedUsers(count);
            var res = new List<UserViewModel>();
            for (int i = 0; i < users.Count; i++)
                res.Add(_mapper.Map<UserViewModel>(users[i]));
            return Ok(res);
        }

        [HttpGet]
        [Route("GetMyRank")]
        public IActionResult GetMyRank(string userId)
        {
            var res = _userService.GetMyRank(userId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(string userId)
        {
            var existingUser = _userService.GetById(userId);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }
            var res = _mapper.Map<UserViewModel>(existingUser);
            return Ok(res);
        }

        [HttpPost]
        [Route("AddMatchCount")]
        public IActionResult AddMatchCount(string userId)
        {
            var res = _userService.AddMatchCount(userId);
            if (res == null)
                return NotFound("User not found");
            return Ok(res);
        }

        [HttpPut]
        [Route("UpdateEnergyForOneDay")]
        public IActionResult UpdateEnergyForOneDay(string userId)
        {
            var res = _userService.UpdateEnergyForOneDay(userId);
            if (res == null)
                return NotFound("User not found");
            return Ok(res);
        }

        [HttpPut]
        [Route("BuyAddRemove")]
        public IActionResult BuyAddRemove(string userId)
        {
            var res = _userService.BuyAddRemove(userId);
            if (res == null)
                return NotFound("User not found");
            return Ok(res);
        }

        [HttpPut]
        [Route("BuyGMPuzzles")]
        public IActionResult BuyGMPuzzles(string userId)
        {
            var res = _userService.BuyGMPuzzles(userId);
            if (res == null)
                return NotFound("User not found");
            return Ok(res);
        }

    }
}
