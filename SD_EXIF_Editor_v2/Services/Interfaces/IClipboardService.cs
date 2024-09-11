using CommunityToolkit.Mvvm.Input;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IClipboardService
    {
        public IAsyncRelayCommand<string> CopyToClipboardCommand { get; }
        public static IAsyncRelayCommand<string> CopyToClipboardCommandStatic { get; }
    }
}
