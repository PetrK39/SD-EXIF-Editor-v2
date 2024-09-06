using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using ExifLibrary;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
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
        public void LoadFileIntoModel(ImageModel imageModel, string filePath)
        {
            _logger.LogTrace("Entering LoadFile method.");
            _logger.LogDebug("Loading image from file path: {FilePath}", filePath);

            try
            {
                imageModel.IsFileLoaded = true;
                imageModel.FilePath = filePath;

                using (var stream = File.OpenRead(filePath))
                {
                    var imageFile = ImageFile.FromStream(stream);

                    var prop = GetMetadataProperty(imageFile);
                    imageModel.RawMetadata = prop.Value;
                }
                _logger.LogInformation("Image loaded successfully from file path: {FilePath}", filePath);
            }
            catch (Exception ex)
            {
                imageModel.IsFileLoaded = false;
                imageModel.FilePath = null;
                imageModel.RawMetadata = null;

                _logger.LogError(ex, "Failed to load image from file path: {FilePath}.", filePath);
                throw;
            }

            _logger.LogTrace("Exiting LoadFile method.");
        }

        public void SaveFile(ImageModel imageModel)
        {
            _logger.LogTrace("Entering SaveFile method.");

            if (!imageModel.IsFileLoaded)
            {
                _logger.LogWarning("Attempted to save changes without loading a file.");
                return;
            }

            try
            {
                var imageFile = ImageFile.FromFile(imageModel.FilePath);

                var prop = GetMetadataProperty(imageFile);
                prop.Value = imageModel.RawMetadata;

                imageFile.Save(imageModel.FilePath);
                _logger.LogInformation("Image changes saved successfully to file path: {FilePath}", imageModel.FilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save image changes to file path: {FilePath}.", imageModel.FilePath);
                throw;
            }

            _logger.LogTrace("Exiting SaveFile method.");
        }
        public void CloseFileFromModel(ImageModel imageModel)
        {
            imageModel.IsFileLoaded = false;
            imageModel.FilePath = null;
            imageModel.RawMetadata = null;
        }
        public async Task<Uri?> PickFile()
        {
            var result = await GetStorageProvider().OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter = [FilePickerFileTypes.ImagePng],
                Title = "Open Stable Diffusion image:"
            });

            return result.Count >= 1 ? result[0].Path : null;
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
        private IStorageProvider GetStorageProvider()
        {
            if (App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow!.StorageProvider;
            }
            else if (App.Current.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                return TopLevel.GetTopLevel(singleViewPlatform.MainView).StorageProvider;
            }

            throw new NotImplementedException();
        }
    }
}
