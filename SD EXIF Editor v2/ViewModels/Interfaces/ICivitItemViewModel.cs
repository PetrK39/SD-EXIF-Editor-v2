using SD_EXIF_Editor_v2.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SD_EXIF_Editor_v2.ViewModel.Interfaces
{
    public interface ICivitItemViewModel
    {
        public bool IsUnknown { get; }

        public string OriginalName { get; }
        public string OriginalVersion { get; }
        public string Type { get; }
        public double SizeKB { get; }

        public string PromptName { get; }
        public float? Strength { get; }

        public string SiteUri { get; }
        public string DownloadUri { get; }

        public ObservableCollection<CivitItemImage> Images { get; }
        public ICollectionView FilteredImages { get; }
        public bool IsHaveStrength { get; }
        public bool IsNotEmpty { get; } 

        public void OpenUri(string uri);
        public void NextImage();
        public void PrevImage();
        public void GoToImage(CivitItemImage image);
    }
}
