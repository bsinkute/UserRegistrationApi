using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Exceptions;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IUserValidator _userValidator;

        public UserController(IUserService userService, IJwtService jwtService, IUserValidator userValidator)
        {
            _userService = userService;
            _jwtService = jwtService;
            _userValidator = userValidator;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromForm] CreateUserDto userDto)
        {
            var validationResult = _userValidator.ValidateCreateUserDto(userDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            try
            {
                await _userService.RegisterAsync(userDto);
            }
            catch (UsernameAlreadyExistsException)
            {
                return BadRequest($"{userDto.Username} already exists");
            }

            return Ok();
        }

        [HttpGet("Login")]
        public async Task<ActionResult> Login([FromHeader(Name = "username")] string username, [FromHeader(Name = "password")] string password)
        {
            var user = await _userService.LoginAsync(username, password);
            if (user == null) 
            { 
                return BadRequest("Incorrect username or password");
            }

            var token = _jwtService.GenerateToken(username, user.UserId, user.Role);
            return Ok(token);
        }
    }
}
