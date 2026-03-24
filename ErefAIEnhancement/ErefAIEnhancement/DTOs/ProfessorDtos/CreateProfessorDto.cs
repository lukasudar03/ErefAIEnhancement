namespace ErefAIEnhancement.DTOs.ProfessorDtos
{
    public class CreateProfessorDto
    {
        public Guid UserId { get; set; }
        public List<Guid> SubjectIds { get; set; } = new();
    }
}