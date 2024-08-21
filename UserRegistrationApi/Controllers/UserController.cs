using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Controllers
{
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(CreateUserDto userDto)
        {
            await _userService.RegisterAsync(userDto);

            return Ok();
        }

        [HttpGet("Login")]
        public async Task<ActionResult> Login(string username, string password)
        {

            /*if (_userService.Login(username, password, out string role))
            {
                return Ok(_jwtService.GenerateToken(username, role));
            }*/
            return Unauthorized();
        }
    }
}
