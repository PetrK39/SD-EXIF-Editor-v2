using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IClipboardService
    {
        public Task CopyToClipboardAsync(string text);
    }
}
