using Microsoft.AspNetCore.Http;
using Moq;
using UserRegistrationApi.Models.Dto;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Tests.Services
{
    namespace UserRegistrationApi.Tests.Services
    {
        public class UserValidatorTests
        {
            private readonly IUserValidator _userValidator;

            public UserValidatorTests()
            {
                _userValidator = new UserValidator();
            }

            [Fact]
            public void ValidateCreateUserDto_WithValidData_ShouldReturnValidResult()
            {
                // Arrange
                var createUserDto = new CreateUserDto
                {
                    Username = "ValidUsername",
                    Password = "Password1!",
                    FirstName = "John",
                    Surname = "Doe",
                    PersonalIdentificationNumber = "12345678901",
                    PhoneNumber = "+1234567890",
                    Email = "email@example.com",
                    Image = Mock.Of<IFormFile>(),
                    City = "CityName",
                    Street = "StreetName",
                    HouseNumber = "123",
                    ApartmentNumber = "456"
                };

                // Act
                var result = _userValidator.ValidateCreateUserDto(createUserDto);

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidateCreateUserDto_WithInvalidData_ShouldReturnInvalidResult()
            {
                // Arrange
                var createUserDto = new CreateUserDto
                {
                    Username = "ValidUsername",
                    Password = "Password1",
                    FirstName = null,
                    Surname = "   ",
                    PersonalIdentificationNumber = "1234s5678901",
                    PhoneNumber = "i+1234567890",
                    Email = "emailexample.com",
                    Image = null,
                    City = "",
                    Street = null,
                    HouseNumber = string.Empty,
                    ApartmentNumber = "                    "
                };

                // Act
                var result = _userValidator.ValidateCreateUserDto(createUserDto);

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("Password must have"));
                Assert.Contains(result.Errors, e => e.Contains("FirstName cannot be null"));
                Assert.Contains(result.Errors, e => e.Contains("Surname cannot be null"));
                Assert.Contains(result.Errors, e => e.Contains("PIN must be composed"));
                Assert.Contains(result.Errors, e => e.Contains("Phone number must have"));
                Assert.Contains(result.Errors, e => e.Contains("Email format is incorrect"));
                Assert.Contains(result.Errors, e => e.Contains("Profile picture cannot be null"));
                Assert.Contains(result.Errors, e => e.Contains("City cannot be null"));
                Assert.Contains(result.Errors, e => e.Contains("Street cannot be null"));
                Assert.Contains(result.Errors, e => e.Contains("House Number cannot be null"));
                Assert.Contains(result.Errors, e => e.Contains("Apartment Number cannot be null"));
            }

            [Fact]
            public void ValidateFirstName_WithValidFirstName_ShouldReturnValidResult()
            {
                // Act
                var result = _userValidator.ValidateFirstName("John");

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidateFirstName_WithInvalidFirstName_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidateFirstName("");

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("FirstName cannot be null"));
            }

            [Fact]
            public void ValidateSurname_WithValidSurname_ShouldReturnValidResult()
            {
                // Act
                var result = _userValidator.ValidateSurname("Doe");

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidateSurname_WithInvalidSurname_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidateSurname("");

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("Surname cannot be null"));
            }

            [Fact]
            public void ValidatePersonalIdentificationNumber_WithValidPIN_ShouldReturnValidResult()
            {
                // Act
                var result = _userValidator.ValidatePersonalIdentificationNumber("12345678901");

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidatePersonalIdentificationNumber_WithInvalidPIN_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidatePersonalIdentificationNumber("abc");

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("PIN must be composed"));
            }

            [Fact]
            public void ValidatePhoneNumber_WithValidPhoneNumber_ShouldReturnValidResult()
            {
                // Act
                var result = _userValidator.ValidatePhoneNumber("+1234567890");

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidatePhoneNumber_WithInvalidPhoneNumber_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidatePhoneNumber("a12345");

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("Phone number must have"));
            }

            [Fact]
            public void ValidateEmail_WithValidEmail_ShouldReturnValidResult()
            {
                // Act
                var result = _userValidator.ValidateEmail("email@example.com");

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidateEmail_WithInvalidEmail_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidateEmail("email");

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("Email format is incorrect"));
            }

            [Fact]
            public void ValidateProfilePicture_WithValidProfilePicture_ShouldReturnValidResult()
            {
                // Arrange
                var formFile = Mock.Of<IFormFile>();

                // Act
                var result = _userValidator.ValidateProfilePicture(formFile);

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidateProfilePicture_WithNullProfilePicture_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidateProfilePicture(null);

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("Profile picture cannot be null"));
            }

            [Fact]
            public void ValidateCity_WithValidCity_ShouldReturnValidResult()
            {
                // Act
                var result = _userValidator.ValidateCity("CityName");

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidateCity_WithInvalidCity_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidateCity("");

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("City cannot be null"));
            }

            [Fact]
            public void ValidateStreet_WithValidStreet_ShouldReturnValidResult()
            {
                // Act
                var result = _userValidator.ValidateStreet("StreetName");

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidateStreet_WithInvalidStreet_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidateStreet("");

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("Street cannot be null"));
            }

            [Fact]
            public void ValidateHouseNumber_WithValidHouseNumber_ShouldReturnValidResult()
            {
                // Act
                var result = _userValidator.ValidateHouseNumber("123");

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidateHouseNumber_WithInvalidHouseNumber_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidateHouseNumber("");

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("House Number cannot be null"));
            }

            [Fact]
            public void ValidateApartmentNumber_WithValidApartmentNumber_ShouldReturnValidResult()
            {
                // Act
                var result = _userValidator.ValidateApartmentNumber("456");

                // Assert
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ValidateApartmentNumber_WithInvalidApartmentNumber_ShouldReturnInvalidResult()
            {
                // Act
                var result = _userValidator.ValidateApartmentNumber("");

                // Assert
                Assert.False(result.IsValid);
                Assert.Contains(result.Errors, e => e.Contains("Apartment Number cannot be null"));
            }
        }
    }
}
