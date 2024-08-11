using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Windows.Input;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class EditViewModel : ObservableObject, IEditViewModel
    {
        private readonly Image image;
        private readonly MessageService _messageService;
        private readonly ILoggingService _loggingService;

        public string RawMetadata { get => image.RawMetadata; set => image.RawMetadata = value; }

        public EditViewModel(Image image, MessageService messageService, ILoggingService loggingService)
        {
            this.image = image;
            _messageService = messageService;
            _loggingService = loggingService;

            _loggingService.Trace("EditViewModel initialized.");
        }

        [RelayCommand]
        public void Save()
        {
            _loggingService.Trace("Entering Save method.");

            try
            {
                image.SaveChanges();
                _loggingService.Info("Image changes saved successfully.");
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Failed to save image changes: {ex.Message}", ex);
            }

            ApplicationCommands.Close.Execute(null, null);

            _loggingService.Trace("Exiting Save method.");
        }

        [RelayCommand]
        public void Delete()
        {
            _loggingService.Trace("Entering Delete method.");

            try
            {
                if (_messageService.ShowConfirmationMessage("Are you sure you want to remove the generation metadata from this file?"))
                {
                    RawMetadata = "";
                    Save();
                    _loggingService.Info("Generation metadata deleted and changes saved successfully.");
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Failed to delete generation metadata: {ex.Message}", ex);
            }

            _loggingService.Trace("Exiting Delete method.");
        }
    }
}
