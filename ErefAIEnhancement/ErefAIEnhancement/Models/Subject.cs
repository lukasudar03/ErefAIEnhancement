using ErefAIEnhancement.Enums;

namespace ErefAIEnhancement.Models
{
    public class Subject
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid? ProfessorId { get; set; }
        public Professor? Professor { get; set; } = null!;

        public YearOfStudy YearOfStudy { get; set; }
        public Department Department { get; set; }

        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}