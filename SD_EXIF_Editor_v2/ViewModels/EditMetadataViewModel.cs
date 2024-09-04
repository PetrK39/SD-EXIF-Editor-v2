using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.ViewModels.Interfaces;
using System;

namespace SD_EXIF_Editor_v2.ViewModels
{
    public partial class EditMetadataViewModel : ObservableObject, IEditMetadataViewModel, IDisposable
    {
        private readonly ImageModel _imageModel;
        private readonly ILogger _logger;
        private bool disposedValue;

        public bool IsFileLoaded => _imageModel.IsFileLoaded;
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
                    IsFileLoaded = false
                };
                _logger = null!;
            }
        }
        public EditMetadataViewModel(ImageModel imageModel,
            ILogger<EditMetadataViewModel> logger)
        {
            _imageModel = imageModel;
            _logger = logger;

            _imageModel.PropertyChanged += imageModel_PropertyChanged;
        }

        private void imageModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ImageModel.RawMetadata):
                    OnPropertyChanged(nameof(RawMetadata));
                    break;
                case nameof(ImageModel.IsFileLoaded):
                    OnPropertyChanged(nameof(IsFileLoaded));
                    break;
            }
        }

        [RelayCommand]
        private void Clear()
        {
            RawMetadata = "";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _imageModel.PropertyChanged -= imageModel_PropertyChanged;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
