using System.IO;
using System.Windows;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IArgsParserService
    {
        public FileInfo ParseArgs(string[] args);
    }
}
