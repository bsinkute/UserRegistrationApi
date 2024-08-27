using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Controllers
{

    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProfilePictureService _profilePictureService;
        private readonly IUserDtoMapper _userDtoMapper;
        private readonly IUserValidator _userValidator;

        public UserController(IUserService userService, IProfilePictureService profilePictureService, IUserDtoMapper userDtoMapper, IUserValidator userValidator)
        {
            _userService = userService;
            _profilePictureService = profilePictureService;
            _userDtoMapper = userDtoMapper;
            _userValidator = userValidator;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserByIdAsync()
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null) 
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var user = await _userService.GetUserAsync(userId);
            if (user == null) return NotFound(userId);

            return Ok(_userDtoMapper.Bind(user));
        }

        [HttpPut("firstName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateFirstNameAsync([FromBody] string firstName)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidateFirstName(firstName);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _userService.UpdateUserFirstNameAsync(userId, firstName);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("surname")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateSurnameAsync([FromBody] string surname)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidateSurname(surname);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _userService.UpdateUserSurnameAsync(userId, surname);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("personalIdentificationNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateIdentificationNumberAsync([FromBody] string personalIdentificationNumber)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidatePersonalIdentificationNumber(personalIdentificationNumber);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _userService.UpdateUserPersonalIdentificationNumberAsync(userId, personalIdentificationNumber);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("phoneNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePhoneNumberAsync([FromBody] string phoneNumber)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidatePhoneNumber(phoneNumber);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _userService.UpdateUserPhoneNumberAsync(userId, phoneNumber);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateEmailAsync([FromBody] string email)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidateEmail(email);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _userService.UpdateUserEmailAsync(userId, email);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("profilePicture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateProfilePictureAsync([FromForm] UpdateProfilePictureDto updateProfilePictureDto)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidateProfilePicture(updateProfilePictureDto?.Image);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var scaledPicture = _profilePictureService.GenerateProfilePicture(updateProfilePictureDto.Image);

            var updatedUser = await _userService.UpdateUserProfilePictureAsync(userId, scaledPicture);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("city")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCityAsync([FromBody] string city)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidateCity(city);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _userService.UpdateUserCityAsync(userId, city);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("street")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStreetAsync([FromBody] string street)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidateStreet(street);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _userService.UpdateUserStreetAsync(userId, street);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("houseNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateHouseNumberAsync([FromBody] string houseNumber)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidateHouseNumber(houseNumber);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _userService.UpdateUserHouseNumberAsync(userId, houseNumber);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("apartmentNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateApartmentNumberAsync([FromBody] string apartmentNumber)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = new Guid(userIdString);

            var validationResult = _userValidator.ValidateHouseNumber(apartmentNumber);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _userService.UpdateUserApartmentNumberAsync(userId, apartmentNumber);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }
    }
}
