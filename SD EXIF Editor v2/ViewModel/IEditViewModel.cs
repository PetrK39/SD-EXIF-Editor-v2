namespace SD_EXIF_Editor_v2.ViewModel
{
    public interface IEditViewModel
    {
        public string RawMetadata { get; set; }

        public void Delete();
        public void Save();
    }
}
