using ExifLibrary;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System;
using System.Linq;

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
        public void LoadFile(ref ImageModel imageModel, string filePath)
        {
            _logger.LogTrace("Entering LoadFile method.");
            _logger.LogDebug("Loading image from file path: {FilePath}", filePath);

            try
            {
                imageModel.FilePath = filePath;

                var imageFile = ImageFile.FromFile(filePath);

                var prop = GetMetadataProperty(imageFile);
                imageModel.RawMetadata = prop.Value;

                _logger.LogInformation("Image loaded successfully from file path: {FilePath}", filePath);
            }
            catch (Exception ex)
            {
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
