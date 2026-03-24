namespace ErefAIEnhancement.DTOs.ProfessorDtos
{
    public class ProfessorResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
