namespace Editlio.Web.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty; // Varsayılan değer
        public string Email { get; set; } = string.Empty; // Varsayılan değer
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginDate { get; set; } // Nullable
        public bool IsActive { get; set; }
        public int PageCount { get; set; }
    }
}
