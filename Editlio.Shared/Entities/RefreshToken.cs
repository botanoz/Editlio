using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.Entities
{
    public class RefreshToken : Entity<int>
    {
        public string Token { get; set; } = string.Empty; // Token değeri
        public DateTime ExpiresAt { get; set; } // Token süresi
        public bool IsRevoked { get; set; } = false; // Token geçersiz mi
        public int UserId { get; set; } // İlgili kullanıcı ID
        public User User { get; set; } = null!; // Kullanıcı ile ilişki
    }

}
