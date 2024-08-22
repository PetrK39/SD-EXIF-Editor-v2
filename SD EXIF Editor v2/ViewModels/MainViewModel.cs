using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<MainViewModel> _logger;
        private readonly IArgsParserService _argsParserService;

        private readonly Image _image;

        private bool image_retrieved;
        private BitmapImage? bitmapImage;

        public string FilePath => _image.FilePath!;
        public BitmapImage? BitmapImage
        {
            get
            {
                if ((bitmapImage is null || !image_retrieved) && FilePath is not null) getImageAsync();
                return bitmapImage;
            }
        }

        public IViewViewModel ViewViewModel => _viewViewModel;
        public IEditViewModel EditViewModel => _editViewModel;
        public ISettingsViewModel SettingsViewModel => _settingsViewModel;

        public MainViewModel(Image image,
            IViewViewModel viewViewModel,
            IEditViewModel editViewModel,
            ISettingsViewModel settingsViewModel,
            ILogger<MainViewModel> logger,
            IArgsParserService argsParserService)
        {
            _image = image;

            _viewViewModel = viewViewModel;
            _editViewModel = editViewModel;
            _settingsViewModel = settingsViewModel;

            _logger = logger;
            _argsParserService = argsParserService;

            _image.PropertyChanged += _image_PropertyChanged;

            _logger.LogTrace("MainViewModel initialized.");
        }

        private void _image_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _logger.LogTrace("_image_PropertyChanged event triggered.");

            if (e.PropertyName == nameof(_image.FilePath))
            {
                OnPropertyChanged(nameof(FilePath));
                OnPropertyChanged(nameof(BitmapImage));
                _logger.LogDebug($"Current image changed, updating relevant properties");
            }
        }

        [RelayCommand]
        public void LoadImage(string? filePath = null)
        {
            if (filePath is null)
            {
                var file = _argsParserService.ParseArgs(Environment.GetCommandLineArgs());
                _image.LoadFromFilePath(file.FullName);
            }
            else
                _image.LoadFromFilePath(filePath);
        }
        #region Image Loading
        private async void getImageAsync()
        {
            _logger.LogTrace("Entering getImageAsync method.");

            try
            {
                bitmapImage = await CreateImageAsync(FilePath);

                image_retrieved = true;
                OnPropertyChanged(nameof(BitmapImage));
                _logger.LogInformation("Image retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve image: {ex.Message}", ex);
            }

            _logger.LogTrace("Exiting getImageAsync method.");
        }

        private async Task<BitmapImage?> CreateImageAsync(string filename)
        {
            _logger.LogTrace("Entering CreateImageAsync method.");

            if (FilePath is null)
                throw new ArgumentNullException(filename);

            _logger.LogDebug($"Creating image from file: {filename}");

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
                    _logger.LogInformation("Image created successfully.");
                    return image;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to create image: {ex.Message}", ex);
                    return null;
                }
            }
            else
            {
                _logger.LogWarning("File does not exist or filename is null or empty.");
                return null;
            }
        }

        private async Task<byte[]> ReadAllFileAsync(string filename)
        {
            _logger.LogTrace("Entering ReadAllFileAsync method.");
            _logger.LogDebug($"Reading file: {filename}");

            try
            {
                using var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                byte[] buff = new byte[file.Length];
                await file.ReadAsync(buff, 0, (int)file.Length).ConfigureAwait(false);
                _logger.LogInformation("File read successfully.");
                return buff;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to read file: {ex.Message}", ex);
                return [];
            }
        }
        #endregion
    }
}
