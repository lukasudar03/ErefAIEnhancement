namespace ErefAIEnhancement.Models
{
    public class Role
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; } = string.Empty;

        // navigacija (1 role → više usera)
        public List<User> Users { get; set; } = new List<User>();
    }
}