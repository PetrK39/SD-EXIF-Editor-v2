using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services
{
    public class StartupFileService : IStartupFileService
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<StartupFileService> _logger;

        public StartupFileService(IMessageService messageService, ILogger<StartupFileService> logger)
        {
            _messageService = messageService;
            _logger = logger;

            _logger.LogTrace("ArgsParserService initialized.");
        }

        public async Task<string?> GetStartupFileAsync()
        {
            var args = Environment.GetCommandLineArgs();

            if(args.Length > 2)
            {
                await _messageService.ShowErrorMessageAsync($"Usage: {Process.GetCurrentProcess()?.MainModule?.ModuleName} %path_to_png_file");
                return null;
            }

            try
            {
                var file = new FileInfo(args[1]);

                if (!file.Exists)
                {
                    await _messageService.ShowErrorMessageAsync($"File not exists \"{file.FullName}\"");
                    return null;
                }

                if (file.Extension.ToLower() != ".png")
                {
                    await _messageService.ShowErrorMessageAsync("Only .png images are supported");
                    return null;
                }

                return file.FullName;
            }
            catch (Exception ex)
            {
                await _messageService.ShowErrorMessageAsync($"Failed to open file with error:\r\n{ex.Message}");
                return null;
            }
        }
    }
}
