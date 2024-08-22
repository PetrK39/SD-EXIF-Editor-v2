using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class EditViewModel : ObservableObject, IEditViewModel
    {
        private readonly Image _image;
        private readonly IMessageService _messageService;
        private readonly ILogger<EditViewModel> _logger;

        public string RawMetadata { get => _image.RawMetadata; set => _image.RawMetadata = value; }

        public EditViewModel(Image image, IMessageService messageService, ILogger<EditViewModel> logger)
        {
            _image = image;
            _messageService = messageService;
            _logger = logger;

            _image.PropertyChanged += _image_PropertyChanged;

            _logger.LogTrace("EditViewModel initialized.");
        }
        private void _image_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _logger.LogTrace("_image_PropertyChanged event triggered.");

            if (e.PropertyName == nameof(_image.RawMetadata))
            {
                OnPropertyChanged(nameof(RawMetadata));
                _logger.LogDebug($"Current image changed to, updating relevant properties");
            }
        }
        [RelayCommand]
        public void Save()
        {
            _logger.LogTrace("Entering Save method.");

            try
            {
                _image.SaveChanges();
                _logger.LogInformation("Image changes saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save image changes: {ex.Message}", ex);
            }

            ApplicationCommands.Close.Execute(null, null);

            _logger.LogTrace("Exiting Save method.");
        }

        [RelayCommand]
        public void Delete()
        {
            _logger.LogTrace("Entering Delete method.");

            try
            {
                if (_messageService.ShowConfirmationMessage("Are you sure you want to remove the generation metadata from this file?"))
                {
                    RawMetadata = "";
                    Save();
                    _logger.LogInformation("Generation metadata deleted and changes saved successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete generation metadata: {ex.Message}", ex);
            }

            _logger.LogTrace("Exiting Delete method.");
        }
    }
}
