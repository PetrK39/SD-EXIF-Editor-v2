using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.Utils;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public class SettingsViewModel : ObservableObject, ISettingsViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly ILogger<SettingsViewModel> _logger;

        public NSFWLevels NSFWLevel
        {
            get => _settingsService.NSFWLevel;
            set
            {
                _settingsService.NSFWLevel = value;
                OnPropertyChanged();
                _settingsService.Save();
                _logger.LogInformation($"NSFWLevel changed to: {value}");
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
                _logger.LogInformation($"DisplayPlaceholders changed to: {value}");
            }
        }
        public LogLevels LogLevel
        {
            get => _settingsService.LogLevel;
            set
            {
                _settingsService.LogLevel = value;
                OnPropertyChanged();
                _settingsService.Save();
                NLogConfigurator.UpdateLogLevel(value);
                _logger.LogInformation($"Log level changed to: {value}");
            }
        }

        public SettingsViewModel(ISettingsService settingsService, ILogger<SettingsViewModel> logger)
        {
            _settingsService = settingsService;
            _logger = logger;

            _logger.LogTrace("SettingsViewModel initialized.");
        }
    }
}
