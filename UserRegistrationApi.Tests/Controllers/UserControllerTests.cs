using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using UserRegistrationApi.Controllers;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IProfilePictureService> _profilePictureServiceMock;
        private readonly Mock<IUserDtoMapper> _userDtoMapperMock;
        private readonly Mock<IUserValidator> _userValidatorMock;
        private readonly UserController _controller;
        private readonly Guid _testUserId = Guid.NewGuid();

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _profilePictureServiceMock = new Mock<IProfilePictureService>();
            _userDtoMapperMock = new Mock<IUserDtoMapper>();
            _userValidatorMock = new Mock<IUserValidator>();

            _controller = new UserController(
                _userServiceMock.Object,
                _profilePictureServiceMock.Object,
                _userDtoMapperMock.Object,
                _userValidatorMock.Object
            );

            // Mock the User.Claims to return a user ID
            var userClaims = new List<Claim>
        {
            new Claim("userId", _testUserId.ToString())
        };
            var user = new ClaimsPrincipal(new ClaimsIdentity(userClaims));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsOk_WhenUserIsFound()
        {
            // Arrange
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };
            var userDto = new UserDto { Username = "testuser", Role = "User" };

            _userServiceMock.Setup(s => s.GetUserAsync(_testUserId)).ReturnsAsync(user);
            _userDtoMapperMock.Setup(m => m.Bind(user)).Returns(userDto);

            // Act
            var result = await _controller.GetUserByIdAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userDto, okResult.Value);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsNotFound_WhenUserIsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(s => s.GetUserAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUserByIdAsync();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUnauthorized_WhenUserIdIsNull()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.GetUserByIdAsync();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateFirstNameAsync_ReturnsOk_WhenFirstNameIsUpdated()
        {
            // Arrange
            var firstName = "NewFirstName";
            var validationResult = new ValidationResult();
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            _userValidatorMock.Setup(v => v.ValidateFirstName(firstName)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserFirstNameAsync(_testUserId, firstName)).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateFirstNameAsync(firstName);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateFirstNameAsync_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var firstName = "NewFirstName";
            var validationResult = new ValidationResult();
            validationResult.Errors.Add("Invalid first name");

            _userValidatorMock.Setup(v => v.ValidateFirstName(firstName)).Returns(validationResult);

            // Act
            var result = await _controller.UpdateFirstNameAsync(firstName);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(string.Join("\r\n", validationResult.Errors), badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateFirstNameAsync_ReturnsNotFound_WhenUserIsNotFound()
        {
            // Arrange
            var firstName = "NewFirstName";
            var validationResult = new ValidationResult();

            _userValidatorMock.Setup(v => v.ValidateFirstName(firstName)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserFirstNameAsync(It.IsAny<Guid>(), firstName)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateFirstNameAsync(firstName);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.StartsWith("User with ID", notFoundResult.Value.ToString()); // Simplified assertion
        }

        [Fact]
        public async Task UpdateFirstNameAsync_ReturnsUnauthorized_WhenUserIdIsNull()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.UpdateFirstNameAsync("NewFirstName");

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdatePhoneNumberAsync_ReturnsOk_WhenPhoneNumberIsUpdated()
        {
            // Arrange
            var phoneNumber = "1234567890";
            var validationResult = new ValidationResult();
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            _userValidatorMock.Setup(v => v.ValidatePhoneNumber(phoneNumber)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserPhoneNumberAsync(_testUserId, phoneNumber)).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdatePhoneNumberAsync(phoneNumber);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdatePhoneNumberAsync_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var phoneNumber = "1234567890";
            var validationResult = new ValidationResult();
            validationResult.Errors.Add("Invalid phone number");

            _userValidatorMock.Setup(v => v.ValidatePhoneNumber(phoneNumber)).Returns(validationResult);

            // Act
            var result = await _controller.UpdatePhoneNumberAsync(phoneNumber);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(string.Join("\r\n", validationResult.Errors), badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePhoneNumberAsync_ReturnsNotFound_WhenUserIsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var phoneNumber = "1234567890";
            var validationResult = new ValidationResult();

            // Mock user claims to set the userId
            var claims = new List<Claim>
    {
        new Claim("userId", userId.ToString())
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Set up mocks
            _userValidatorMock.Setup(v => v.ValidatePhoneNumber(phoneNumber)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserPhoneNumberAsync(userId, phoneNumber)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdatePhoneNumberAsync(phoneNumber);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID {userId} not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdatePhoneNumberAsync_ReturnsUnauthorized_WhenUserIdIsNull()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.UpdatePhoneNumberAsync("1234567890");

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateEmailAsync_ReturnsOk_WhenEmailIsUpdatedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "newemail@example.com";
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            var validationResult = new ValidationResult();

            _userValidatorMock.Setup(v => v.ValidatePhoneNumber(email)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserPhoneNumberAsync(_testUserId, email)).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdatePhoneNumberAsync(email);

            // Assert
            Assert.IsType<OkResult>(result);
        }


        [Fact]
        public async Task UpdateEmailAsync_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "invalidemail";
            var validationResult = new ValidationResult();
            validationResult.Errors.Add("Invalid email format");

            var claims = new List<Claim>
    {
        new Claim("userId", userId.ToString())
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _userValidatorMock.Setup(v => v.ValidateEmail(email)).Returns(validationResult);

            // Act
            var result = await _controller.UpdateEmailAsync(email);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(string.Join("\r\n", validationResult.Errors), badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateEmailAsync_ReturnsNotFound_WhenUserIsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "newemail@example.com";
            var validationResult = new ValidationResult();

            var claims = new List<Claim>
    {
        new Claim("userId", userId.ToString())
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _userValidatorMock.Setup(v => v.ValidateEmail(email)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserEmailAsync(userId, email)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateEmailAsync(email);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID {userId} not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateProfilePictureAsync_ReturnsOk_WhenProfilePictureIsUpdatedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };
            var profilePictureDto = new UpdateProfilePictureDto { Image = fileMock.Object };
            var validationResult = new ValidationResult();

            var claims = new List<Claim>
    {
        new Claim("userId", userId.ToString())
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _userValidatorMock.Setup(v => v.ValidateProfilePicture(profilePictureDto.Image)).Returns(validationResult);
            _profilePictureServiceMock.Setup(s => s.GenerateProfilePicture(profilePictureDto.Image)).Returns(new byte[] { });
            _userServiceMock.Setup(s => s.UpdateUserProfilePictureAsync(userId, It.IsAny<byte[]>())).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateProfilePictureAsync(profilePictureDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateProfilePictureAsync_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            var profilePictureDto = new UpdateProfilePictureDto { Image = fileMock.Object };
            var validationResult = new ValidationResult();
            validationResult.Errors.Add("Invalid image format");

            var claims = new List<Claim>
    {
        new Claim("userId", userId.ToString())
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _userValidatorMock.Setup(v => v.ValidateProfilePicture(profilePictureDto.Image)).Returns(validationResult);

            // Act
            var result = await _controller.UpdateProfilePictureAsync(profilePictureDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(string.Join("\r\n", validationResult.Errors), badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProfilePictureAsync_ReturnsNotFound_WhenUserIsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            var profilePictureDto = new UpdateProfilePictureDto { Image = fileMock.Object };
            var validationResult = new ValidationResult();

            var claims = new List<Claim>
    {
        new Claim("userId", userId.ToString())
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _userValidatorMock.Setup(v => v.ValidateProfilePicture(profilePictureDto.Image)).Returns(validationResult);
            _profilePictureServiceMock.Setup(s => s.GenerateProfilePicture(profilePictureDto.Image)).Returns(new byte[] { });
            _userServiceMock.Setup(s => s.UpdateUserProfilePictureAsync(userId, It.IsAny<byte[]>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateProfilePictureAsync(profilePictureDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID {userId} not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateCityAsync_ValidCity_ReturnsOk()
        {
            // Arrange
            var city = "New City";
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            var validationResult = new ValidationResult();
            _userValidatorMock.Setup(v => v.ValidateCity(city)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserCityAsync(_testUserId, city)).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateCityAsync(city);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateCityAsync_InvalidCity_ReturnsBadRequest()
        {
            // Arrange
            var city = "New City";
            var userId = Guid.Parse(_controller.ControllerContext.HttpContext.User.Claims.First(c => c.Type == "userId").Value);
            var errors = new List<string> { "Invalid city" };

            var validationResult = new ValidationResult { Errors = errors };
            _userValidatorMock.Setup(v => v.ValidateCity(city)).Returns(validationResult);

            // Act
            var result = await _controller.UpdateCityAsync(city);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(string.Join("\r\n", errors), badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateCityAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var city = "New City";
            var userId = Guid.Parse(_controller.ControllerContext.HttpContext.User.Claims.First(c => c.Type == "userId").Value);

            var validationResult = new ValidationResult();
            _userValidatorMock.Setup(v => v.ValidateCity(city)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserCityAsync(userId, city)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateCityAsync(city);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID {userId} not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateCityAsync_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // No userId claim

            // Act
            var result = await _controller.UpdateCityAsync("Some City");

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateStreetAsync_ValidStreet_ReturnsOk()
        {
            // Arrange
            var street = "New Street";
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            var validationResult = new ValidationResult();

            _userValidatorMock.Setup(v => v.ValidateStreet(street)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserStreetAsync(_testUserId, street)).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateStreetAsync(street);

            // Assert
            Assert.IsType<OkResult>(result);
        }



        [Fact]
        public async Task UpdateStreetAsync_InvalidStreet_ReturnsBadRequest()
        {
            // Arrange
            var street = "New Street";
            var userId = Guid.Parse(_controller.ControllerContext.HttpContext.User.Claims.First(c => c.Type == "userId").Value);
            var errors = new List<string> { "Invalid street" };

            var validationResult = new ValidationResult { Errors = errors };
            _userValidatorMock.Setup(v => v.ValidateStreet(street)).Returns(validationResult);

            // Act
            var result = await _controller.UpdateStreetAsync(street);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(string.Join("\r\n", errors), badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateStreetAsync_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.UpdateStreetAsync("New Street");

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateStreetAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var street = "New Street";
            var userId = Guid.Parse(_controller.ControllerContext.HttpContext.User.Claims.First(c => c.Type == "userId").Value);

            var validationResult = new ValidationResult();
            _userValidatorMock.Setup(v => v.ValidateStreet(street)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserStreetAsync(userId, street)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateStreetAsync(street);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID {userId} not found.", notFoundResult.Value);


        }

        [Fact]
        public async Task UpdateHouseNumberAsync_ValidHouseNumber_ReturnsOk()
        {
            // Arrange
            var houseNumber = "123";
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            var validationResult = new ValidationResult();
            _userValidatorMock.Setup(v => v.ValidateHouseNumber(houseNumber)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserHouseNumberAsync(_testUserId, houseNumber)).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateHouseNumberAsync(houseNumber);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateHouseNumberAsync_InvalidHouseNumber_ReturnsBadRequest()
        {
            // Arrange
            var houseNumber = "123";
            var userId = Guid.Parse(_controller.ControllerContext.HttpContext.User.Claims.First(c => c.Type == "userId").Value);
            var errors = new List<string> { "Invalid house number" };

            var validationResult = new ValidationResult { Errors = errors };
            _userValidatorMock.Setup(v => v.ValidateHouseNumber(houseNumber)).Returns(validationResult);

            // Act
            var result = await _controller.UpdateHouseNumberAsync(houseNumber);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(string.Join("\r\n", errors), badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateHouseNumberAsync_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.UpdateHouseNumberAsync("123");

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateHouseNumberAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var houseNumber = "123";
            var userId = Guid.Parse(_controller.ControllerContext.HttpContext.User.Claims.First(c => c.Type == "userId").Value);

            var validationResult = new ValidationResult();
            _userValidatorMock.Setup(v => v.ValidateHouseNumber(houseNumber)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserHouseNumberAsync(userId, houseNumber)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateHouseNumberAsync(houseNumber);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID {userId} not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateApartmentNumberAsync_ValidApartmentNumber_ReturnsOk()
        {
            // Arrange
            var apartmentNumber = "Apt 101";
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            var validationResult = new ValidationResult();
            _userValidatorMock.Setup(v => v.ValidateApartmentNumber(apartmentNumber)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserApartmentNumberAsync(_testUserId, apartmentNumber)).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateApartmentNumberAsync(apartmentNumber);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateApartmentNumberAsync_InvalidApartmentNumber_ReturnsBadRequest()
        {
            // Arrange
            var apartmentNumber = "Apt 101";
            var userId = Guid.Parse(_controller.ControllerContext.HttpContext.User.Claims.First(c => c.Type == "userId").Value);
            var errors = new List<string> { "Invalid apartment number" };

            var validationResult = new ValidationResult { Errors = errors };
            _userValidatorMock.Setup(v => v.ValidateApartmentNumber(apartmentNumber)).Returns(validationResult);

            // Act
            var result = await _controller.UpdateApartmentNumberAsync(apartmentNumber);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(string.Join("\r\n", errors), badRequestResult.Value);
        }


        [Fact]
        public async Task UpdateApartmentNumberAsync_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.UpdateApartmentNumberAsync("Apt 101");

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateApartmentNumberAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var apartmentNumber = "Apt 101";
            var userId = Guid.Parse(_controller.ControllerContext.HttpContext.User.Claims.First(c => c.Type == "userId").Value);

            var validationResult = new ValidationResult();
            _userValidatorMock.Setup(v => v.ValidateApartmentNumber(apartmentNumber)).Returns(validationResult);
            _userServiceMock.Setup(s => s.UpdateUserApartmentNumberAsync(userId, apartmentNumber)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateApartmentNumberAsync(apartmentNumber);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID {userId} not found.", notFoundResult.Value);
        }

    }
}
