using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using System.Collections.ObjectModel;

namespace SD_EXIF_Editor_v2.ViewModel.DesignTime
{
    public partial class DesignTimeViewViewModel : ObservableObject, IViewViewModel
    {
        public SDMetadata Metadata { get; }
        public bool IsCivitBusy { get; }
        public ObservableCollection<CivitItem> CivitItems { get; }

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

            CivitItems = new ObservableCollection<CivitItem>
            {
                new CivitItem
                {
                    OriginalName = "Test Original Name 1",
                    PromptName = "Test Prompt Name 1",
                    OriginalVersion = "v0.1",
                    Strength = null,
                    Type = "TEST TYPE 1"
                },
                new CivitItem
                {
                    OriginalName = "Test Original Name 2",
                    PromptName = "Test Prompt Name 2",
                    OriginalVersion = "v0.2",
                    Strength = 1.23f,
                    Type = "TEST TYPE 2"
                },
                new CivitItem
                {
                    IsUnknown = true,
                    PromptName = "Test Prompt Name 3",
                    Strength = -4.56f
                }
            };
        }

        [RelayCommand]
        public void Copy(string parameter)
        {
            throw new NotImplementedException();
        }
        [RelayCommand]
        public void OpenUri(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
