using SD_EXIF_Editor_v2.Model;
using System.Threading.Tasks;
using System;
using Avalonia.Platform.Storage;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IFileService
    {
        Task<Uri?> PickFileToLoad();
        Task<IStorageFile?> PickFileToSave();

        Task LoadFileIntoModelAsync(ImageModel imageModel, Uri fileUri);
        Task SaveFileFromModelAsync(ImageModel imageModel);
        Task SaveFileAsFromModelAsync(ImageModel imageModel, IStorageFile newFile);
        void CloseFileFromModel(ImageModel imageModel);
    }
}
