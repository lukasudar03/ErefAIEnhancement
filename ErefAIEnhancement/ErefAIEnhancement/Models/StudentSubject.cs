namespace ErefAIEnhancement.Models
{
    public class StudentSubject
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public bool Selected { get; set; }
    }
}