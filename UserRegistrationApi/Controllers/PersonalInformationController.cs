using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Controllers
{

    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class PersonalInformationController : ControllerBase
    {
        private readonly IPersonalInformationService _personalInformationService;
        private readonly IProfilePictureService _profilePictureService;
        private readonly IUserDtoMapper _userDtoMapper;
        private readonly IUserValidator _userValidator;

        public PersonalInformationController(IPersonalInformationService personalInformationService, IProfilePictureService profilePictureService, IUserDtoMapper userDtoMapper, IUserValidator userValidator)
        {
            _personalInformationService = personalInformationService;
            _profilePictureService = profilePictureService;
            _userDtoMapper = userDtoMapper;
            _userValidator = userValidator;
        }

        [HttpGet("{userId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserByIdAsync([FromRoute] Guid userId)
        {
            var user = await _personalInformationService.GetUserAsync(userId);
            if (user == null) return NotFound(userId);

            return Ok(_userDtoMapper.Bind(user));
        }

        [HttpPut("{userId:Guid}/firstName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateFirstNameAsync([FromRoute] Guid userId, [FromBody] string firstName)
        {
            var validationResult = _userValidator.ValidateFirstName(firstName);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _personalInformationService.UpdateUserFirstNameAsync(userId, firstName);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("{userId:Guid}/surname")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateSurnameAsync([FromRoute] Guid userId, [FromBody] string surname)
        {
            var validationResult = _userValidator.ValidateSurname(surname);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _personalInformationService.UpdateUserSurnameAsync(userId, surname);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("{userId:Guid}/personalIdentificationNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateIdentificationNumberAsync([FromRoute] Guid userId, [FromBody] string personalIdentificationNumber)
        {
            var validationResult = _userValidator.ValidatePersonalIdentificationNumber(personalIdentificationNumber);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _personalInformationService.UpdateUserPersonalIdentificationNumberAsync(userId, personalIdentificationNumber);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("{userId:Guid}/phoneNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePhoneNumberAsync([FromRoute] Guid userId, [FromBody] string phoneNumber)
        {
            var validationResult = _userValidator.ValidatePhoneNumber(phoneNumber);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _personalInformationService.UpdateUserPhoneNumberAsync(userId, phoneNumber);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("{userId:Guid}/email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateEmailAsync([FromRoute] Guid userId, [FromBody] string email)
        {
            var validationResult = _userValidator.ValidateEmail(email);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _personalInformationService.UpdateUserEmailAsync(userId, email);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("{userId:Guid}/profilePicture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateProfilePictureAsync([FromRoute] Guid userId, [FromForm] UpdateProfilePictureDto updateProfilePictureDto)
        {
            var validationResult = _userValidator.ValidateProfilePicture(updateProfilePictureDto?.Image);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var scaledPicture = _profilePictureService.GenerateProfilePicture(updateProfilePictureDto.Image);

            var updatedUser = await _personalInformationService.UpdateUserProfilePictureAsync(userId, scaledPicture);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("{userId:Guid}/city")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCityAsync([FromRoute] Guid userId, [FromBody] string city)
        {
            var validationResult = _userValidator.ValidateCity(city);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _personalInformationService.UpdateUserCityAsync(userId, city);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("{userId:Guid}/street")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStreetAsync([FromRoute] Guid userId, [FromBody] string street)
        {
            var validationResult = _userValidator.ValidateStreet(street);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _personalInformationService.UpdateUserStreetAsync(userId, street);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("{userId:Guid}/houseNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateHouseNumberAsync([FromRoute] Guid userId, [FromBody] string houseNumber)
        {
            var validationResult = _userValidator.ValidateHouseNumber(houseNumber);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _personalInformationService.UpdateUserHouseNumberAsync(userId, houseNumber);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }

        [HttpPut("{userId:Guid}/apartmentNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateApartmentNumberAsync([FromRoute] Guid userId, [FromBody] string apartmentNumber)
        {
            var validationResult = _userValidator.ValidateHouseNumber(apartmentNumber);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join("\r\n", validationResult.Errors));
            }

            var updatedUser = await _personalInformationService.UpdateUserApartmentNumberAsync(userId, apartmentNumber);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok();
        }
    }
}
