﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using System.Collections.ObjectModel;

namespace SD_EXIF_Editor_v2.ViewModel.DesignTime
{
    public partial class DesignTimeViewViewModel : ObservableObject, IViewViewModel
    {
        public string RawMetadata { get; }
        public SDMetadata Metadata { get; }
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
                CFGScale = 1.2f,
                Sampler = "Test Sampler",
                ScheduleType = "Test Schedule",
                Seed = 123456,
                Size = new System.Drawing.Size(768, 1024),
                Steps = 20,
                Version = "v0.0.0"
            };

            IsCivitBusy = true;

            ShouldDisplayPromptHeader = true;
            ShouldDisplayNegativePromptHeader = false;
            ShouldDisplaySeparator = true;
            ShouldDisplayPlaceholder = true;
            ShouldDisplayNegativePlaceholder = true;

            CivitItemViewModels = [];
            CivitItemViewModels.Add(new CivitItemViewModel(new CivitItem("test", 1.5f), new Service.SettingsService()));
        }

        [RelayCommand]
        public void Copy(string parameter)
        {
            throw new NotImplementedException();
        }
    }
}
