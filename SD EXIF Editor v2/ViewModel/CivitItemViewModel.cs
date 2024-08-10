using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class CivitItemViewModel : ObservableObject
    {
        private readonly CivitItem _civitItem;
        private readonly SettingsService _settingsService;

        public bool IsUnknown => _civitItem.IsUnknown;

        public string OriginalName => _civitItem.OriginalName;
        public string OriginalVersion => _civitItem.OriginalVersion;
        public string Type => _civitItem.Type;
        public double SizeKB => _civitItem.SizeKB;

        public string PromptName => _civitItem.PromptName;
        public float? Strength => _civitItem.Strength;

        public string SiteUri => _civitItem.SiteUri;
        public string DownloadUri => _civitItem.DownloadUri;


        public ObservableCollection<CivitItemImage> Images { get; set; }
        public ICollectionView FilteredImages
        {
            get
            {
                var source = CollectionViewSource.GetDefaultView(Images);
                source.Filter = i => i is CivitItemImage ci && ci.NSFWLevel <= _settingsService.NSFWLevel;
                return source;
            }
        }

        public CivitItemViewModel(CivitItem civitItem, SettingsService settingsService)
        {
            _civitItem = civitItem;
            _settingsService = settingsService;

            if (!_civitItem.IsUnknown)
            {
                Images = new ObservableCollection<CivitItemImage>(_civitItem.Images);
                FilteredImages.MoveCurrentToFirst();
                FilteredImages.CurrentChanged += FilteredImages_CurrentChanged;
            }
            else
                Images = [];

            _settingsService.PropertyChanged += _settingsService_PropertyChanged;
        }

        private void FilteredImages_CurrentChanged(object? sender, EventArgs e)
        {
            if (FilteredImages.CurrentItem is CivitItemImage civitItemImage)
            {
                foreach (CivitItemImage image in FilteredImages)
                    image.IsCurrent = false;
                civitItemImage.IsCurrent = true;
            }
        }

        private void _settingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_settingsService.NSFWLevel))
            {
                FilteredImages.Refresh();
                FilteredImages.MoveCurrentToFirst();
            }
        }


        [RelayCommand]
        public void OpenUri(string uri)
        {
            var sInfo = new System.Diagnostics.ProcessStartInfo(uri)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }
        [RelayCommand]
        public void NextImage()

        {
            if (!FilteredImages.MoveCurrentToNext())
                FilteredImages.MoveCurrentToFirst();
        }
        [RelayCommand]
        public void PrevImage()
        {
            if (!FilteredImages.MoveCurrentToPrevious())
                FilteredImages.MoveCurrentToLast();
        }
    }
}
