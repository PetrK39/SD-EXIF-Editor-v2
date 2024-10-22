﻿using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;

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

        public CivitItem(string promptName, 
            float? strength, 
            string originalName, 
            string originalVersion, 
            string type, 
            double sizeKB, 
            List<CivitItemImage> images, 
            string downloadUri, 
            string siteUri)
        {
            IsUnknown = false;
            PromptName = promptName;
            Strength = strength;
            OriginalName = originalName;
            OriginalVersion = originalVersion;
            Type = type;
            SizeKB = sizeKB;
            Images = images;
            DownloadUri = downloadUri;
            SiteUri = siteUri;
        }
        public CivitItem(string promptName, float? strength)
        {
            IsUnknown = true;
            PromptName = promptName;
            Strength = strength;

            OriginalName = "";
            OriginalVersion = "";
            Type = "";
            SizeKB = 0d;
            Images = [];
            DownloadUri = "";
            SiteUri = "";
        }
    }
    public partial class CivitItemImage : ObservableObject
    {
        public string Uri { get; init; }
        public NSFWLevels NSFWLevel { get; init; }
        [ObservableProperty]
        public bool isCurrent = false;

        public CivitItemImage(string uri, NSFWLevels nSFWLevel)
        {
            Uri = uri;
            NSFWLevel = nSFWLevel;
        }
    }
}
