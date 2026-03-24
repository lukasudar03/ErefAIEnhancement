using ErefAIEnhancement.Exceptions;
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
        private readonly IServiceProvider _serviceProvider;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToResponseDto).ToList();
        }

        public async Task<UserResponseDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found");

            return MapToResponseDto(user);
        }

        public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
        {
            await ValidateAsync(dto);

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new BadRequestException("User with this email already exists.");

            var roleExists = await _userRepository.RoleExistsAsync(dto.RoleId);
            if (!roleExists)
                throw new BadRequestException("Selected role does not exist.");

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

        public async Task<UserResponseDto> UpdateAsync(Guid id, UpdateUserDto dto)
        {
            await ValidateAsync(dto);

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found");

            var existingUserWithEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != id)
                throw new BadRequestException("Another user with this email already exists.");

            var roleExists = await _userRepository.RoleExistsAsync(dto.RoleId);
            if (!roleExists)
                throw new BadRequestException("Selected role does not exist.");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.RoleId = dto.RoleId;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            var updatedUser = await _userRepository.GetByIdAsync(id);
            return MapToResponseDto(updatedUser!);
        }

        public async Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
        {
            await ValidateAsync(dto);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found");

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
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

        private async Task ValidateAsync<T>(T dto)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator == null)
                return;

            var result = await validator.ValidateAsync(dto);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }
    }
}