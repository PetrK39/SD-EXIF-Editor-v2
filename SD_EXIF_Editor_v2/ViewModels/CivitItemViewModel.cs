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
using Avalonia.Controls;
using SD_EXIF_Editor_v2.Services;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.ViewModels
{
    public partial class CivitItemViewModel : ObservableObject, ICivitItemViewModel
    {
        private CivitItem _civitItem;
        private readonly ISettingsService _settingsService;
        private readonly IUrlOpenerService _urlOpenerService;
        private readonly ILogger<CivitItemViewModel> _logger;

        public bool IsUnknown => _civitItem.IsUnknown;
        public bool IsHaveStrength => _civitItem.Strength != null;

        public string OriginalName => _civitItem.OriginalName;
        public string OriginalVersion => _civitItem.OriginalVersion;
        public string Type => _civitItem.Type;
        public double? SizeKB => _civitItem.SizeKB;

        public string PromptName => _civitItem.PromptName;
        public float? Strength => _civitItem.Strength;

        public string SiteUri => _civitItem.SiteUri;
        public string DownloadUri => _civitItem.DownloadUri;

        public IEnumerable<CivitItemImage> FilteredImages => _civitItem.Images.Where(i => (i.NSFWLevel & _settingsService.NSFWLevel) == i.NSFWLevel);
        public bool IsFilteredImagesEmpty => !FilteredImages.Any();
        // Design only
        public CivitItemViewModel()
        {
            if (Design.IsDesignMode)
            {
                _settingsService = null;
                _logger = null;

                _civitItem = new("promptName", -1.23f, "originalName", "originalVersion", "type", 123, [], "/download/uri", "site/uri");
            }
        }
        public CivitItemViewModel(ISettingsService settingsService, IUrlOpenerService urlOpenerService, ILogger<CivitItemViewModel> logger)
        {
            _settingsService = settingsService;
            _urlOpenerService = urlOpenerService;
            _logger = logger;

            _settingsService.PropertyChanged += settingsService_PropertyChanged; // TODO: check if subsciption happens multiple times

            _logger.LogTrace("CivitItemViewModel initialized.");
        }

        private void settingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_settingsService.NSFWLevel))
            {
                OnPropertyChanged(nameof(FilteredImages));
            }
        }

        public void Initialize(CivitItem civitItem)
        {
            _logger.LogTrace("Entering Initialize method.");

            _civitItem = civitItem;

            _logger.LogTrace("Exiting Initialize method.");
        }

        [RelayCommand]
        public async Task OpenUrlAsync(string uri)
        {
            _logger.LogTrace("Entering OpenUri method.");
            _logger.LogDebug($"Opening URI: {uri}");

            try
            {
                await _urlOpenerService.OpenUrlAsync(uri);
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