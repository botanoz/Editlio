using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.DTOs.File
{
    public class FileUploadDto
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public byte[] Content { get; set; } = Array.Empty<byte>();
    }
}
