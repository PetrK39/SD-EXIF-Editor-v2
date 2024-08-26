using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IMessageService
    {
        public Task ShowErrorMessageAsync(string message);
        public Task ShowInfoMessageAsync(string message);
        public Task<bool> ShowConfirmationMessageAsync(string message);
    }
}
