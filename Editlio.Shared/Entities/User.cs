using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.Entities
{
    public class User : Entity<int>
    {
        public string Username { get; set; } = string.Empty; // Kullanıcı adı
        public string Email { get; set; } = string.Empty; // E-posta adresi
        public string PasswordHash { get; set; } = string.Empty; // Şifre (hashlenmiş)
        public ICollection<Page> Pages { get; set; } = new List<Page>(); // Kullanıcının oluşturduğu sayfalar
    }

}
