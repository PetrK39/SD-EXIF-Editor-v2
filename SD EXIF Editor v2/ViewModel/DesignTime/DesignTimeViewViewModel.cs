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

        public DesignTimeViewViewModel()
        {
            Metadata = new SDMetadata
            {
                Prompt = "test prompt",
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

        }

        [RelayCommand]
        public void Copy(string parameter)
        {
            throw new NotImplementedException();
        }
    }
}