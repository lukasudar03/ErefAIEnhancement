using ErefAIEnhancement.DTOs;
using ErefAIEnhancement.DTOs.UserDtos;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using ErefAIEnhancement.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ErefAIEnhancement.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IValidator<CreateUserDto> createUserValidator,
            IValidator<UpdateUserDto> updateUserValidator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
        }

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToResponseDto).ToList();
        }

        public async Task<UserResponseDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            return MapToResponseDto(user);
        }

        public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
        {
            var validationResult = await _createUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            var roleExists = await _userRepository.RoleExistsAsync(dto.RoleId);
            if (!roleExists)
                throw new Exception("Selected role does not exist.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                RoleId = dto.RoleId
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var createdUser = await _userRepository.GetByIdAsync(user.Id);
            return MapToResponseDto(createdUser!);
        }

        public async Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto)
        {
            var validationResult = await _updateUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            var existingUserWithEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != id)
                throw new Exception("Another user with this email already exists.");

            var roleExists = await _userRepository.RoleExistsAsync(dto.RoleId);
            if (!roleExists)
                throw new Exception("Selected role does not exist.");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.RoleId = dto.RoleId;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            }

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            var updatedUser = await _userRepository.GetByIdAsync(id);
            return MapToResponseDto(updatedUser!);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        private static UserResponseDto MapToResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role?.RoleName ?? string.Empty
            };
        }
    }
}