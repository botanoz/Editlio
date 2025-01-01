namespace Editlio.Web.Models
{
    public class PageViewModel
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty; // Varsayılan değer
        public string Content { get; set; } = string.Empty; // Varsayılan değer
        public bool IsProtected { get; set; }
        public string? PasswordHint { get; set; } // Nullable
        public int? OwnerId { get; set; } // Nullable
        public string? OwnerUsername { get; set; } // Nullable
        public string? OwnerEmail { get; set; } // Nullable
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } // Nullable
        public List<FileViewModel> Files { get; set; } = new(); // Varsayılan boş liste
        public int TotalFiles { get; set; }
        public bool IsEditable { get; set; }
    }
}
