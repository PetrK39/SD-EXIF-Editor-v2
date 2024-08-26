namespace SD_EXIF_Editor_v2.ViewModels.Interfaces
{
    public interface IEditMetadataViewModel
    {
        public string RawMetadata { get; set; }

        public void Clear();
    }
}
