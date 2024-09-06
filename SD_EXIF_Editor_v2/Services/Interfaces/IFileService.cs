using SD_EXIF_Editor_v2.Model;
using System.Threading.Tasks;
using System;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IFileService
    {
        // TODO: Refactor to use IStorageFile in model and for saving/loading
        Task<Uri?> PickFile();
        Task<Uri?> PickFileToSave();

        void LoadFileIntoModel(ImageModel imageModel, string filePath);
        void SaveFileFromModel(ImageModel imageModel);
        void SaveFileAsFromModel(ImageModel imageModel, string newPath);
        void CloseFileFromModel(ImageModel imageModel);
    }
}
