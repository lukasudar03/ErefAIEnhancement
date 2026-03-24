using ErefAIEnhancement.DTOs.ProfessorDtos;

namespace ErefAIEnhancement.Services.Interfaces
{
    public interface IProfessorService
    {
        Task<List<ProfessorResponseDto>> GetAllAsync();
        Task<ProfessorResponseDto> GetByIdAsync(Guid id);
        Task<ProfessorResponseDto> CreateAsync(CreateProfessorDto dto);
        Task<ProfessorResponseDto> UpdateAsync(Guid id, UpdateProfessorDto dto);
        Task DeleteAsync(Guid id);
    }
}