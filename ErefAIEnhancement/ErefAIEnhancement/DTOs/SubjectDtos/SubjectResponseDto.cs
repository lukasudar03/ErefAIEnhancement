using ErefAIEnhancement.Enums;

namespace ErefAIEnhancement.DTOs.SubjectDtos
{
    public class SubjectResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ProfessorId { get; set; }
        public string? ProfessorName { get; set; } = string.Empty;
        public string? ProfessorEmail { get; set; } = string.Empty;
        public YearOfStudy YearOfStudy { get; set; }
        public Department Department { get; set; }
        public bool Required { get; set; }
    }
}