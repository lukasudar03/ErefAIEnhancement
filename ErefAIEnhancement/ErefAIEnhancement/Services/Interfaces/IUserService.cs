using ErefAIEnhancement.DTOs.UserDtos;

namespace ErefAIEnhancement.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto> GetByIdAsync(Guid id);
        Task<UserResponseDto> CreateAsync(CreateUserDto dto);
        Task<UserResponseDto> UpdateAsync(Guid id, UpdateUserDto dto);
        Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
        Task DeleteAsync(Guid id);
    }
}