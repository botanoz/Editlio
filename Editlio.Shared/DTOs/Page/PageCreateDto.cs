using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.DTOs.Page
{
    public class PageCreateDto
    {
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsProtected { get; set; }
        public string? Password { get; set; }
    }
}
