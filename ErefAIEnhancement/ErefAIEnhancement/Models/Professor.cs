namespace ErefAIEnhancement.Models
{
    public class Professor
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }

        // kasnije:
        // public ICollection<Subject> Subjects { get; set; }
    }
}
