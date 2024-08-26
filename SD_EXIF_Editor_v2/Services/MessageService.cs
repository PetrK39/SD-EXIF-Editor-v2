using Microsoft.Extensions.Logging;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
            _logger.LogTrace("Message Service initialized.");
        }

        public async Task ShowErrorMessageAsync(string message)
        {
            _logger.LogTrace("Entering ShowErrorMessage method.");
            _logger.LogDebug($"Error message to be shown: {message}.");

            await MessageBoxManager.GetMessageBoxStandard("SD Exif Editor Error",
                message,
                ButtonEnum.Ok,
                Icon.Error).ShowAsync();

            _logger.LogTrace("Exiting ShowErrorMessage method.");
        }

        public async Task ShowInfoMessageAsync(string message)
        {
            _logger.LogTrace("Entering ShowInfoMessage method.");
            _logger.LogDebug($"Info message to be shown: {message}.");

            await MessageBoxManager.GetMessageBoxStandard("SD Exif Editor Info",
                message,
                ButtonEnum.Ok).ShowAsync();

            _logger.LogTrace("Exiting ShowInfoMessage method.");
        }

        public async Task<bool> ShowConfirmationMessageAsync(string message)
        {
            _logger.LogTrace("Entering ShowConfirmationMessage method.");
            _logger.LogDebug($"Error message to be shown: {message}.");

            var result = await MessageBoxManager.GetMessageBoxStandard("SD Exif Editor Confirmation",
                message,
                ButtonEnum.YesNo,
                Icon.None).ShowAsync() == ButtonResult.Yes;

            _logger.LogInformation($"Confirmation message result: {result}.");

            _logger.LogTrace("Exiting ShowConfirmationMessage method.");
            return result;
        }
    }
}
