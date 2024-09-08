using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SD_EXIF_Editor_v2.ViewModels.Interfaces
{
    public interface IViewMetadataViewModel
    {
        public bool IsFileLoaded { get; }
        public Uri FileUri { get; }

        public string RawMetadata { get; }

        public string? Prompt { get; }
        public string? NegativePrompt { get; }
        public IReadOnlyDictionary<string, string> MetadataProperties { get; }

        public bool IsCivitBusy { get; }
        public ObservableCollection<ICivitItemViewModel> CivitItemViewModels { get; }

        public bool ShouldDisplayPlaceholders { get; }
        public int MaximumLines { get; }

        public IAsyncRelayCommand<string> CopyToClipboardCommand { get; }
    }
}
