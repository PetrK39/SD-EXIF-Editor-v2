using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Properties;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.Services
{
    public class SettingsService : ObservableObject, ISettingsService
    {
        public NSFWLevels NSFWLevel
        {
            get => (NSFWLevels)Settings.Default.NSFWLevel;
            set
            {
                Settings.Default.NSFWLevel = (int)value;
                OnPropertyChanged();
            }
        }
        public bool DisplayPlaceholders
        {
            get => Settings.Default.DisplayPlaceholders;
            set
            {
                Settings.Default.DisplayPlaceholders = value;
                OnPropertyChanged();
            }
        }
        public LogLevels LogLevel
        {
            get => (LogLevels)Settings.Default.LogLevel;
            set
            {
                Settings.Default.LogLevel = (int)value;
            }
        }

        public bool IsDarkTheme
        {
            get => Settings.Default.IsDarkTheme;
            set
            {
                Settings.Default.IsDarkTheme = value;
                OnPropertyChanged();
            }
        }

        public int MaximumLines
        {
            get => Settings.Default.MaximumLines;
            set
            {
                Settings.Default.MaximumLines = value;
                OnPropertyChanged();
            }
        }
        public void Save()
        {
            Settings.Default.Save();
        }
    }
}