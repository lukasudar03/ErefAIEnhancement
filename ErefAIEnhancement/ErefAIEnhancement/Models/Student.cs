using ErefAIEnhancement.Enums;

namespace ErefAIEnhancement.Models
{
    public class Student
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public string IndexNumber { get; set; } = string.Empty;

        public YearOfStudy YearOfStudy { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Department Department { get; set; }

        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}