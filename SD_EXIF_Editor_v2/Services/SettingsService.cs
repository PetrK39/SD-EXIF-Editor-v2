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

        public bool ShouldKeepSize
        {
            get => Settings.Default.ShouldKeepSize;
            set
            {
                Settings.Default.ShouldKeepSize = value;
                OnPropertyChanged();
            }
        }
        public bool ShouldKeepPos
        {
            get => Settings.Default.ShouldKeepPos;
            set
            {
                Settings.Default.ShouldKeepPos = value;
                OnPropertyChanged();
            }
        }
        public double WindowWidth
        {
            get => Settings.Default.WindowWidth;
            set
            {
                Settings.Default.WindowWidth = value;
                OnPropertyChanged();
            }
        }
        public double WindowHeight
        {
            get => Settings.Default.WindowHeight;
            set
            {
                Settings.Default.WindowHeight = value;
                OnPropertyChanged();
            }
        }
        public int WindowLeft
        {
            get => Settings.Default.WindowLeft;
            set
            {
                Settings.Default.WindowLeft = value;
                OnPropertyChanged();
            }
        }
        public int WindowTop
        {
            get => Settings.Default.WindowTop;
            set
            {
                Settings.Default.WindowTop = value;
                OnPropertyChanged();
            }
        }

        public void Save()
        {
            Settings.Default.Save();
        }
    }
}