using CommunityToolkit.Mvvm.Input;

namespace SD_EXIF_Editor_v2.ViewModels.Interfaces
{
    public interface IEditMetadataViewModel
    {
        public string RawMetadata { get; set; }

        public IRelayCommand ClearCommand { get; }
    }
}
