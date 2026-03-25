using ErefAIEnhancement.Enums;

namespace ErefAIEnhancement.DTOs.SubjectDtos
{
    public class CreateSubjectDto
    {
        public string Name { get; set; } = string.Empty;
        public YearOfStudy YearOfStudy { get; set; }
        public Department Department { get; set; }
        public bool Required { get; set; }
    }
}
