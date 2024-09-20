using SD_EXIF_Editor_v2.Model;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IMetadataParserService
    {
        public Task<SDMetadata> ParseFromRawMetadataAsync(string rawMetadata);
    }
}
