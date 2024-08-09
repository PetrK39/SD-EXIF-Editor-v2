using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;

namespace SD_EXIF_Editor_v2.ViewModel.DesignTime
{
    public partial class DesignTimeMainViewModel : ObservableObject, IMainViewModel
    {
        public string FilePath { get; }
        public BitmapImage? BitmapImage { get; }
        public IViewViewModel ViewViewModel { get; }
        public IEditViewModel EditViewModel { get; }
        public ISettingsViewModel SettingsViewModel { get; }

        public DesignTimeMainViewModel()
        {
            FilePath = "C:/test/file/path";

            BitmapImage = null;
            ViewViewModel = new DesignTimeViewViewModel();
            EditViewModel = new DesignTimeEditViewModel();
            SettingsViewModel = new DesignTimeSettingsViewModel();
        }
    }
}
