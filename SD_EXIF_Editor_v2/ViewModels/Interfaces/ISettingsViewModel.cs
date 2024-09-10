using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.ViewModels.Interfaces
{
    public interface ISettingsViewModel
    {
        public NSFWLevels NSFWLevel { get; set; }
        public bool DisplayPlaceholders { get; set; }
        public LogLevels LogLevel { get; set; }
        public int MaximumLines { get; set; }
        public bool ShouldKeepSize { get; set; }
        public bool ShouldKeepPos {  get; set; }
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public int WindowTop {  get; set; }
        public int WindowLeft { get; set; }
    }
}
