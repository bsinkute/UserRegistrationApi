using Moq;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Repositories;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ICreateUserMapper> _createUserMapperMock;
        private readonly Mock<IUserCredentialService> _userCredentialServiceMock;
        private readonly AuthenticationService _authenticationService;
        private readonly Guid _testUserId = Guid.NewGuid();

        public AuthenticationServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _createUserMapperMock = new Mock<ICreateUserMapper>();
            _userCredentialServiceMock = new Mock<IUserCredentialService>();
            _authenticationService = new AuthenticationService(
                _userRepositoryMock.Object,
                _createUserMapperMock.Object,
                _userCredentialServiceMock.Object
            );
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(username))
                .ReturnsAsync(user);

            _userCredentialServiceMock
                .Setup(s => s.VerifyPasswordHash(password, user.Password, user.Salt))
                .Returns(true);

            // Act
            var result = await _authenticationService.LoginAsync(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var username = "nonexistentuser";
            var password = "password123";

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(username))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _authenticationService.LoginAsync(username, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var user = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(username))
                .ReturnsAsync(user);

            _userCredentialServiceMock
                .Setup(s => s.VerifyPasswordHash(password, user.Password, user.Salt))
                .Returns(false);

            // Act
            var result = await _authenticationService.LoginAsync(username, password);

            // Assert
            Assert.Null(result);
        }
    }
}