using ErefAIEnhancement.DTOs.AuthDtos;

namespace ErefAIEnhancement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}