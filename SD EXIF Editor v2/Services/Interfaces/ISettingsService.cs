using System.ComponentModel;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface ISettingsService : INotifyPropertyChanged
    {
        public NSFWLevels NSFWLevel{ get; set; }
        public bool DisplayPlaceholders { get; set; }
        public LogLevels LogLevel { get; set; }
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
    public enum LogLevels
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}
