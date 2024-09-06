using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using ExifLibrary;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.Utils;
using SD_EXIF_Editor_v2.Views;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services
{
    public class FileService : IFileService
    {
        private const string MetadataFieldName = "parameters";
        private readonly ILogger _logger;
        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;

            _logger.LogTrace("FileService initialized.");
        }
        public async Task LoadFileIntoModelAsync(ImageModel imageModel, Uri fileUri)
        {
            _logger.LogTrace("Entering LoadFile method.");
            _logger.LogDebug("Loading image from file path: {FileUri}", fileUri);

            try
            {
                imageModel.IsFileLoaded = true;
                imageModel.FileUri = fileUri;

                var storageFile = await StorageProviderUtils.GetStorageProvider().TryGetFileFromPathAsync(fileUri);

                using (var stream = await storageFile.OpenReadAsync())
                {
                    var imageFile = ImageFile.FromStream(stream);

                    var prop = GetMetadataProperty(imageFile);
                    imageModel.RawMetadata = prop.Value;
                }

                _logger.LogInformation("Image loaded successfully from file uri");
            }
            catch (Exception ex)
            {
                imageModel.IsFileLoaded = false;
                imageModel.FileUri = null;
                imageModel.RawMetadata = null;

                _logger.LogError(ex, "Failed to load image from file uri.");
                throw;
            }

            _logger.LogTrace("Exiting LoadFile method.");
        }

        public async Task SaveFileFromModelAsync(ImageModel imageModel)
        {
            _logger.LogTrace("Entering SaveFile method.");

            if (!imageModel.IsFileLoaded)
            {
                _logger.LogWarning("Attempted to save changes without loading a file.");
                return;
            }

            try
            {
                var file = await StorageProviderUtils.GetStorageProvider().TryGetFileFromPathAsync(imageModel.FileUri!);

                if (file is null)
                {
                    throw new FileNotFoundException("File not found", imageModel.FileUri!.LocalPath);
                }

                ImageFile? imageFile = null;

                using (var streamRead = await file.OpenReadAsync())
                {
                    imageFile = await ImageFile.FromStreamAsync(streamRead);
                }

                var prop = GetMetadataProperty(imageFile);
                prop.Value = imageModel.RawMetadata;

                using (var streamWrite = await file.OpenWriteAsync())
                {
                    await imageFile.SaveAsync(streamWrite);
                }

                _logger.LogInformation("Image changes saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save image changes to file.");
                throw;
            }

            _logger.LogTrace("Exiting SaveFile method.");
        }
        public async Task SaveFileAsFromModelAsync(ImageModel imageModel, IStorageFile newFile)
        {
            if (!imageModel.IsFileLoaded) return;

            var sp = StorageProviderUtils.GetStorageProvider();

            var oldFile = await sp.TryGetFileFromPathAsync(imageModel.FileUri!);

            if (oldFile is null)
            {
                throw new FileNotFoundException("File not found", imageModel.FileUri!.LocalPath);
            }

            if (newFile is null)
            {
                throw new FileNotFoundException("No new file specified");
            }

            using (var streamOld = await oldFile.OpenReadAsync())
            using (var streamNew = await newFile.OpenWriteAsync())
            {
                await streamOld.CopyToAsync(streamNew);
            }

            imageModel.FileUri = newFile.Path;

            await SaveFileFromModelAsync(imageModel);
        }
        public void CloseFileFromModel(ImageModel imageModel)
        {
            imageModel.IsFileLoaded = false;
            imageModel.FileUri = null;
            imageModel.RawMetadata = null;
        }
        public async Task<Uri?> PickFileToLoad()
        {
            var result = await StorageProviderUtils.GetStorageProvider().OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter = [FilePickerFileTypes.ImagePng],
                Title = "Open Stable Diffusion image:"
            });

            return result.Count >= 1 ? result[0].Path : null;
        }
        public async Task<IStorageFile?> PickFileToSave()
        {
            var result = await StorageProviderUtils.GetStorageProvider().SaveFilePickerAsync(new FilePickerSaveOptions
            {
                ShowOverwritePrompt = true,
                Title = "Save Stable Diffusion image:"
            });

            return result;
        }
        private PNGText GetMetadataProperty(ImageFile imageFile)
        {
            var prop = imageFile.Properties.Where(p => p is PNGText).Cast<PNGText>().SingleOrDefault(p => p.Keyword == MetadataFieldName);
            if (prop == null)
            {
                _logger.LogDebug($"Metadata property with keyword '{MetadataFieldName}' not found. Creating new property.");
                prop = new PNGText(ExifTag.PNGText, MetadataFieldName, "", false);
                imageFile.Properties.Add(prop);
            }
            return prop;
        }
    }
}
