using ErefAIEnhancement.DTOs.StudentDto;
using ErefAIEnhancement.DTOs.StudentSubjectDtos;

namespace ErefAIEnhancement.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentResponseDto>> GetAllAsync();
        Task<StudentResponseDto> GetByIdAsync(Guid id);
        Task<StudentResponseDto> CreateAsync(CreateStudentDto dto);
        Task<StudentResponseDto> UpdateAsync(Guid id, UpdateStudentDto dto);
        Task DeleteAsync(Guid id);

        Task<List<StudentSubjectSelectionItemDto>> GetSubjectsAsync(Guid studentId);
        Task<List<StudentSubjectSelectionItemDto>> UpdateSubjectsAsync(Guid studentId, UpdateStudentSubjectsDto dto);
    }
}