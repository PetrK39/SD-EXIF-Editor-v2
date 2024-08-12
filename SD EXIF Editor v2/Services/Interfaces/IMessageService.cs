namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IMessageService
    {
        public void ShowErrorMessage(string message);
        public void ShowInfoMessage(string message);
        public bool ShowConfirmationMessage(string message);
    }
}
