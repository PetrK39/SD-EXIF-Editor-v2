using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using System.Collections.ObjectModel;
using System.Windows;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class ViewViewModel : ObservableObject, IViewViewModel
    {
        private readonly Image _image;

        private readonly MetadataParserService _metadataParserService;
        private readonly CivitService _civitService;
        private readonly SettingsService _settingsService;

        private bool isCivitBusy = true;

        public string RawMetadata => _image.RawMetadata;
        public SDMetadata Metadata { get; set; }
        public bool IsCivitBusy { get => isCivitBusy; set => SetProperty(ref isCivitBusy, value); }
        public ObservableCollection<CivitItemViewModel> CivitItemViewModels { get; set; }

        public bool ShouldDisplayPromptHeader => !string.IsNullOrWhiteSpace(Metadata.Prompt) || _settingsService.DisplayPlaceholders;
        public bool ShouldDisplayNegativePromptHeader => !string.IsNullOrWhiteSpace(Metadata.NegativePrompt) || _settingsService.DisplayPlaceholders;
        public bool ShouldDisplaySeparator => !string.IsNullOrWhiteSpace(Metadata.Prompt) || !string.IsNullOrWhiteSpace(Metadata.NegativePrompt) || _settingsService.DisplayPlaceholders;
        public bool ShouldDisplayPlaceholder => string.IsNullOrWhiteSpace(Metadata.Prompt) && _settingsService.DisplayPlaceholders;
        public bool ShouldDisplayNegativePlaceholder => string.IsNullOrWhiteSpace(Metadata.NegativePrompt) && _settingsService.DisplayPlaceholders;

        public ViewViewModel(Image image,
            MetadataParserService metadataParserService,
            CivitService civitServide,
            SettingsService settingsService)
        {
            _image = image;

            _metadataParserService = metadataParserService;
            _civitService = civitServide;
            _settingsService = settingsService;

            Metadata = _metadataParserService.ParseFromRawMetadata(image.RawMetadata);

            CivitItemViewModels = [];

            LoadItemsCivitItemViewModels();

            _settingsService.PropertyChanged += settingsService_PropertyChanged;
        }

        private void settingsService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_settingsService.DisplayPlaceholders))
            {
                OnPropertyChanged(nameof(ShouldDisplayPromptHeader));
                OnPropertyChanged(nameof(ShouldDisplayNegativePromptHeader));
                OnPropertyChanged(nameof(ShouldDisplaySeparator));
                OnPropertyChanged(nameof(ShouldDisplayPlaceholder));
            }
        }

        private async void LoadItemsCivitItemViewModels()
        {
            IsCivitBusy = true;

            if (Metadata.Model is SDModel model)
                CivitItemViewModels.Add(new CivitItemViewModel(await _civitService.GetItemFromHash(model.Name, model.Hash, null), _settingsService));

            foreach (var lora in Metadata.Loras)
                CivitItemViewModels.Add(new CivitItemViewModel(await _civitService.GetItemFromHash(lora.Name, lora.Hash, lora.Strength), _settingsService));

            IsCivitBusy = false;
        }

        [RelayCommand]
        public void Copy(string parameter)
        {
            Clipboard.SetDataObject(parameter);
        }
    }
}
