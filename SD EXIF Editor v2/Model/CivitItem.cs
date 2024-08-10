using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Service;

namespace SD_EXIF_Editor_v2.Model
{
    public class CivitItem
    {
        public bool IsUnknown { get; init; }

        public string PromptName { get; init; }
        public float? Strength { get; init; }

        public string OriginalName { get; init; }
        public string OriginalVersion { get; init; }
        public string Type { get; init; }

        public double SizeKB { get; init; }

        public List<CivitItemImage> Images { get; init; }
        public string DownloadUri { get; init; }
        public string SiteUri { get; init; }
    }
    public partial class CivitItemImage : ObservableObject
    {
        public string Uri { get; init; }
        public NSFWLevels NSFWLevel { get; init; }
        [ObservableProperty]
        public bool isCurrent = false;
    }
}
