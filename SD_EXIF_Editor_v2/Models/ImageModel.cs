using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace SD_EXIF_Editor_v2.Model
{
    public partial class ImageModel : ObservableObject
    {
        [ObservableProperty]
        private Uri? fileUri = null;
        [ObservableProperty]
        private string? rawMetadata = null;
        [ObservableProperty]
        private bool isFileLoaded = false;
    }
}
