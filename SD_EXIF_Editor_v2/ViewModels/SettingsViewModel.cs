using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Services;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.Utils;
using SD_EXIF_Editor_v2.ViewModels.Interfaces;
using System;
using System.Collections.Generic;

namespace SD_EXIF_Editor_v2.ViewModels
{
    public partial class SettingsViewModel : ObservableObject, ISettingsViewModel
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
                _logger.LogInformation("NSFWLevel changed to: {Value}", value);
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
                _logger.LogInformation("DisplayPlaceholders changed to: {Value}", value);
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
                _logger.LogInformation("Log level changed to: {Value}", value);
            }
        }
        public int MaximumLines
        {
            get => _settingsService.MaximumLines;
            set
            {
                _settingsService.MaximumLines = value;
                OnPropertyChanged();
                _settingsService.Save();
            }
        }
        public bool ShouldKeepSize
        {
            get => _settingsService.ShouldKeepSize;
            set
            {
                _settingsService.ShouldKeepSize = value;
                OnPropertyChanged();
            }
        }
        public bool ShouldKeepPos
        {
            get => _settingsService.ShouldKeepPos;
            set
            {
                _settingsService.ShouldKeepPos = value;
                OnPropertyChanged();
            }
        }
        public double WindowWidth
        {
            get => _settingsService.WindowWidth;
            set
            {
                _settingsService.WindowWidth = value;
                OnPropertyChanged();
            }
        }
        public double WindowHeight
        {
            get => _settingsService.WindowHeight;
            set
            {
                _settingsService.WindowHeight = value;
                OnPropertyChanged();
            }
        }
        public int WindowTop
        {
            get => _settingsService.WindowTop;
            set
            {
                _settingsService.WindowTop = value;
                OnPropertyChanged();
            }
        }
        public int WindowLeft
        {
            get => _settingsService.WindowLeft;
            set
            {
                _settingsService.WindowLeft = value;
                OnPropertyChanged();
            }
        }

        public Array LogLevels => Enum.GetValues(typeof(LogLevels));

        // Design only
        public SettingsViewModel()
        {
            if (Design.IsDesignMode)
            {
                _settingsService = new SettingsService();
                _logger = null;
            }
        }
        public SettingsViewModel(ISettingsService settingsService, ILogger<SettingsViewModel> logger)
        {
            _settingsService = settingsService;
            _logger = logger;

            _logger.LogTrace("SettingsViewModel initialized.");
        }

        [RelayCommand]
        private void ToggleNSFWLevelFlag(NSFWLevels flag)
        {
            if (NSFWLevel.HasFlag(flag))
            {
                NSFWLevel &= ~flag; // Remove the flag
            }
            else
            {
                NSFWLevel |= flag; // Add the flag
            }
        }
    }

}
