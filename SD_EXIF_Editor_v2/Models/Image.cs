using CommunityToolkit.Mvvm.ComponentModel;
using ExifLibrary;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace SD_EXIF_Editor_v2.Model
{
    public partial class Image : ObservableObject
    {
        private const string MetadataFieldName = "parameters";

        private ImageFile? imageFile;
        private string? filePath = null;

        private readonly ILogger<Image> _logger;

        public string? FilePath { get => filePath; private set => SetProperty(ref filePath, value); }
        public string? RawMetadata
        {
            get => GetMetadataProperty()?.Value;
            set
            {
                if (GetMetadataProperty() is PNGText prop)
                {
                    prop.Value = value;
                    OnPropertyChanged();
                }
            }
        }

        public Image(ILogger<Image> logger)
        {
            _logger = logger;
            _logger.LogTrace("Image class initialized.");
        }

        public void LoadFromFilePath(string filePath)
        {
            _logger.LogTrace("Entering LoadFromFilePath method.");
            _logger.LogDebug($"Loading image from file path: {filePath}");

            try
            {
                FilePath = filePath;
                imageFile = ImageFile.FromFile(filePath);
                _logger.LogInformation($"Image loaded successfully from file path: {filePath}");

                OnPropertyChanged(nameof(RawMetadata));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load image from file path: {filePath}. Error: {ex.Message}", ex);
                throw;
            }

            _logger.LogTrace("Exiting LoadFromFilePath method.");
        }

        public void SaveChanges()
        {
            _logger.LogTrace("Entering SaveChanges method.");

            if (imageFile == null || FilePath == null)
            {
                _logger.LogWarning("Attempted to save changes without loading an image file.");
                return;
            }

            try
            {
                imageFile.Save(FilePath);
                _logger.LogInformation($"Image changes saved successfully to file path: {FilePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save image changes to file path: {FilePath}. Error: {ex.Message}", ex);
                throw;
            }

            _logger.LogTrace("Exiting SaveChanges method.");
        }

        private PNGText? GetMetadataProperty()
        {
            _logger.LogTrace("Entering GetMetadataProperty method.");

            if (imageFile == null)
            {
                return null;
            }

            var prop = imageFile.Properties.Where(p => p is PNGText).Cast<PNGText>().SingleOrDefault(p => p.Keyword == MetadataFieldName);

            if (prop == null)
            {
                _logger.LogDebug($"Metadata property with keyword '{MetadataFieldName}' not found. Creating new property.");
                prop = new PNGText(ExifTag.PNGText, MetadataFieldName, "", false);
                imageFile.Properties.Add(prop);
            }

            _logger.LogTrace("Exiting GetMetadataProperty method.");
            return prop;
        }
    }
}
