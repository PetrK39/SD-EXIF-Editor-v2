using SD_EXIF_Editor_v2.ViewModels.Interfaces;
using System.Collections.ObjectModel;

namespace SD_EXIF_Editor_v2.ViewModels.Interfaces
{
    public interface IViewViewModel
    {
        public string RawMetadata { get; }

        public string? Prompt { get; }
        public string? NegativePrompt { get; }

        public bool IsCivitBusy { get; }
        public ObservableCollection<ICivitItemViewModel> CivitItemViewModels { get; }

        public void CopyToClipboard(string parameter);
    }
}
