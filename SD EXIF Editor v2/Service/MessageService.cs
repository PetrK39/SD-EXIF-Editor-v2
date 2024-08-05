using System.Windows;

namespace SD_EXIF_Editor_v2.Service
{
    public class MessageService
    {
        public void ShowCriticalErrorMessage(string message)
        {
            MessageBox.Show(message,
                "SD EXIF Editor Critical Error",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation);

            Environment.Exit(1);
        }

        public void ShowInfoMessage(string message)
        {
            MessageBox.Show(message,
                "SD EXIF Editor Critical Error",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
