﻿using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services
{
    public class ArgsParserService : IArgsParserService
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<ArgsParserService> _logger;

        public ArgsParserService(IMessageService messageService, ILogger<ArgsParserService> logger)
        {
            _messageService = messageService;
            _logger = logger;

            _logger.LogTrace("ArgsParserService initialized.");
        }

        public async Task<FileInfo> ParseArgsAsync(string[] args)
        {
            _logger.LogTrace("Entering ParseArgs method.");
            _logger.LogDebug($"Arguments to be parsed: {string.Join(", ", args)}");

            if (args.Length == 1 || args.Length > 2)
            {
                _logger.LogError($"Invalid number of arguments: {args.Length}");
                await _messageService.ShowErrorMessageAsync($"Usage: {Process.GetCurrentProcess()?.MainModule?.ModuleName} %path_to_png_file");
                Environment.Exit(1);
            }

            try
            {
                var file = new FileInfo(args[1]);
                _logger.LogDebug($"Attempting to parse file: {file.FullName}");

                if (!file.Exists)
                {
                    _logger.LogError($"File not exists: {file.FullName}");
                    await _messageService.ShowErrorMessageAsync($"File not exists \"{file.FullName}\"");
                    Environment.Exit(1);
                }

                if (file.Extension != ".png")
                {
                    _logger.LogError($"Unsupported file extension: {file.Extension}");
                    await _messageService.ShowErrorMessageAsync("Only .png images are supported");
                    Environment.Exit(1);
                }

                _logger.LogInformation($"File parsed successfully: {file.FullName}");
                _logger.LogTrace("Exiting ParseArgs method.");
                return file;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to parse file with error: {ex.Message}", ex);
                await _messageService.ShowErrorMessageAsync($"Failed to open file with error:\r\n{ex.Message}");
                Environment.Exit(1);

                _logger.LogTrace("Exiting ParseArgs method.");
                return null;
            }
        }
    }
}