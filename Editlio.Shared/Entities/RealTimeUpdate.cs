using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.Entities
{
    public class RealTimeUpdate : Entity<int>
    {
        public int PageId { get; set; } // İlgili sayfa ID
        public Page Page { get; set; } = null!; // Sayfa ile ilişki
        public string UpdatedBy { get; set; } = string.Empty; // Değişikliği yapan kullanıcı adı
        public string ChangeDescription { get; set; } = string.Empty; // Yapılan değişikliklerin açıklaması
    }

}
