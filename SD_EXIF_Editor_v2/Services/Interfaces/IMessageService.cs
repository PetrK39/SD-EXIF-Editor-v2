using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IMessageService
    {
        public Task ShowErrorMessageAsync(string message);
        public void ShowErrorNotification(string title, string message);
        public Task ShowInfoMessageAsync(string message);
        public Task<bool> ShowConfirmationMessageAsync(string message);
        public Task ShowAboutDialogAsync();
        public Task<bool?> ShowExitConfirmationDialogAsync();
    }
}
