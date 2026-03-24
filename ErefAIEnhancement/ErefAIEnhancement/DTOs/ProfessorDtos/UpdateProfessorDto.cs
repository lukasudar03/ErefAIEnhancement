namespace ErefAIEnhancement.DTOs.ProfessorDtos
{
    public class UpdateProfessorDto
    {
        public Guid UserId { get; set; }
        public List<Guid> SubjectIds { get; set; } = new();
    }
}