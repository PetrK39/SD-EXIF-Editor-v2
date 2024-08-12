using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Windows;

namespace SD_EXIF_Editor_v2.Service
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
            _logger.LogTrace("Message Service initialized.");
        }

        public void ShowErrorMessage(string message)
        {
            _logger.LogTrace("Entering ShowErrorMessage method.");
            _logger.LogDebug($"Error message to be shown: {message}.");

            MessageBox.Show(message,
                "SD EXIF Editor Critical Error",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation);

            _logger.LogTrace("Exiting ShowErrorMessage method.");
        }

        public void ShowInfoMessage(string message)
        {
            _logger.LogTrace("Entering ShowInfoMessage method.");
            _logger.LogDebug($"Info message to be shown: {message}.");

            MessageBox.Show(message,
                "SD EXIF Editor Info",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            _logger.LogTrace("Exiting ShowInfoMessage method.");
        }

        public bool ShowConfirmationMessage(string message)
        {
            _logger.LogTrace("Entering ShowConfirmationMessage method.");
            _logger.LogDebug($"Error message to be shown: {message}.");

            var result = MessageBox.Show(message,
                "SD EXIF Editor Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.None) == MessageBoxResult.Yes;

            _logger.LogInformation($"Confirmation message result: {result}.");

            _logger.LogTrace("Exiting ShowConfirmationMessage method.");
            return result;
        }
    }
}
