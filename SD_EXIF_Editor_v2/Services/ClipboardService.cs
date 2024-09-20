using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.Utils;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services
{
    public partial class ClipboardService : IClipboardService
    {
        public static IAsyncRelayCommand<string> CopyToClipboardCommandStatic => new AsyncRelayCommand<string>(CopyToClipboardAsync, CopyToClipboardCanExecute);
        public IAsyncRelayCommand<string> CopyToClipboardCommand => CopyToClipboardCommandStatic;
        private static async Task CopyToClipboardAsync(string? text)
        {
            if(AvaloniaUtils.GetClipboard() is IClipboard clipboard)
            {
                await clipboard.SetTextAsync(text);
            }
        }
        private static bool CopyToClipboardCanExecute(string? text) => !string.IsNullOrEmpty(text);
    }
}
