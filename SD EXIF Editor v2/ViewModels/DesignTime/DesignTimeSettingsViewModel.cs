using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.ViewModel.DesignTime
{
    public partial class DesignTimeSettingsViewModel : ObservableObject, ISettingsViewModel
    {
        public NSFWLevels NSFWLevel { get; set; }
        public bool DisplayPlaceholders { get; set; }

        public DesignTimeSettingsViewModel()
        {
            NSFWLevel = NSFWLevels.Soft;
            DisplayPlaceholders = true;
        }
    }
}
