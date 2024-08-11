using SD_EXIF_Editor_v2.Model;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    interface IMetadataParserService
    {
        public SDMetadata ParseFromRawMetadata(string rawMetadata);
    }
}
