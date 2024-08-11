using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.IO;
using System.Windows.Media.Imaging;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class MainViewModel : ObservableObject, IMainViewModel
    {
        private readonly IViewViewModel _viewViewModel;
        private readonly IEditViewModel _editViewModel;
        private readonly ISettingsViewModel _settingsViewModel;
        private readonly ILoggingService _loggingService;

        private readonly Image _image;

        private bool image_retrieved;
        private BitmapImage? bitmapImage;

        public string FilePath => _image.FilePath!;
        public BitmapImage? BitmapImage
        {
            get
            {
                if (!image_retrieved) getImageAsync();
                return bitmapImage;
            }
        }

        public IViewViewModel ViewViewModel => _viewViewModel;
        public IEditViewModel EditViewModel => _editViewModel;
        public ISettingsViewModel SettingsViewModel => _settingsViewModel;

        public MainViewModel(Image image,
            ViewViewModel viewViewModel,
            EditViewModel editViewModel,
            SettingsViewModel settingsViewModel,
            ILoggingService loggingService)
        {
            _image = image;
            _viewViewModel = viewViewModel;
            _editViewModel = editViewModel;
            _settingsViewModel = settingsViewModel;
            _loggingService = loggingService;

            _loggingService.Trace("MainViewModel initialized.");
        }

        #region Image Loading
        private async void getImageAsync()
        {
            _loggingService.Trace("Entering getImageAsync method.");

            try
            {
                image_retrieved = true;
                bitmapImage = await CreateImageAsync(FilePath).ConfigureAwait(true);
                OnPropertyChanged(nameof(bitmapImage));
                _loggingService.Info("Image retrieved successfully.");
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Failed to retrieve image: {ex.Message}", ex);
            }

            _loggingService.Trace("Exiting getImageAsync method.");
        }

        private async Task<BitmapImage?> CreateImageAsync(string filename)
        {
            _loggingService.Trace("Entering CreateImageAsync method.");
            _loggingService.Debug($"Creating image from file: {filename}");

            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                try
                {
                    byte[] buffer = await ReadAllFileAsync(filename).ConfigureAwait(false);
                    MemoryStream ms = new(buffer);
                    BitmapImage image = new();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    image.Freeze();
                    _loggingService.Info("Image created successfully.");
                    return image;
                }
                catch (Exception ex)
                {
                    _loggingService.Error($"Failed to create image: {ex.Message}", ex);
                    return null;
                }
            }
            else
            {
                _loggingService.Warn("File does not exist or filename is null or empty.");
                return null;
            }
        }

        private async Task<byte[]> ReadAllFileAsync(string filename)
        {
            _loggingService.Trace("Entering ReadAllFileAsync method.");
            _loggingService.Debug($"Reading file: {filename}");

            try
            {
                using var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                byte[] buff = new byte[file.Length];
                await file.ReadAsync(buff, 0, (int)file.Length).ConfigureAwait(false);
                _loggingService.Info("File read successfully.");
                return buff;
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Failed to read file: {ex.Message}", ex);
                return [];
            }
        }
        #endregion
    }
}
