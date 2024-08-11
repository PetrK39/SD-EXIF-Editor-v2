using ExifLibrary;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.Model
{
    public class Image
    {
        private const string MetadataFieldName = "parameters";

        private ImageFile? imageFile;

        private readonly ILoggingService _logger;

        public string? FilePath { get; private set; }
        public string RawMetadata
        {
            get => GetMetadataProperty().Value;
            set => GetMetadataProperty().Value = value;
        }

        public Image(ILoggingService logger)
        {
            _logger = logger;
            _logger.Trace("Image class initialized.");
        }

        public void LoadFromFilePath(string filePath)
        {
            _logger.Trace("Entering LoadFromFilePath method.");
            _logger.Debug($"Loading image from file path: {filePath}");

            try
            {
                FilePath = filePath;
                imageFile = ImageFile.FromFile(filePath);
                _logger.Info($"Image loaded successfully from file path: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to load image from file path: {filePath}. Error: {ex.Message}", ex);
                throw;
            }

            _logger.Trace("Exiting LoadFromFilePath method.");
        }

        public void SaveChanges()
        {
            _logger.Trace("Entering SaveChanges method.");

            if (imageFile == null || FilePath == null)
            {
                _logger.Warn("Attempted to save changes without loading an image file.");
                return;
            }

            try
            {
                imageFile.Save(FilePath);
                _logger.Info($"Image changes saved successfully to file path: {FilePath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to save image changes to file path: {FilePath}. Error: {ex.Message}", ex);
                throw;
            }

            _logger.Trace("Exiting SaveChanges method.");
        }

        private PNGText GetMetadataProperty()
        {
            _logger.Trace("Entering GetMetadataProperty method.");

            if (imageFile == null)
            {
                _logger.Warn("Attempted to get metadata property without loading an image file.");
                throw new InvalidOperationException("Image file is not loaded.");
            }

            var prop = imageFile.Properties.Where(p => p is PNGText).Cast<PNGText>().SingleOrDefault(p => p.Keyword == MetadataFieldName);

            if (prop == null)
            {
                _logger.Debug($"Metadata property with keyword '{MetadataFieldName}' not found. Creating new property.");
                prop = new PNGText(ExifTag.PNGText, MetadataFieldName, "", false);
                imageFile.Properties.Add(prop);
            }

            _logger.Trace("Exiting GetMetadataProperty method.");
            return prop;
        }
    }
}
