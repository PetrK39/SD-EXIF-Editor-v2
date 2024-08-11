using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public interface ISettingsViewModel
    {
        public NSFWLevels NSFWLevel { get; set; }
        public bool DisplayPlaceholders { get; set; }
    }
}
