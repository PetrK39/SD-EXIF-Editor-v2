using SD_EXIF_Editor_v2.Model;
using System.Collections.Generic;

namespace SD_EXIF_Editor_v2.ViewModels.Interfaces
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

        public IEnumerable<CivitItemImage> FilteredImages { get; }

        public void OpenUri(string uri);
    }
}
