using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.ViewModels.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections;
using System.Linq;

namespace SD_EXIF_Editor_v2.Factories
{
    public partial class CivitItemViewModel : ObservableObject, ICivitItemViewModel
    {
        private CivitItem _civitItem;
        private readonly ISettingsService _settingsService;
        private readonly ILogger<CivitItemViewModel> _logger;

        public bool IsUnknown => _civitItem.IsUnknown;

        public string OriginalName => _civitItem.OriginalName;
        public string OriginalVersion => _civitItem.OriginalVersion;
        public string Type => _civitItem.Type;
        public double SizeKB => _civitItem.SizeKB;

        public string PromptName => _civitItem.PromptName;
        public float? Strength => _civitItem.Strength;

        public string SiteUri => _civitItem.SiteUri;
        public string DownloadUri => _civitItem.DownloadUri;

        public IEnumerable<CivitItemImage> FilteredImages => _civitItem.Images.Where(i => i.NSFWLevel < _settingsService.NSFWLevel);

        public CivitItemViewModel(ISettingsService settingsService, ILogger<CivitItemViewModel> logger)
        {
            _settingsService = settingsService;
            _logger = logger;

            _logger.LogTrace("CivitItemViewModel initialized.");

            _settingsService.PropertyChanged += _settingsService_PropertyChanged;
        }
        public void Initialize(CivitItem civitItem)
        {
            _logger.LogTrace("Entering Initialize method.");

            _civitItem = civitItem;

            _logger.LogTrace("Exiting Initialize method.");
        }

        private void _settingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _logger.LogTrace("settingsService_PropertyChanged event triggered.");

            if (e.PropertyName == nameof(_settingsService.NSFWLevel))
            {
                OnPropertyChanged(nameof(FilteredImages));
                _logger.LogDebug("NSFWLevel property changed. Refreshed FilteredImages.");
            }
        }

        [RelayCommand]
        public void OpenUri(string uri)
        {
            _logger.LogTrace("Entering OpenUri method.");
            _logger.LogDebug($"Opening URI: {uri}");

            try
            {
                var sInfo = new System.Diagnostics.ProcessStartInfo(uri)
                {
                    UseShellExecute = true,
                };
                System.Diagnostics.Process.Start(sInfo);
                _logger.LogInformation($"Successfully opened URI: {uri}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to open URI: {uri}. Error: {ex.Message}", ex);
            }

            _logger.LogTrace("Exiting OpenUri method.");
        }
    }

}