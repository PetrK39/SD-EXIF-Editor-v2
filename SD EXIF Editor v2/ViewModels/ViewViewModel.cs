using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Factories.Interfaces;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class ViewViewModel : ObservableObject, IViewViewModel
    {
        private readonly Image _image;
        private readonly IMetadataParserService _metadataParserService;
        private readonly ICivitService _civitService;
        private readonly ISettingsService _settingsService;
        private readonly ICivitItemViewModelFactory _viewModelFactory;
        private readonly ILogger<ViewViewModel> _logger;

        private bool isCivitBusy = true;

        public string RawMetadata => _image.RawMetadata;
        public SDMetadata? Metadata { get; set; }
        public string? Prompt => Metadata?.Prompt;
        public string? NegativePrompt => Metadata?.NegativePrompt;
        public bool IsCivitBusy { get => isCivitBusy; set => SetProperty(ref isCivitBusy, value); }
        public ObservableCollection<ICivitItemViewModel> CivitItemViewModels { get; set; }

        public bool ShouldDisplayPromptHeader => !string.IsNullOrWhiteSpace(Metadata?.Prompt) || _settingsService.DisplayPlaceholders;
        public bool ShouldDisplayNegativePromptHeader => !string.IsNullOrWhiteSpace(Metadata?.NegativePrompt) || _settingsService.DisplayPlaceholders;
        public bool ShouldDisplaySeparator => !string.IsNullOrWhiteSpace(Metadata?.Prompt) || !string.IsNullOrWhiteSpace(Metadata?.NegativePrompt) || _settingsService.DisplayPlaceholders;
        public bool ShouldDisplayPlaceholder => string.IsNullOrWhiteSpace(Metadata?.Prompt) && _settingsService.DisplayPlaceholders;
        public bool ShouldDisplayNegativePlaceholder => string.IsNullOrWhiteSpace(Metadata?.NegativePrompt) && _settingsService.DisplayPlaceholders;

        public ViewViewModel(Image image,
            IMetadataParserService metadataParserService,
            ICivitService civitService,
            ISettingsService settingsService,
            ICivitItemViewModelFactory viewModelFactory,
            ILogger<ViewViewModel> logger)
        {
            _image = image;
            _metadataParserService = metadataParserService;
            _civitService = civitService;
            _settingsService = settingsService;
            _viewModelFactory = viewModelFactory;
            _logger = logger;

            _logger.LogTrace("ViewViewModel initialized.");

            _image.PropertyChanged += image_PropertyChanged;
            _settingsService.PropertyChanged += settingsService_PropertyChanged;
        }
        private void image_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _logger.LogTrace("image_PropertyChanged event triggered.");

            if (e.PropertyName == nameof(_image.RawMetadata) && _image.RawMetadata is not null)
            {
                _logger.LogDebug("RawMetadata property changed. Updating relevant properties.");

                Metadata = _metadataParserService.ParseFromRawMetadata(_image.RawMetadata);
                OnPropertyChanged(nameof(Metadata));
                OnPropertyChanged(nameof(Prompt));
                OnPropertyChanged(nameof(NegativePrompt));

                OnPropertyChanged(nameof(ShouldDisplayPromptHeader));
                OnPropertyChanged(nameof(ShouldDisplayNegativePromptHeader));
                OnPropertyChanged(nameof(ShouldDisplayPlaceholder));
                OnPropertyChanged(nameof(ShouldDisplayNegativePlaceholder));
                OnPropertyChanged(nameof(ShouldDisplaySeparator));

                CivitItemViewModels = [];
                OnPropertyChanged(nameof(CivitItemViewModels));

                LoadItemsCivitItemViewModels();
            }
        }
        private void settingsService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _logger.LogTrace("settingsService_PropertyChanged event triggered.");

            if (e.PropertyName == nameof(_settingsService.DisplayPlaceholders))
            {
                OnPropertyChanged(nameof(ShouldDisplayPromptHeader));
                OnPropertyChanged(nameof(ShouldDisplayNegativePromptHeader));
                OnPropertyChanged(nameof(ShouldDisplaySeparator));
                OnPropertyChanged(nameof(ShouldDisplayPlaceholder));
                _logger.LogDebug("DisplayPlaceholders property changed. Notifying relevant properties.");
            }
        }

        private async void LoadItemsCivitItemViewModels()
        {
            _logger.LogTrace("Entering LoadItemsCivitItemViewModels method.");
            IsCivitBusy = true;

            try
            {
                if (Metadata.Model is SDModel model)
                {
                    _logger.LogDebug($"Loading CivitItemViewModel for model: {model.Name}");
                    var vm = _viewModelFactory.Create(await _civitService.GetItemFromHash(model.Name, model.Hash, null));
                    CivitItemViewModels.Add(vm);
                }

                foreach (var lora in Metadata.Loras)
                {
                    _logger.LogDebug($"Loading CivitItemViewModel for lora: {lora.Name}");
                    var vm = _viewModelFactory.Create(await _civitService.GetItemFromHash(lora.Name, lora.Hash, lora.Strength));
                    CivitItemViewModels.Add(vm);
                }

                _logger.LogInformation("CivitItemViewModels loaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load CivitItemViewModels: {ex.Message}", ex);
            }
            finally
            {
                IsCivitBusy = false;
                _logger.LogTrace("Exiting LoadItemsCivitItemViewModels method.");
            }
        }

        [RelayCommand]
        public void Copy(string parameter)
        {
            _logger.LogTrace("Entering Copy method.");
            _logger.LogDebug($"Copying to clipboard: {parameter}");

            try
            {
                Clipboard.SetDataObject(parameter);
                _logger.LogInformation("Data copied to clipboard successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to copy data to clipboard: {ex.Message}", ex);
            }

            _logger.LogTrace("Exiting Copy method.");
        }
    }
}
