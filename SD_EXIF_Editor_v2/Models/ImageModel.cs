using CommunityToolkit.Mvvm.ComponentModel;

namespace SD_EXIF_Editor_v2.Model
{
    public partial class ImageModel : ObservableObject
    {
        [ObservableProperty]
        private string? filePath = null;
        [ObservableProperty]
        private string? rawMetadata = null;
        [ObservableProperty]
        private bool isFileLoaded = false;
    }
}
