using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Windows;

namespace SD_EXIF_Editor_v2.Service
{
    public class MessageService : IMessageService
    {
        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message,
                "SD EXIF Editor Critical Error",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation);
        }

        public void ShowInfoMessage(string message)
        {
            MessageBox.Show(message,
                "SD EXIF Editor Info",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public bool ShowConfirmationMessage(string message)
        {
            return MessageBox.Show(message,
                "SD EXIF Editor Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.None) == MessageBoxResult.Yes;
        }
    }
}
