using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPersonalInformationService _personalInformationService;
        private readonly IUserDtoMapper _userDtoMapper;
        public AdminController(IUserService userService, IPersonalInformationService personalInformationService, IUserDtoMapper userDtoMapper)
        {
            _userService = userService;
            _personalInformationService = personalInformationService;
            _userDtoMapper = userDtoMapper;
        }

        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync()
        {
            var users = await _personalInformationService.GetUsersAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }

            var userDtos = users.Select(_userDtoMapper.Bind);

            return Ok(userDtos);
        }

        [HttpDelete("{id:Guid}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
        {
            await _personalInformationService.DeleteUserAsync(id);
            return Ok();
        }
    }
}
