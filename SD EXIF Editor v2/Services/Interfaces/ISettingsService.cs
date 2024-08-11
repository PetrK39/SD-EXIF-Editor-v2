namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    interface ISettingsService
    {
        public NSFWLevels NSFWLevel{ get; set; }
        public bool DisplayPlaceholders { get; set; }
        public void Save();
    }
    public enum NSFWLevels
    {
        None = 1,
        Soft = 2,
        Mature = 4,
        X = 8,
        xxx = 16
    }
}
