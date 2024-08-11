using ExifLibrary;

namespace SD_EXIF_Editor_v2.Model
{
    public class Image
    {
        private const string MetadataFieldName = "parameters";
        private ImageFile? imageFile;

        public string? FilePath { get; private set; }
        public string RawMetadata
        {
            get => GetMetadataProperty().Value;
            set => GetMetadataProperty().Value = value;
        }

        public void LoadFromFilePath(string filePath)
        {
            FilePath = filePath;
            imageFile = ImageFile.FromFile(filePath);
        }
        public void SaveChanges()
        {
            imageFile!.Save(FilePath);
        }
        private PNGText GetMetadataProperty()
        {
            var prop = imageFile!.Properties.Where(p => p is PNGText).Cast<PNGText>().SingleOrDefault(p => p.Keyword == MetadataFieldName);

            if (prop is null)
            {
                prop = new PNGText(ExifTag.PNGText, MetadataFieldName, "", false);
                imageFile.Properties.Add(prop);
            }

            return prop;
        }
    }
}
