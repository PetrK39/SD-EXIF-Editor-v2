using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Factories.Interfaces;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.ViewModels
{
    public partial class ViewMetadataViewModel : ObservableObject, IViewMetadataViewModel
    {
        private readonly ImageModel _imageModel;
        private readonly IMetadataParserService _metadataParserService;
        private readonly ICivitService _civitService;
        private readonly ISettingsService _settingsService;
        private readonly ICivitItemViewModelFactory _civitItemViewModelFactory;
        private readonly ILogger _logger;

        private SDMetadata? _sdMetadata;

        public string FilePath => _imageModel.FilePath;
        public string RawMetadata => _imageModel.RawMetadata;
        public string? Prompt => _sdMetadata?.Prompt;
        public string? NegativePrompt => _sdMetadata?.NegativePrompt;
        public IReadOnlyDictionary<string, string> MetadataProperties => _sdMetadata?.MetadataProperties;

        [ObservableProperty]
        private bool _isCivitBusy = false;
        public ObservableCollection<ICivitItemViewModel> CivitItemViewModels { get; init; }

        // Design only
        public ViewMetadataViewModel()
        {
            if (Design.IsDesignMode)
            {
                _imageModel = new ImageModel()
                {
                    FilePath = "/test/path/to/image.png",
                    RawMetadata = "prompt\r\nNegative prompt:negative\r\nVersion: design",
                    IsFileLoaded = true
                };
                _sdMetadata = new()
                {
                    Prompt = "prompt",
                    NegativePrompt = "negative prompt",
                    Model = new SDModel("model name", "model hash"),
                    Loras = [new SDLora("lora1", "lora1h", 1.23f), new SDLora("lora2", "lora2", -1.23f)]
                };

                CivitItemViewModels = [
                    new CivitItemViewModel(),
                    new CivitItemViewModel()
                    ];

                _metadataParserService = null!;
                _civitService = null!;
                _settingsService = null!;
                _civitItemViewModelFactory = null!;
                _logger = null!;
            }
        }
        public ViewMetadataViewModel(ImageModel imageModel,
            IMetadataParserService metadataParserService,
            ICivitService civitService,
            ISettingsService settingsService,
            ICivitItemViewModelFactory viewModelFactory,
            ILogger<ViewMetadataViewModel> logger)
        {
            _imageModel = imageModel;
            _metadataParserService = metadataParserService;
            _civitService = civitService;
            _settingsService = settingsService;
            _civitItemViewModelFactory = viewModelFactory;
            _logger = logger;

            _imageModel.PropertyChanged += imageModel_PropertyChanged;
            CivitItemViewModels = [];

            UpdateSdMetadata();

            _logger.LogTrace("ViewViewModel initialized.");
        }

        [RelayCommand]
        private void CopyToClipboard(string text)
        {
            // TODO: implement the service 
            ;
        }
        private async Task UpdateSdMetadata()
        {
            _sdMetadata = await _metadataParserService.ParseFromRawMetadataAsync(_imageModel.RawMetadata);
            OnPropertyChanged(nameof(FilePath));
            OnPropertyChanged(nameof(Prompt));
            OnPropertyChanged(nameof(NegativePrompt));
            OnPropertyChanged(nameof(MetadataProperties));

            LoadItemsCivitItemViewModels();
        }
        private async void imageModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _logger.LogTrace("image_PropertyChanged event triggered.");

            if (e.PropertyName == nameof(_imageModel.RawMetadata) && _imageModel.RawMetadata is not null)
            {
                _logger.LogDebug("RawMetadata property changed. Updating relevant properties.");

                await UpdateSdMetadata();
            }
        }
        private async void LoadItemsCivitItemViewModels()
        {
            _logger.LogTrace("Entering LoadItemsCivitItemViewModels method.");
            IsCivitBusy = true;
            CivitItemViewModels.Clear();

            try
            {
                if (_sdMetadata.Model is SDModel model)
                {
                    _logger.LogDebug("Loading CivitItemViewModel for model: {ModelName}", model.Name);
                    var vm = _civitItemViewModelFactory.Create(await _civitService.GetItemFromHash(model.Name, model.Hash, null));
                    CivitItemViewModels.Add(vm);
                }

                foreach (var lora in _sdMetadata.Loras)
                {
                    _logger.LogDebug("Loading CivitItemViewModel for lora: {LoraName}", lora.Name);
                    var vm = _civitItemViewModelFactory.Create(await _civitService.GetItemFromHash(lora.Name, lora.Hash, lora.Strength));
                    CivitItemViewModels.Add(vm);
                }

                _logger.LogInformation("CivitItemViewModels loaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to load CivitItemViewModels.");
            }
            finally
            {
                IsCivitBusy = false;
                _logger.LogTrace("Exiting LoadItemsCivitItemViewModels method.");
            }
        }
    }
}
