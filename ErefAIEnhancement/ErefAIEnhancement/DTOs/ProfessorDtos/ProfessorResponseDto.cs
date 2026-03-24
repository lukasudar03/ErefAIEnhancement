using ErefAIEnhancement.DTOs.SubjectDtos;

namespace ErefAIEnhancement.DTOs.ProfessorDtos
{
    public class ProfessorResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<ProfessorSubjectDto> Subjects { get; set; } = new();
    }
}