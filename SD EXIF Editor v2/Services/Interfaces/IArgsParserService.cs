using System.IO;
using System.Windows;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    interface IArgsParserService
    {
        public FileInfo ParseArgs(StartupEventArgs e);
    }
}
