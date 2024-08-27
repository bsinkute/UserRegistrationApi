using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Exceptions;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly IUserValidator _userValidator;

        public UserController(IAuthenticationService authenticationService, IJwtService jwtService, IUserValidator userValidator)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _userValidator = userValidator;
        }

        [HttpPost("Register")]
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

            try
            {
                await _authenticationService.RegisterAsync(userDto);
            }
            catch (UsernameAlreadyExistsException)
            {
                return BadRequest($"{userDto.Username} already exists");
            }

            return Ok();
        }

        [HttpGet("Login")]
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
