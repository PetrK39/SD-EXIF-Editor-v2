using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using System.Collections.ObjectModel;

namespace SD_EXIF_Editor_v2.ViewModel.DesignTime
{
    public partial class DesignTimeViewViewModel : ObservableObject, IViewViewModel
    {
        public string RawMetadata { get; }
        public SDMetadata Metadata { get; }
        public string Prompt { get; }
        public string NegativePrompt { get; }
        public bool IsCivitBusy { get; }
        public ObservableCollection<CivitItemViewModel> CivitItemViewModels { get; }
        public bool ShouldDisplayPromptHeader { get; }
        public bool ShouldDisplayNegativePromptHeader { get; }
        public bool ShouldDisplaySeparator { get; }
        public bool ShouldDisplayPlaceholder { get; }
        public bool ShouldDisplayNegativePlaceholder { get; }

        public DesignTimeViewViewModel()
        {
            RawMetadata = "test raw metadata";

            Metadata = new SDMetadata
            {
                Prompt = "",
                NegativePrompt = "test negative prompt",
            };

            Prompt = Metadata.Prompt;
            NegativePrompt = Metadata.NegativePrompt;

            IsCivitBusy = true;

            ShouldDisplayPromptHeader = true;
            ShouldDisplayNegativePromptHeader = false;
            ShouldDisplaySeparator = true;
            ShouldDisplayPlaceholder = true;
            ShouldDisplayNegativePlaceholder = true;

            CivitItemViewModels = [];
        }

        [RelayCommand]
        public void Copy(string parameter)
        {
            throw new NotImplementedException();
        }
    }
}
