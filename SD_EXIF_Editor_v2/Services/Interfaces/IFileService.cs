using SD_EXIF_Editor_v2.Model;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IFileService
    {
        void LoadFile(ref ImageModel imageModel, string filePath);
        void SaveFile(ImageModel imageModel);
    }
}
