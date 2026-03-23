using ErefAIEnhancement.Enums;

namespace ErefAIEnhancement.DTOs.StudentDto
{
    public class StudentResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string IndexNumber { get; set; } = string.Empty;
        public YearOfStudy YearOfStudy { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Department Department { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
