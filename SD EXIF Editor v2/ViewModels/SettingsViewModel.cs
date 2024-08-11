using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Service;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public class SettingsViewModel : ObservableObject, ISettingsViewModel
    {
        private readonly SettingsService _settingsService;

        public NSFWLevels NSFWLevel
        {
            get => _settingsService.NSFWLevel;
            set
            {
                _settingsService.NSFWLevel = value;
                OnPropertyChanged();
                _settingsService.Save();
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
            }
        }

        public SettingsViewModel(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }
    }
}
