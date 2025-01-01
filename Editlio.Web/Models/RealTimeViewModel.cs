namespace Editlio.Web.Models
{
    public class RealTimeViewModel
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public string UpdatedBy { get; set; } = string.Empty; // Varsayılan değer
        public string ChangeDescription { get; set; } = string.Empty; // Varsayılan değer
        public DateTime UpdatedAt { get; set; }
        public string UpdateType { get; set; } = string.Empty; // Varsayılan değer
    }
}
