namespace ErefAIEnhancement.DTOs.AuthDtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
