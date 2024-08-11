using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Windows;

namespace SD_EXIF_Editor_v2.Service
{
    public class MessageService : IMessageService
    {
        private readonly ILoggingService _loggingService;
        public MessageService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _loggingService.Trace("Message Service initialized.");
        }

        public void ShowErrorMessage(string message)
        {
            _loggingService.Trace("Entering ShowErrorMessage method.");
            _loggingService.Debug($"Error message to be shown: {message}.");

            MessageBox.Show(message,
                "SD EXIF Editor Critical Error",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation);

            _loggingService.Trace("Exiting ShowErrorMessage method.");
        }

        public void ShowInfoMessage(string message)
        {
            _loggingService.Trace("Entering ShowInfoMessage method.");
            _loggingService.Debug($"Info message to be shown: {message}.");

            MessageBox.Show(message,
                "SD EXIF Editor Info",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            _loggingService.Trace("Exiting ShowInfoMessage method.");
        }

        public bool ShowConfirmationMessage(string message)
        {
            _loggingService.Trace("Entering ShowConfirmationMessage method.");
            _loggingService.Debug($"Error message to be shown: {message}.");

            var result = MessageBox.Show(message,
                "SD EXIF Editor Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.None) == MessageBoxResult.Yes;

            _loggingService.Info($"Confirmation message result: {result}.");

            _loggingService.Trace("Exiting ShowConfirmationMessage method.");
            return result;
        }
    }
}
