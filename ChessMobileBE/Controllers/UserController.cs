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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login(UserDTO model)
        {
            var existingUser = _userService.Get(model.PlayGamesId);
            if(existingUser == null)
            {
                // create user
                var newUser = _userService.Add(model);
                return Ok(newUser);
            }
            return Ok(existingUser);
        }
    }
}
