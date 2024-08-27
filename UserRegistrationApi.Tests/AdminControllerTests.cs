using Microsoft.AspNetCore.Mvc;
using Moq;
using UserRegistrationApi.Controllers;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Tests
{
    public class AdminControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IUserDtoMapper> _userDtoMapperMock;
        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _userDtoMapperMock = new Mock<IUserDtoMapper>();
            _controller = new AdminController(_userServiceMock.Object, _userDtoMapperMock.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsOkResult_WithUserDtos()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "JohnDoe",
                    Password = new byte[] { },
                    Salt = new byte[] { },
                    Role = "Admin"
                }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto
                {
                    Username = "JohnDoe",
                    Role = "Admin",
                }
            };

            _userServiceMock.Setup(s => s.GetUsersAsync()).ReturnsAsync(users);
            _userDtoMapperMock.Setup(m => m.Bind(It.IsAny<User>())).Returns(userDtos.First());

            // Act
            var result = await _controller.GetUsersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var userList = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value).ToList();
            Assert.Single(userList);
            Assert.Equal("JohnDoe", userList[0].Username);
            Assert.Equal("Admin", userList[0].Role);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsNotFound_WhenNoUsers()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUsersAsync()).ReturnsAsync((List<User>)null);

            // Act
            var result = await _controller.GetUsersAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No users found.", notFoundResult.Value);
        }
    }
}