using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SD_EXIF_Editor_v2.ViewModel.DesignTime
{
    public partial class DesignTimeEditViewModel : ObservableObject, IEditViewModel
    {
        public string RawMetadata { get; set; }

        public DesignTimeEditViewModel()
        {
            RawMetadata = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam tempor lorem et molestie imperdiet. Vestibulum eget sodales mi. Suspendisse ac efficitur dui. Sed a egestas nunc. In vitae tortor tempor, posuere lacus sit amet, efficitur sapien. Aenean mollis est nec libero ultrices, vitae placerat dolor ullamcorper. Curabitur sit amet finibus lectus. Sed sit amet odio in eros efficitur rhoncus. Praesent luctus nisi venenatis dapibus tincidunt.";
        }
        [RelayCommand]
        public void Delete()
        {
            throw new NotImplementedException();
        }
        [RelayCommand]
        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
