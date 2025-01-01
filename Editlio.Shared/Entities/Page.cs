
namespace Editlio.Shared.Entities
{
    public class Page : Entity<int>
    {
        public string Slug { get; set; } = string.Empty; // URL için benzersiz anahtar (örnek: 12345 veya abcd1)
        public string Content { get; set; } = string.Empty; // Kod veya yazı içeriği
        public bool IsProtected { get; set; } = false; // Şifre korumalı mı
        public string? PasswordHash { get; set; } // Şifre (hashlenmiş, opsiyonel)
        public int? OwnerId { get; set; } // Sayfayı oluşturan kullanıcı (opsiyonel)
        public User? Owner { get; set; } = null; // Sahiplik ilişkisi
        public ICollection<File> Files { get; set; } = new List<File>(); // Sayfaya yüklenen dosyalar
    }
}
