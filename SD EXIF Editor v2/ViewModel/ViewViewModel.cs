using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using System.Collections.ObjectModel;
using System.Windows;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class ViewViewModel : ObservableObject, IViewViewModel
    {
        private readonly Image _image;

        private readonly MetadataParserService _metadataParserService;
        private readonly CivitService _civitService;

        private bool isCivitBusy = false;

        public string RawMetadata => _image.RawMetadata;
        public SDMetadata Metadata { get; set; }
        public bool IsCivitBusy { get => isCivitBusy; set => SetProperty(ref isCivitBusy, value); }
        public ObservableCollection<CivitItem> CivitItems { get; set; }

        public ViewViewModel(Image image,
            MetadataParserService metadataParserService,
            CivitService civitServide)
        {
            _image = image;

            _metadataParserService = metadataParserService;
            _civitService = civitServide;

            Metadata = _metadataParserService.ParseFromRawMetadata(image.RawMetadata);

            CivitItems = [];

            LoadCivitItems();
        }
        private async Task LoadCivitItems()
        {
            IsCivitBusy = true;

            if (Metadata.Model != null)
                CivitItems.Add(await _civitService.GetItemFromHash(Metadata.Model.Name, Metadata.Model.Hash, null));

            foreach (var lora in Metadata.Loras)
                CivitItems.Add(await _civitService.GetItemFromHash(lora.Name, lora.Hash, lora.Strength));

            IsCivitBusy = false;
        }

        [RelayCommand]
        public void Copy(string parameter)
        {
            Clipboard.SetDataObject(parameter);
        }
        [RelayCommand]
        public void OpenUri(string uri)
        {
            var sInfo = new System.Diagnostics.ProcessStartInfo(uri)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }
    }
}
