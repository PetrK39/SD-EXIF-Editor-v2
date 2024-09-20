using CommunityToolkit.Mvvm.ComponentModel;

namespace SD_EXIF_Editor_v2.Models
{
    public partial class WindowOrientationModel : ObservableObject
    {
        [ObservableProperty]
        private bool isHorizontal;
    }
}
