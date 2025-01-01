using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.DTOs.RealTimeUpdate
{
    public class RealTimeUpdateCreateDto
    {
        public int PageId { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public string ChangeDescription { get; set; } = string.Empty;
    }
}
