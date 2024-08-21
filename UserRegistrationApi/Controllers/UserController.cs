using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Controllers
{
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
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
            var user = await _userService.LoginAsync(username, password);
            if (user == null) 
            { 
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(username, user.Role);
            return Ok(token);
        }
    }
}
