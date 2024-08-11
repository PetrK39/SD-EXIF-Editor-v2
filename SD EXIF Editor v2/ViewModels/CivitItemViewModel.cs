using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private readonly CivitItem _civitItem;
        private readonly SettingsService _settingsService;
        private readonly ILoggingService _loggingService;

        public bool IsUnknown => _civitItem.IsUnknown;

        public string OriginalName => _civitItem.OriginalName;
        public string OriginalVersion => _civitItem.OriginalVersion;
        public string Type => _civitItem.Type;
        public double SizeKB => _civitItem.SizeKB;

        public string PromptName => _civitItem.PromptName;
        public float? Strength => _civitItem.Strength;

        public string SiteUri => _civitItem.SiteUri;
        public string DownloadUri => _civitItem.DownloadUri;

        public ObservableCollection<CivitItemImage> Images { get; init; }
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

        public CivitItemViewModel(CivitItem civitItem, SettingsService settingsService, ILoggingService loggingService)
        {
            _civitItem = civitItem;
            _settingsService = settingsService;
            _loggingService = loggingService;

            _loggingService.Trace("CivitItemViewModel initialized.");

            if (!_civitItem.IsUnknown)
            {
                Images = new ObservableCollection<CivitItemImage>(_civitItem.Images);
                FilteredImages.MoveCurrentToFirst();
                FilteredImages.CurrentChanged += FilteredImages_CurrentChanged;
            }
            else
            {
                Images = new ObservableCollection<CivitItemImage>();
            }

            _settingsService.PropertyChanged += _settingsService_PropertyChanged;
        }

        private void FilteredImages_CurrentChanged(object? sender, EventArgs e)
        {
            _loggingService.Trace("FilteredImages_CurrentChanged event triggered.");

            if (FilteredImages.CurrentItem is CivitItemImage civitItemImage)
            {
                foreach (CivitItemImage image in FilteredImages)
                    image.IsCurrent = false;
                civitItemImage.IsCurrent = true;
                _loggingService.Debug($"Current image changed to: {civitItemImage.Uri}");
            }
        }

        private void _settingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _loggingService.Trace("settingsService_PropertyChanged event triggered.");

            if (e.PropertyName == nameof(_settingsService.NSFWLevel))
            {
                FilteredImages.Refresh();
                FilteredImages.MoveCurrentToFirst();
                _loggingService.Debug("NSFWLevel property changed. Refreshed FilteredImages and moved to first.");

                OnPropertyChanged(nameof(IsNotEmpty));
            }
        }

        [RelayCommand]
        public void OpenUri(string uri)
        {
            _loggingService.Trace("Entering OpenUri method.");
            _loggingService.Debug($"Opening URI: {uri}");

            try
            {
                var sInfo = new System.Diagnostics.ProcessStartInfo(uri)
                {
                    UseShellExecute = true,
                };
                System.Diagnostics.Process.Start(sInfo);
                _loggingService.Info($"Successfully opened URI: {uri}");
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Failed to open URI: {uri}. Error: {ex.Message}", ex);
            }

            _loggingService.Trace("Exiting OpenUri method.");
        }

        [RelayCommand]
        public void NextImage()
        {
            _loggingService.Trace("Entering NextImage method.");

            if (!FilteredImages.MoveCurrentToNext())
                FilteredImages.MoveCurrentToFirst();
            _loggingService.Debug("Moved to next image.");

            _loggingService.Trace("Exiting NextImage method.");
        }

        [RelayCommand]
        public void PrevImage()
        {
            _loggingService.Trace("Entering PrevImage method.");

            if (!FilteredImages.MoveCurrentToPrevious())
                FilteredImages.MoveCurrentToLast();
            _loggingService.Debug("Moved to previous image.");

            _loggingService.Trace("Exiting PrevImage method.");
        }

        [RelayCommand]
        public void GoToImage(CivitItemImage image)
        {
            _loggingService.Trace("Entering GoToImage method.");
            _loggingService.Debug($"Going to image: {image.Uri}");

            FilteredImages.MoveCurrentTo(image);
            _loggingService.Info($"Successfully moved to image: {image.Uri}");

            _loggingService.Trace("Exiting GoToImage method.");
        }
    }
}
