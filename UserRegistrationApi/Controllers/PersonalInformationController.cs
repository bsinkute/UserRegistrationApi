using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Attributes;
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

        public PersonalInformationController(IPersonalInformationService personalInformationService, IProfilePictureService profilePictureService, IUserDtoMapper userDtoMapper)
        {
            _personalInformationService = personalInformationService;
            _profilePictureService = profilePictureService;
            _userDtoMapper = userDtoMapper;
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
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
            if (string.IsNullOrWhiteSpace(surname))
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
            if (string.IsNullOrWhiteSpace(personalIdentificationNumber))
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
            if (updateProfilePictureDto is null)
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
            if (string.IsNullOrWhiteSpace(street))
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
            if (string.IsNullOrWhiteSpace(houseNumber))
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
            if (string.IsNullOrWhiteSpace(apartmentNumber))
            {
                return BadRequest("User data or first name cannot be null or empty.");
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
