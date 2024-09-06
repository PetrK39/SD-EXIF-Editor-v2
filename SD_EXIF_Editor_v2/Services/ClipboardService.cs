using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.Utils;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services
{
    public class ClipboardService : IClipboardService
    {
        public async Task CopyToClipboardAsync(string text)
        {
            await AvaloniaUtils.GetClipboard().SetTextAsync(text);
        }
    }
}
