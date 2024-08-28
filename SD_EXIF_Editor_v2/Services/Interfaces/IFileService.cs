using SD_EXIF_Editor_v2.Model;
using System.Threading.Tasks;
using System;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IFileService
    {
        Task<Uri?> PickFile();

        void LoadFileIntoModel(ImageModel imageModel, string filePath);
        void SaveFile(ImageModel imageModel);
    }
}
