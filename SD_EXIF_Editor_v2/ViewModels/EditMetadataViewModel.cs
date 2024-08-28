using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.ViewModels.Interfaces;

namespace SD_EXIF_Editor_v2.ViewModels
{
    public partial class EditMetadataViewModel : ObservableObject, IEditMetadataViewModel
    {
        private readonly ImageModel _imageModel;
        private readonly ILogger _logger;
        public string RawMetadata
        {
            get => _imageModel.RawMetadata;
            set => SetProperty(_imageModel.RawMetadata, value, _imageModel, (im, v) => { im.RawMetadata = v; });
        }
        // Design only
        public EditMetadataViewModel()
        {
            if (Design.IsDesignMode)
            {
                _imageModel = new ImageModel()
                {
                    FilePath = "/test/path/to/image.png",
                    RawMetadata = "prompt\r\nNegative prompt:negative\r\nVersion: design",
                    IsFileLoaded = true
                };
                _logger = null!;
            }
        }
        public EditMetadataViewModel(ImageModel imageModel,
            ILogger<EditMetadataViewModel> logger)
        {
            _imageModel = imageModel;
            _logger = logger;
        }
        [RelayCommand]
        private void Clear()
        {
            RawMetadata = "";
        }
    }
}
