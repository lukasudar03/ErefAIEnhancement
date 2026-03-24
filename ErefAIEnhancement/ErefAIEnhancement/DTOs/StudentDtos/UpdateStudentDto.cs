using ErefAIEnhancement.Enums;

namespace ErefAIEnhancement.DTOs.StudentDto
{
    public class UpdateStudentDto
    {
        public string IndexNumber { get; set; } = string.Empty;
        public YearOfStudy YearOfStudy { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Department Department { get; set; }
    }
}
