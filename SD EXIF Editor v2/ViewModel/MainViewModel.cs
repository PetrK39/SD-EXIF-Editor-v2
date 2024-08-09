using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using System.IO;
using System.Windows.Media.Imaging;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class MainViewModel : ObservableObject, IMainViewModel
    {
        private readonly IViewViewModel _viewViewModel;
        private readonly IEditViewModel _editViewModel;
        private readonly ISettingsViewModel _settingsViewModel;

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
            SettingsViewModel settingsViewModel)
        {
            _image = image;

            _viewViewModel = viewViewModel;
            _editViewModel = editViewModel;
            _settingsViewModel = settingsViewModel;
        }

        #region Image Loading
        private async void getImageAsync()
        {
            image_retrieved = true;
            bitmapImage = await CreateImageAsync(FilePath).ConfigureAwait(true);
            OnPropertyChanged(nameof(bitmapImage));
        }
        private async Task<BitmapImage?> CreateImageAsync(string filename)
        {
            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                try
                {
                    byte[] buffer = await ReadAllFileAsync(filename).ConfigureAwait(false);
                    MemoryStream ms = new MemoryStream(buffer);
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    image.Freeze();
                    return image;
                }
                catch
                {
                    return null;
                }
            }
            else return null;
        }
        private async Task<byte[]> ReadAllFileAsync(string filename)
        {
            try
            {
                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    byte[] buff = new byte[file.Length];
                    await file.ReadAsync(buff, 0, (int)file.Length).ConfigureAwait(false);
                    return buff;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
