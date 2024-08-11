using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SD_EXIF_Editor_v2.Service
{
    public class ArgsParserService : IArgsParserService
    {
        private readonly MessageService _messageService;
        private readonly ILoggingService _loggingService;

        public ArgsParserService(MessageService messageService, ILoggingService loggingService)
        {
            _messageService = messageService;
            _loggingService = loggingService;

            _loggingService.Trace("ArgsParserService initialized.");
        }

        public FileInfo ParseArgs(StartupEventArgs e)
        {
            _loggingService.Trace("Entering ParseArgs method.");
            _loggingService.Debug($"Arguments to be parsed: {string.Join(", ", e.Args)}");

            if (e.Args.Length == 0 || e.Args.Length > 1)
            {
                _loggingService.Error($"Invalid number of arguments: {e.Args.Length}");
                _messageService.ShowErrorMessage($"Usage: {Process.GetCurrentProcess()?.MainModule?.ModuleName} %path_to_png_file");
                Environment.Exit(1);
            }

            try
            {
                var file = new FileInfo(e.Args[0]);
                _loggingService.Debug($"Attempting to parse file: {file.FullName}");

                if (!file.Exists)
                {
                    _loggingService.Error($"File not exists: {file.FullName}");
                    _messageService.ShowErrorMessage($"File not exists \"{file.FullName}\"");
                    Environment.Exit(1);
                }

                if (file.Extension != ".png")
                {
                    _loggingService.Error($"Unsupported file extension: {file.Extension}");
                    _messageService.ShowErrorMessage("Only .png images are supported");
                    Environment.Exit(1);
                }

                _loggingService.Info($"File parsed successfully: {file.FullName}");
                _loggingService.Trace("Exiting ParseArgs method.");
                return file;
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Failed to parse file with error: {ex.Message}", ex);
                _messageService.ShowErrorMessage($"Failed to open file with error:\r\n{ex.Message}");
                Environment.Exit(1);

                _loggingService.Trace("Exiting ParseArgs method.");
                return null;
            }
        }
    }
}
