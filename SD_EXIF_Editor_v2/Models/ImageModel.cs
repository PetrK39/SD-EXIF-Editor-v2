using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Memento;
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

        public Memento.StringMemento SaveMemento() => new(RawMetadata);
        public void LoadMemento(Memento.StringMemento memento)
        {
            rawMetadata = memento.State;
            OnPropertyChanged(nameof(RawMetadata));
        }
    }
}
