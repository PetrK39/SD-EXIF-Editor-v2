using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SD_EXIF_Editor_v2.Service
{
    public class ArgsParserService
    {
        private readonly MessageService _messageService;
        public ArgsParserService(MessageService messageService)
        {
            _messageService = messageService;
        }
        public FileInfo ParseArgs(StartupEventArgs e)
        {
            if (e.Args.Length == 0 || e.Args.Length > 1)
                _messageService.ShowCriticalErrorMessage($"Usage: {Process.GetCurrentProcess().MainModule.ModuleName} %path_to_png_file");

            try
            {
                var file = new FileInfo(e.Args[0]);

                if (!file.Exists)
                    _messageService.ShowCriticalErrorMessage($"File not exists \"{file.FullName}\"");
                if (file.Extension != ".png")
                    _messageService.ShowCriticalErrorMessage("Only .png images are supported");

                return file;
            }
            catch (Exception ex)
            {
                _messageService.ShowCriticalErrorMessage($"Failed to open file with error:\r\n{ex.Message}");
                return null;
            }
        }
    }
}
