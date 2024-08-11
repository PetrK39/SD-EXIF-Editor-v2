using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public class SettingsViewModel : ObservableObject, ISettingsViewModel
    {
        private readonly SettingsService _settingsService;
        private readonly ILoggingService _loggingService;

        public NSFWLevels NSFWLevel
        {
            get => _settingsService.NSFWLevel;
            set
            {
                _settingsService.NSFWLevel = value;
                OnPropertyChanged();
                _settingsService.Save();
                _loggingService.Info($"NSFWLevel changed to: {value}");
            }
        }

        public bool DisplayPlaceholders
        {
            get => _settingsService.DisplayPlaceholders;
            set
            {
                _settingsService.DisplayPlaceholders = value;
                OnPropertyChanged();
                _settingsService.Save();
                _loggingService.Info($"DisplayPlaceholders changed to: {value}");
            }
        }

        public SettingsViewModel(SettingsService settingsService, ILoggingService loggingService)
        {
            _settingsService = settingsService;
            _loggingService = loggingService;

            _loggingService.Trace("SettingsViewModel initialized.");
        }
    }
}
