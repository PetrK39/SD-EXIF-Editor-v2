using SD_EXIF_Editor_v2.Model;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    interface ICivitService
    {
        public Task<CivitItem> GetItemFromHash(string origName, string origHash, float? strength = null);
    }
}
