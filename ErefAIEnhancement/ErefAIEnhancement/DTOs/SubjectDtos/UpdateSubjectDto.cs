using ErefAIEnhancement.Enums;

namespace ErefAIEnhancement.DTOs.SubjectDtos
{
    public class UpdateSubjectDto
    {
        public string Name { get; set; } = string.Empty;
        public YearOfStudy YearOfStudy { get; set; }
        public Department Department { get; set; }
    }
}