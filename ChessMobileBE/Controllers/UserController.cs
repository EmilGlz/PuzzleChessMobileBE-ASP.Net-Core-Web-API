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
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper = null)
        {
            _userService = userService;
            _mapper = mapper;
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
    }
}
