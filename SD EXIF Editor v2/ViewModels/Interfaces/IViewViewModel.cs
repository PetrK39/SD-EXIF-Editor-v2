using SD_EXIF_Editor_v2.Model;
using System.Collections.ObjectModel;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public interface IViewViewModel
    {
        public string RawMetadata { get; }
        public SDMetadata Metadata { get; }
        public string? Prompt { get; }
        public string? NegativePrompt { get; }
        public bool IsCivitBusy { get; }
        public ObservableCollection<CivitItemViewModel> CivitItemViewModels { get; }

        public bool ShouldDisplayPromptHeader { get; }
        public bool ShouldDisplayNegativePromptHeader { get; }
        public bool ShouldDisplaySeparator { get; }
        public bool ShouldDisplayPlaceholder { get; }
        public bool ShouldDisplayNegativePlaceholder { get; }

        public void Copy(string parameter);
    }
}
