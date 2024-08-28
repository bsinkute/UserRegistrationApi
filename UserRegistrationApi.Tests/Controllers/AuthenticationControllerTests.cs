using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserRegistrationApi.Controllers;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IUserValidator> _userValidatorMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _jwtServiceMock = new Mock<IJwtService>();
            _userValidatorMock = new Mock<IUserValidator>();
            _userServiceMock = new Mock<IUserService>();
            _controller = new AuthenticationController(_authenticationServiceMock.Object, _jwtServiceMock.Object, _userValidatorMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsSuccessfullyRegistered()
        {
            // Arrange
            var userDto = new CreateUserDto
            {
                Username = "newuser",
                Password = "password123",
                FirstName = "John",
                Surname = "Doe",
                PersonalIdentificationNumber = "1234567890",
                PhoneNumber = "123-456-7890",
                Email = "john.doe@example.com",
                Image = CreateMockFormFile("image.jpg", "image/jpeg"),
                City = "City",
                Street = "Street",
                HouseNumber = "123",
                ApartmentNumber = "45"
            };

            var validationResult = new ValidationResult();

            _userValidatorMock.Setup(v => v.ValidateCreateUserDto(userDto)).Returns(validationResult);
            _userServiceMock.Setup(s => s.GetUserAsync(userDto.Username)).ReturnsAsync((User)null);
            _authenticationServiceMock.Setup(a => a.RegisterAsync(userDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserAlreadyExists()
        {
            // Arrange
            var userDto = new CreateUserDto
            {
                Username = "existinguser",
                Password = "password123",
                FirstName = "John",
                Surname = "Doe",
                PersonalIdentificationNumber = "1234567890",
                PhoneNumber = "123-456-7890",
                Email = "john.doe@example.com",
                Image = CreateMockFormFile("image.jpg", "image/jpeg"),
                City = "City",
                Street = "Street",
                HouseNumber = "123",
                ApartmentNumber = "45"
            };

            var validationResult = new ValidationResult();
            var existingUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = "existinguser",
                Password = new byte[] { },
                Salt = new byte[] { },
                Role = "Admin"
            };

            _userValidatorMock.Setup(v => v.ValidateCreateUserDto(userDto)).Returns(validationResult);
            _userServiceMock.Setup(s => s.GetUserAsync(userDto.Username)).ReturnsAsync(existingUser);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("existinguser already exists", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var userDto = new CreateUserDto
            {
                Username = "invaliduser",
                Password = "password123",
                FirstName = "John",
                Surname = "Doe",
                PersonalIdentificationNumber = "1234567890",
                PhoneNumber = "123-456-7890",
                Email = "john.doe@example.com",
                Image = CreateMockFormFile("image.jpg", "image/jpeg"),
                City = "City",
                Street = "Street",
                HouseNumber = "123",
                ApartmentNumber = "45"
            };

            var validationResult = new ValidationResult
            {
                Errors = new List<string> { "Username is required", "Email is invalid" }
            };

            _userValidatorMock.Setup(v => v.ValidateCreateUserDto(userDto)).Returns(validationResult);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username is required\r\nEmail is invalid", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithToken_WhenCredentialsAreCorrect()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = username,
                Password = new byte[] { 0x20 },
                Salt = new byte[] { 0x20 },
                Role = "User"
            };

            var token = "valid-jwt-token";

            _authenticationServiceMock.Setup(a => a.LoginAsync(username, password))
                .ReturnsAsync(user);

            _jwtServiceMock.Setup(j => j.GenerateToken(username, user.UserId, user.Role))
                .Returns(token);

            // Act
            var result = await _controller.Login(username, password);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(token, okResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenCredentialsAreIncorrect()
        {
            // Arrange
            var username = "testuser";
            var password = "wrongpassword";

            _authenticationServiceMock.Setup(a => a.LoginAsync(username, password))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(username, password);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Incorrect username or password", badRequestResult.Value);
        }

        private IFormFile CreateMockFormFile(string fileName, string contentType)
        {
            var content = "Fake file content";
            var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            return new FormFile(fileStream, 0, fileStream.Length, "id_from_form", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}