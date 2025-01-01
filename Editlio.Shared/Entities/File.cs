using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.Entities
{
    public class File : Entity<int>
    {
        public string FileName { get; set; } = string.Empty; // Dosya adı
        public string FilePath { get; set; } = string.Empty; // Dosya depolama yolu
        public long FileSize { get; set; } // Dosya boyutu (byte)
        public string ContentType { get; set; } = string.Empty; // Dosya türü (örneğin, image/png)
        public int PageId { get; set; } // İlgili sayfa ID
        public Page Page { get; set; } = null!; // Sayfa ile ilişki
    }
}
