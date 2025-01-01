namespace Editlio.Web.Models
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty; // Varsayılan değer
        public string FilePath { get; set; } = string.Empty; // Varsayılan değer
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty; // Varsayılan değer
        public int PageId { get; set; }
        public DateTime UploadedAt { get; set; }
        public bool IsDownloadable { get; set; }
    }
}
