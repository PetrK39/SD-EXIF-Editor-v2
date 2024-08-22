using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace SD_EXIF_Editor_v2.ViewModel
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

        public ObservableCollection<CivitItemImage> Images { get; private set; }
        public ICollectionView FilteredImages
        {
            get
            {
                var source = CollectionViewSource.GetDefaultView(Images);
                source.Filter = i => i is CivitItemImage ci && ci.NSFWLevel <= _settingsService.NSFWLevel;
                return source;
            }
        }

        public bool IsHaveStrength => Strength is not null;
        public bool IsNotEmpty => !FilteredImages.IsEmpty;

        public CivitItemViewModel(ISettingsService settingsService, ILogger<CivitItemViewModel> logger)
        {
            _settingsService = settingsService;
            _logger = logger;

            _logger.LogTrace("CivitItemViewModel initialized.");

            Images = [];

            _settingsService.PropertyChanged += _settingsService_PropertyChanged;
        }
        public void Initialize(CivitItem civitItem)
        {
            _logger.LogTrace("Entering Initialize method.");

            _civitItem = civitItem;

            if (!_civitItem.IsUnknown)
            {
                Images = new ObservableCollection<CivitItemImage>(_civitItem.Images);
                OnPropertyChanged(nameof(Images));

                FilteredImages.MoveCurrentToFirst();
                FilteredImages.CurrentChanged += FilteredImages_CurrentChanged;
            }

            _logger.LogTrace("Exiting Initialize method.");
        }

        private void FilteredImages_CurrentChanged(object? sender, EventArgs e)
        {
            _logger.LogTrace("FilteredImages_CurrentChanged event triggered.");

            if (FilteredImages.CurrentItem is CivitItemImage civitItemImage)
            {
                foreach (CivitItemImage image in FilteredImages)
                    image.IsCurrent = false;
                civitItemImage.IsCurrent = true;
                _logger.LogDebug($"Current image changed to: {civitItemImage.Uri}");
            }
        }

        private void _settingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _logger.LogTrace("settingsService_PropertyChanged event triggered.");

            if (e.PropertyName == nameof(_settingsService.NSFWLevel))
            {
                FilteredImages.Refresh();
                FilteredImages.MoveCurrentToFirst();
                _logger.LogDebug("NSFWLevel property changed. Refreshed FilteredImages and moved to first.");

                OnPropertyChanged(nameof(IsNotEmpty));
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

        [RelayCommand]
        public void NextImage()
        {
            _logger.LogTrace("Entering NextImage method.");

            if (!FilteredImages.MoveCurrentToNext())
                FilteredImages.MoveCurrentToFirst();
            _logger.LogDebug("Moved to next image.");

            _logger.LogTrace("Exiting NextImage method.");
        }

        [RelayCommand]
        public void PrevImage()
        {
            _logger.LogTrace("Entering PrevImage method.");

            if (!FilteredImages.MoveCurrentToPrevious())
                FilteredImages.MoveCurrentToLast();
            _logger.LogDebug("Moved to previous image.");

            _logger.LogTrace("Exiting PrevImage method.");
        }

        [RelayCommand]
        public void GoToImage(CivitItemImage image)
        {
            _logger.LogTrace("Entering GoToImage method.");
            _logger.LogDebug($"Going to image: {image.Uri}");

            FilteredImages.MoveCurrentTo(image);
            _logger.LogInformation($"Successfully moved to image: {image.Uri}");

            _logger.LogTrace("Exiting GoToImage method.");
        }
    }
}
