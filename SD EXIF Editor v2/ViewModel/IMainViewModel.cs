using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        public string FilePath { get; }
        public BitmapImage? BitmapImage { get; }

        public IViewViewModel ViewViewModel { get; }
        public IEditViewModel EditViewModel { get; }
        public ISettingsViewModel SettingsViewModel { get; }
    }
}
