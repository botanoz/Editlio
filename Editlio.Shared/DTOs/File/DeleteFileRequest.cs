﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.DTOs.File
{
    public class DeleteFileRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
