using ErefAIEnhancement.DTOs.SubjectDtos;

namespace ErefAIEnhancement.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<List<SubjectResponseDto>> GetAllAsync();
        Task<SubjectResponseDto> GetByIdAsync(Guid id);
        Task<SubjectResponseDto> CreateAsync(CreateSubjectDto dto);
        Task<SubjectResponseDto> UpdateAsync(Guid id, UpdateSubjectDto dto);
        Task DeleteAsync(Guid id);
    }
}