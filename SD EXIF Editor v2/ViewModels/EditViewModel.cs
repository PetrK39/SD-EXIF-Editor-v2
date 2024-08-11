using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using System.Windows.Input;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class EditViewModel : ObservableObject, IEditViewModel
    {
        private readonly Image image;
        private readonly MessageService _messageService;

        public string RawMetadata { get => image.RawMetadata; set => image.RawMetadata = value; }

        public EditViewModel(Image image, MessageService messageService)
        {
            this.image = image;
            _messageService = messageService;
        }

        [RelayCommand]
        public void Save()
        {
            image.SaveChanges();

            ApplicationCommands.Close.Execute(null, null);
        }
        [RelayCommand]
        public void Delete()
        {
            if (_messageService.ShowConfirmationMessage("Are you sure you want to remove the generation metadata from this file?"))
            {
                RawMetadata = "";
                Save();
            }
        }
    }
}
