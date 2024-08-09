using SD_EXIF_Editor_v2.Model;
using System.Collections.ObjectModel;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public interface IViewViewModel
    {
        public string RawMetadata { get; }
        public SDMetadata Metadata { get; }
        public bool IsCivitBusy { get; }
        public ObservableCollection<CivitItem> CivitItems { get; }

        public void Copy(string parameter);
        public void OpenUri(string uri);
    }
}
