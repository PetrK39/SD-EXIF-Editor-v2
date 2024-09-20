using SD_EXIF_Editor_v2.Model;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface ICivitService
    {
        public Task<CivitItem> GetItemFromHash(string origName, string origHash, string fallbackType, float? strength = null);
    }
}
