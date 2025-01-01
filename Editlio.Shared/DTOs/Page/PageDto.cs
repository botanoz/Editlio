using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.DTOs.Page
{
    public class PageDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsProtected { get; set; }
        public int? OwnerId { get; set; }
        public string? OwnerUsername { get; set; } // Kullanıcı adını taşır
    }
}
