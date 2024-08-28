using Moq;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Repositories;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserService _userService;
        private readonly Guid _testUserId = Guid.NewGuid();

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var expectedUser = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserAsync(_testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnUser_WhenUsernameExists()
        {
            // Arrange
            var expectedUser = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };

            _userRepositoryMock.Setup(r => r.GetUserAsync(expectedUser.Username))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserAsync(expectedUser.Username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnListOfUsers()
        {
            // Arrange
            var user1 = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };
            var user2 = new User { UserId = _testUserId, Username = "testuser", Password = [], Salt = [], Role = "User" };
            var users = new List<User> { user1, user2 };
            

            _userRepositoryMock.Setup(r => r.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, ((List<User>)result).Count);
        }

        [Fact]
        public async Task UpdateUserFirstNameAsync_ShouldUpdateUserFirstName()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation 
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "OldFirstName",
                    Surname = "Surname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890"
                }
            };

            var newFirstName = "NewFirstName";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserPersonalInformation(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserFirstNameAsync(_testUserId, newFirstName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newFirstName, result.PersonalInformation.FirstName);
        }

        [Fact]
        public async Task UpdateUserCityAsync_ShouldUpdateUserCity()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "OldFirstName",
                    Surname = "Surname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890",
                    Address = new Address 
                    { 
                        AddressId = Guid.NewGuid(),
                        City = "OldCity",
                        Street = "Street",
                        HouseNumber = "15",
                        ApartmentNumber = "12",

                    }
                }
            };

            var newCity = "NewCity";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserAddress(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserCityAsync(_testUserId, newCity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newCity, result.PersonalInformation.Address.City);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserIsDeleted()
        {
            // Arrange

            _userRepositoryMock.Setup(r => r.DeleteUserAsync(_testUserId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.DeleteUserAsync(_testUserId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUserFirstNameAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var newFirstName = "NewFirstName";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.UpdateUserFirstNameAsync(_testUserId, newFirstName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserSurnameAsync_ShouldUpdateUserSurname()
        {
            // Arrange
               var existingUser = new User
               {
                   UserId = _testUserId,
                   Username = "testuser",
                   Password = [],
                   Salt = [],
                   Role = "User",
                   PersonalInformation = new PersonalInformation
                   {
                       PersonalInformationId = Guid.NewGuid(),
                       FirstName = "FirstName",
                       Surname = "OldSurname",
                       PersonalIdentificationNumber = "176486",
                       ProfilePicture = [],
                       Email = "mail",
                       PhoneNumber = "1234567890"
                   }
               };

            var newSurname = "NewSurname";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserPersonalInformation(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserSurnameAsync(_testUserId, newSurname);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newSurname, result.PersonalInformation.Surname);
        }

        [Fact]
        public async Task UpdateUserPersonalIdentificationNumberAsync_ShouldUpdatePersonalIdentificationNumber()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "FirstName",
                    Surname = "Surname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890"
                }
            };

            var newPIN = "NewPIN";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserPersonalInformation(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserPersonalIdentificationNumberAsync(_testUserId, newPIN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newPIN, result.PersonalInformation.PersonalIdentificationNumber);
        }
        [Fact]
        public async Task UpdateUserPhoneNumberAsync_ShouldUpdatePhoneNumber()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "FirstName",
                    Surname = "OldSurname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890"
                }
            };

            var newPhoneNumber = "NewPhoneNumber";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserPersonalInformation(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserPhoneNumberAsync(_testUserId, newPhoneNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newPhoneNumber, result.PersonalInformation.PhoneNumber);
        }

        [Fact]
        public async Task UpdateUserEmailAsync_ShouldUpdateEmail()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "FirstName",
                    Surname = "OldSurname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890"
                }
            };

            var newEmail = "newemail@example.com";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserPersonalInformation(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserEmailAsync(_testUserId, newEmail);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newEmail, result.PersonalInformation.Email);
        }

        [Fact]
        public async Task UpdateUserProfilePictureAsync_ShouldUpdateProfilePicture()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "FirstName",
                    Surname = "OldSurname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890"
                }
            };

            var newProfilePicture = new byte[] { 0x02, 0x03 };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserPersonalInformation(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserProfilePictureAsync(_testUserId, newProfilePicture);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newProfilePicture, result.PersonalInformation.ProfilePicture);
        }

        [Fact]
        public async Task UpdateUserCityAsync_ShouldUpdateCity()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "OldFirstName",
                    Surname = "Surname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890",
                    Address = new Address
                    {
                        AddressId = Guid.NewGuid(),
                        City = "OldCity",
                        Street = "Street",
                        HouseNumber = "15",
                        ApartmentNumber = "12",

                    }
                }
            };

            var newCity = "NewCity";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserAddress(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserStreetAsync(_testUserId, newCity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newCity, result.PersonalInformation.Address.Street);
        }

        [Fact]
        public async Task UpdateUserStreetAsync_ShouldUpdateStreet()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "OldFirstName",
                    Surname = "Surname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890",
                    Address = new Address
                    {
                        AddressId = Guid.NewGuid(),
                        City = "OldCity",
                        Street = "Street",
                        HouseNumber = "15",
                        ApartmentNumber = "12",

                    }
                }
            };

            var newStreet = "NewStreet";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserAddress(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserStreetAsync(_testUserId, newStreet);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newStreet, result.PersonalInformation.Address.Street);
        }

        [Fact]
        public async Task UpdateUserHouseNumberAsync_ShouldUpdateHouseNumber()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "OldFirstName",
                    Surname = "Surname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890",
                    Address = new Address
                    {
                        AddressId = Guid.NewGuid(),
                        City = "OldCity",
                        Street = "Street",
                        HouseNumber = "15",
                        ApartmentNumber = "12",

                    }
                }
            };

            var newHouseNumber = "NewHouseNumber";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserAddress(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserHouseNumberAsync(_testUserId, newHouseNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newHouseNumber, result.PersonalInformation.Address.HouseNumber);
        }

        [Fact]
        public async Task UpdateUserApartmentNumberAsync_ShouldUpdateApartmentNumber()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = _testUserId,
                Username = "testuser",
                Password = [],
                Salt = [],
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = "OldFirstName",
                    Surname = "Surname",
                    PersonalIdentificationNumber = "176486",
                    ProfilePicture = [],
                    Email = "mail",
                    PhoneNumber = "1234567890",
                    Address = new Address
                    {
                        AddressId = Guid.NewGuid(),
                        City = "OldCity",
                        Street = "Street",
                        HouseNumber = "15",
                        ApartmentNumber = "12",

                    }
                }
            };

            var newApartmentNumber = "NewApartmentNumber";

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(_testUserId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUserAddress(existingUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserApartmentNumberAsync(_testUserId, newApartmentNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newApartmentNumber, result.PersonalInformation.Address.ApartmentNumber);
        }
    }
}