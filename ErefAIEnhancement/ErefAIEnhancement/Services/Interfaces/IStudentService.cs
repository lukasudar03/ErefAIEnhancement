using ErefAIEnhancement.DTOs.StudentDto;

namespace ErefAIEnhancement.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentResponseDto>> GetAllAsync();
        Task<StudentResponseDto?> GetByIdAsync(Guid id);
        Task<StudentResponseDto> CreateAsync(CreateStudentDto dto);
        Task<StudentResponseDto?> UpdateAsync(Guid id, UpdateStudentDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}