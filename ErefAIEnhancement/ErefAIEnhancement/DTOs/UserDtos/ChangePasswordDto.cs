namespace ErefAIEnhancement.DTOs.UserDtos
{
    public class ChangePasswordDto
    {
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
