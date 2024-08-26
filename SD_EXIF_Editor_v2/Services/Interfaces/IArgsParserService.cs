﻿using System.IO;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IArgsParserService
    {
        public Task<FileInfo> ParseArgsAsync(string[] args);
    }
}