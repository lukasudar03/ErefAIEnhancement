using ErefAIEnhancement.Enums;

namespace ErefAIEnhancement.DTOs.StudentDto
{
    public class CreateStudentDto
    {
        public Guid UserId { get; set; }
        public string IndexNumber { get; set; } = string.Empty;
        public YearOfStudy YearOfStudy { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Department Department { get; set; }
    }
}
