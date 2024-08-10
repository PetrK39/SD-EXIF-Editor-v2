using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Model
{
    public class CivitItem
    {
        public bool IsUnknown { get; set; }

        public string PromptName { get; set; }
        public float? Strength { get; set; }

        public string OriginalName { get; set; }
        public string OriginalVersion { get; set; }
        public string Type { get; set; }

        public double SizeKB { get; set; }

        public List<CivitItemImage> Images { get; set; }
        public string DownloadUri { get; set; }
        public string SiteUri { get; set; }
    }
    public partial class CivitItemImage : ObservableObject
    {
        public string Uri { get; set; }
        public NSFWLevels NSFWLevel { get; set; }
        [ObservableProperty]
        public bool isCurrent = false;
    }
}
