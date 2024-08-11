using System.Diagnostics;
using System.IO;
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
            {
                _messageService.ShowErrorMessage($"Usage: {Process.GetCurrentProcess()?.MainModule?.ModuleName} %path_to_png_file");
                Environment.Exit(1);
            }

            try
            {
                var file = new FileInfo(e.Args[0]);

                if (!file.Exists)
                {
                    _messageService.ShowErrorMessage($"File not exists \"{file.FullName}\"");
                    Environment.Exit(1);
                }

                if (file.Extension != ".png")
                {
                    _messageService.ShowErrorMessage("Only .png images are supported");
                    Environment.Exit(1);
                }

                return file;
            }
            catch (Exception ex)
            {
                _messageService.ShowErrorMessage($"Failed to open file with error:\r\n{ex.Message}");
                Environment.Exit(1);

                return null;
            }
        }
    }
}
