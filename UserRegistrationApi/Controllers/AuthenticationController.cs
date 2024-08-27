using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly IUserValidator _userValidator;
        private readonly IUserService _userService;

        public AuthenticationController(IAuthenticationService authenticationService, IJwtService jwtService, IUserValidator userValidator, IUserService userService)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _userValidator = userValidator;
            _userService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromForm] CreateUserDto userDto)
        {
            var validationResult = _userValidator.ValidateCreateUserDto(userDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            if (await _userService.GetUserAsync(userDto.Username) != null)
            {
                return BadRequest($"{userDto.Username} already exists");
            }

            await _authenticationService.RegisterAsync(userDto);

            return Ok();
        }

        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromHeader(Name = "username")] string username, [FromHeader(Name = "password")] string password)
        {
            var user = await _authenticationService.LoginAsync(username, password);
            if (user == null) 
            { 
                return BadRequest("Incorrect username or password");
            }

            var token = _jwtService.GenerateToken(username, user.UserId, user.Role);
            return Ok(token);
        }
    }
}
