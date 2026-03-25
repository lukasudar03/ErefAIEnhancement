using ErefAIEnhancement.Enums;

namespace ErefAIEnhancement.DTOs.StudentSubjectDtos
{
    public class StudentSubjectSelectionItemDto
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public YearOfStudy YearOfStudy { get; set; }
        public Department Department { get; set; }
        public bool Required { get; set; }
        public bool Selected { get; set; }
    }
}