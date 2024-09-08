using System;
using System.ComponentModel;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface ISettingsService : INotifyPropertyChanged
    {
        public NSFWLevels NSFWLevel{ get; set; }
        public bool DisplayPlaceholders { get; set; }
        public LogLevels LogLevel { get; set; }
        public bool IsDarkTheme { get; set; }
        public int MaximumLines { get; set; }
        public void Save();
    }

    [Flags]
    public enum NSFWLevels
    {
        None = 1,
        Soft = 2,
        Mature = 4,
        X = 8,
        XXX = 16
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
