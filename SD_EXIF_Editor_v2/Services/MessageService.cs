using Avalonia.Controls.Notifications;
using Microsoft.Extensions.Logging;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.Utils;
using System;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly WindowNotificationManager _notificationManager;
        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
            _logger.LogTrace("Message Service initialized.");

            _notificationManager = new(AvaloniaUtils.TopLevel)
            {
                Position = NotificationPosition.BottomRight,
                MaxItems = 3
            };
        }
        public void ShowErrorNotification(string title, string message)
        {
            _notificationManager.Show(new Notification(title, message, NotificationType.Error, TimeSpan.FromSeconds(10)));
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

        public async Task ShowAboutDialogAsync()
        {
            await MessageBoxManager.GetMessageBoxStandard("SD Exif About", "Not implemented yet", ButtonEnum.Ok).ShowAsync();
        }
    }
}
