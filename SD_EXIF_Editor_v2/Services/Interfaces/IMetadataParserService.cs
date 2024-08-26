using SD_EXIF_Editor_v2.Model;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IMetadataParserService
    {
        public SDMetadata ParseFromRawMetadata(string rawMetadata);
    }
}
