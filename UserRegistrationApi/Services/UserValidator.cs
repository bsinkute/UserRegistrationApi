using System.Text.RegularExpressions;
using UserRegistrationApi.Models.Dto;

namespace UserRegistrationApi.Services
{
    public interface IUserValidator
    {
        ValidationResult ValidateCreateUserDto(CreateUserDto createUserDto);
        ValidationResult ValidateFirstName(string firstName);
        ValidationResult ValidateSurname(string surname);
        ValidationResult ValidatePersonalIdentificationNumber(string personalIdentificationNumber);
        ValidationResult ValidatePhoneNumber(string phoneNumber);
        ValidationResult ValidateEmail(string email);
        ValidationResult ValidateProfilePicture(IFormFile picture);
        ValidationResult ValidateCity(string city);
        ValidationResult ValidateStreet(string street);
        ValidationResult ValidateHouseNumber(string houseNumber);
        ValidationResult ValidateApartmentNumber(string apartmentNumber);
    }

    public class UserValidator : IUserValidator
    {
        public ValidationResult ValidateCreateUserDto(CreateUserDto createUserDto)
        {
            var validationResult = new ValidationResult();

            validationResult.Merge(ValidateFirstName(createUserDto.FirstName));
            validationResult.Merge(ValidateSurname(createUserDto.Surname));
            validationResult.Merge(ValidatePersonalIdentificationNumber(createUserDto.PersonalIdentificationNumber));
            validationResult.Merge(ValidatePhoneNumber(createUserDto.PhoneNumber));
            validationResult.Merge(ValidateEmail(createUserDto.Email));
            validationResult.Merge(ValidateProfilePicture(createUserDto.Image));
            validationResult.Merge(ValidateCity(createUserDto.City));
            validationResult.Merge(ValidateStreet(createUserDto.Street));
            validationResult.Merge(ValidateHouseNumber(createUserDto.HouseNumber));
            validationResult.Merge(ValidateApartmentNumber(createUserDto.ApartmentNumber));

            return validationResult;
        }

        public ValidationResult ValidateFirstName(string firstName)
        {
            var validationResult = new ValidationResult();
            const int firstNameLength = 150;
            if (!ValidateStringValue(firstName, firstNameLength))
            {
                validationResult.Errors.Add($"FirstName cannot be null, empty or longer than {firstNameLength}");
            }
            return validationResult;
        }

        public ValidationResult ValidateSurname(string surname)
        {
            var validationResult = new ValidationResult();
            const int surnameLength = 150;
            if (!ValidateStringValue(surname, surnameLength))
            {
                validationResult.Errors.Add($"Surname cannot be null, empty or longer than {surnameLength}");
            }
            return validationResult;
        }

        public ValidationResult ValidatePersonalIdentificationNumber(string personalIdentificationNumber)
        {
            var validationResult = new ValidationResult();
            const int personalIdentificationNumberLength = 150;
            if (!ValidateStringValue(personalIdentificationNumber, personalIdentificationNumberLength))
            {
                validationResult.Errors.Add($"PIN cannot be null, empty or longer than {personalIdentificationNumberLength}");
            }

            if (!personalIdentificationNumber.All(char.IsDigit))
            {
                validationResult.Errors.Add($"PIN must be composed out of digits");
            }
            return validationResult;
        }

        public ValidationResult ValidatePhoneNumber(string phoneNumber)
        {
            var validationResult = new ValidationResult();
            const int phoneNumberLength = 20;
            if (!ValidateStringValue(phoneNumber, phoneNumberLength))
            {
                validationResult.Errors.Add($"Phone Number cannot be null, empty or longer than {phoneNumberLength}");
            }

            string pattern = @"^\+?\d{1,3}\d{1,14}$";
            if (!Regex.IsMatch(phoneNumber, pattern))
            {
                validationResult.Errors.Add($"Phone number must have an country code and cannot contain a special character");
            }
            return validationResult;
        }

        public ValidationResult ValidateEmail(string email)
        {
            var validationResult = new ValidationResult();
            const int emailLength = 100;
            if (!ValidateStringValue(email, emailLength))
            {
                validationResult.Errors.Add($"Email cannot be null, empty or longer than {emailLength}");
            }

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, pattern))
            {
                validationResult.Errors.Add($"Email format is incorrect");
            }
            return validationResult;
        }

        public ValidationResult ValidateProfilePicture(IFormFile picture)
        {
            var validationResult = new ValidationResult();
            if (picture is null)
            {
                validationResult.Errors.Add($"Profile picture cannot be null");
            }
            return validationResult;
        }

        public ValidationResult ValidateCity(string city)
        {
            var validationResult = new ValidationResult();
            const int cityLength = 100;
            if (!ValidateStringValue(city, cityLength))
            {
                validationResult.Errors.Add($"City cannot be null, empty or longer than {cityLength}");
            }
            return validationResult; 
        }

        public ValidationResult ValidateStreet(string street)
        {
            var validationResult = new ValidationResult();
            const int streetLength = 100;
            if (!ValidateStringValue(street, streetLength))
            {
                validationResult.Errors.Add($"Street cannot be null, empty or longer than {streetLength}");
            }
            return validationResult;
        }

        public ValidationResult ValidateHouseNumber(string houseNumber)
        {
            var validationResult = new ValidationResult();
            const int houseNumberLength = 100;
            if (!ValidateStringValue(houseNumber, houseNumberLength))
            {
                validationResult.Errors.Add($"House Number cannot be null, empty or longer than {houseNumberLength}");
            }
            return validationResult;
        }

        public ValidationResult ValidateApartmentNumber(string apartmentNumber)
        {
            var validationResult = new ValidationResult();
            const int apartmentNumberLength = 100;
            if (!ValidateStringValue(apartmentNumber, apartmentNumberLength))
            {
                validationResult.Errors.Add($"Apartment Number cannot be null, empty or longer than {apartmentNumberLength}");
            }
            return validationResult;
        }

        private static bool ValidateStringValue(string value, int stringLength)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > stringLength)
            {
                return false;
            }
            return true;
        }
    }

    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; set; } = new List<string>();

        public void Merge(ValidationResult other)
        {
            Errors.AddRange(other.Errors);
        }
    }
}
