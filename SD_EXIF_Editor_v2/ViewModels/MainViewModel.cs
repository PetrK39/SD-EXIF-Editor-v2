using CommunityToolkit.Mvvm.ComponentModel;

namespace SD_EXIF_Editor_v2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Welcome to Avalonia!";
    }
}
