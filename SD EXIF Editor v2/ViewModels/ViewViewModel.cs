using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;
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
        private readonly ILoggingService _loggingService;

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
            CivitService civitService,
            SettingsService settingsService,
            ILoggingService loggingService)
        {
            _image = image;
            _metadataParserService = metadataParserService;
            _civitService = civitService;
            _settingsService = settingsService;
            _loggingService = loggingService;

            _loggingService.Trace("ViewViewModel initialized.");

            Metadata = _metadataParserService.ParseFromRawMetadata(image.RawMetadata);

            CivitItemViewModels = new ObservableCollection<CivitItemViewModel>();

            LoadItemsCivitItemViewModels();

            _settingsService.PropertyChanged += settingsService_PropertyChanged;
        }

        private void settingsService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _loggingService.Trace("settingsService_PropertyChanged event triggered.");

            if (e.PropertyName == nameof(_settingsService.DisplayPlaceholders))
            {
                OnPropertyChanged(nameof(ShouldDisplayPromptHeader));
                OnPropertyChanged(nameof(ShouldDisplayNegativePromptHeader));
                OnPropertyChanged(nameof(ShouldDisplaySeparator));
                OnPropertyChanged(nameof(ShouldDisplayPlaceholder));
                _loggingService.Debug("DisplayPlaceholders property changed. Notifying relevant properties.");
            }
        }

        private async void LoadItemsCivitItemViewModels()
        {
            _loggingService.Trace("Entering LoadItemsCivitItemViewModels method.");
            IsCivitBusy = true;

            try
            {
                if (Metadata.Model is SDModel model)
                {
                    _loggingService.Debug($"Loading CivitItemViewModel for model: {model.Name}");
                    CivitItemViewModels.Add(new CivitItemViewModel(await _civitService.GetItemFromHash(model.Name, model.Hash, null), _settingsService, _loggingService));
                }

                foreach (var lora in Metadata.Loras)
                {
                    _loggingService.Debug($"Loading CivitItemViewModel for lora: {lora.Name}");
                    CivitItemViewModels.Add(new CivitItemViewModel(await _civitService.GetItemFromHash(lora.Name, lora.Hash, lora.Strength), _settingsService, _loggingService));
                }

                _loggingService.Info("CivitItemViewModels loaded successfully.");
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Failed to load CivitItemViewModels: {ex.Message}", ex);
            }
            finally
            {
                IsCivitBusy = false;
                _loggingService.Trace("Exiting LoadItemsCivitItemViewModels method.");
            }
        }

        [RelayCommand]
        public void Copy(string parameter)
        {
            _loggingService.Trace("Entering Copy method.");
            _loggingService.Debug($"Copying to clipboard: {parameter}");

            try
            {
                Clipboard.SetDataObject(parameter);
                _loggingService.Info("Data copied to clipboard successfully.");
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Failed to copy data to clipboard: {ex.Message}", ex);
            }

            _loggingService.Trace("Exiting Copy method.");
        }
    }
}
