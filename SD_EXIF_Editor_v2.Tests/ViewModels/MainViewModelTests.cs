using Avalonia.Headless.XUnit;
using Avalonia.Platform.Storage;
using Moq;
using SD_EXIF_Editor_v2.Messages;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Models;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.ViewModels;

namespace SD_EXIF_Editor_v2.Tests.ViewModels
{
    public class MainViewModelTests
    {
        private readonly Mock<ImageModel> _mockImageModel;
        private readonly Mock<WindowOrientationModel> _mockWindowOrientationModel;
        private readonly Mock<IFileService> _mockFileService;
        private readonly Mock<IStartupFileService> _mockStartupFileService;
        private readonly Mock<ISettingsService> _mockSettingsService;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<IServiceProvider> _mockServiceProvider;

        public MainViewModelTests()
        {
            _mockImageModel = new Mock<ImageModel>();
            _mockWindowOrientationModel = new Mock<WindowOrientationModel>();
            _mockFileService = new Mock<IFileService>();
            _mockStartupFileService = new Mock<IStartupFileService>();
            _mockSettingsService = new Mock<ISettingsService>();
            _mockMessageService = new Mock<IMessageService>();
            _mockServiceProvider = new Mock<IServiceProvider>();
        }

        [AvaloniaFact]
        public void IsDarkTheme_ShouldUpdateSettingsServiceAndThemeVariant()
        {
            // Arrange
            var initialIsDarkTheme = false;
            var updatedIsDarkTheme = true;
            _mockSettingsService.SetupProperty(s => s.IsDarkTheme, initialIsDarkTheme);
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);

            // Act
            viewModel.IsDarkTheme = updatedIsDarkTheme;

            // Assert
            Assert.Equal(updatedIsDarkTheme, viewModel.IsDarkTheme);
            _mockSettingsService.Verify(s => s.Save(), Times.Once);
            Assert.Equal(Avalonia.Styling.ThemeVariant.Dark, App.Current.RequestedThemeVariant);
        }

        [AvaloniaFact]
        public void Receive_WindowLoadedMessage_ShouldInitializeStartupFile()
        {
            // Arrange
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);
            var message = new WindowLoadedMessage(true);

            // Act
            viewModel.Receive(message);

            // Assert
            _mockStartupFileService.Verify(s => s.GetStartupFileAsync(), Times.Once);
        }

        [AvaloniaFact]
        public void Receive_WindowSizeChangedMessage_ShouldUpdateWindowOrientationModel()
        {
            // Arrange
            var isHorizontal = true;
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);
            var message = new WindowSizeChangedMessage(isHorizontal);

            // Act
            viewModel.Receive(message);

            // Assert
            Assert.Equal(isHorizontal, _mockWindowOrientationModel.Object.IsHorizontal);
        }

        [AvaloniaFact]
        public async Task Receive_DragDropOpenFileMessage_ShouldLoadFileIntoModel()
        {
            // Arrange
            var fileUri = new Uri("file:///test.jpg");
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);
            var message = new DragDropOpenFileMessage(fileUri);

            // Act
            viewModel.Receive(message);

            // Assert
            _mockFileService.Verify(f => f.LoadFileIntoModelAsync(_mockImageModel.Object, fileUri), Times.Once);
        }

        [AvaloniaFact]
        public async Task Receive_ClosingConfirmationMessage_ShouldHandleConfirmation()
        {
            // Arrange
            var hasChanged = true;
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);
            var message = new ClosingConfirmationMessage();

            _mockMessageService.Setup(m => m.ShowExitConfirmationDialogAsync()).ReturnsAsync(true);

            // Act
            viewModel.Receive(message);

            // Assert
            _mockFileService.Verify(f => f.SaveFileFromModelAsync(_mockImageModel.Object), Times.Once);
            Assert.True(await message.ResponseCompletionSource.Task);
        }

        [AvaloniaFact]
        public void SelectedListItemChanged_ShouldUpdateCurrentPage()
        {
            // Arrange
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);
            var listItemTemplate = new ListItemTemplate(typeof(EditMetadataViewModel), "test", "Test");

            // Act
            viewModel.SelectedListItem = listItemTemplate;

            // Assert
            Assert.IsType<ViewMetadataViewModel>(viewModel.CurrentPage);
        }

        [AvaloniaFact]
        public void TriggerPane_ShouldToggleIsPaneOpen()
        {
            // Arrange
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);

            // Act
            viewModel.TriggerPaneCommand.Execute(null);

            // Assert
            Assert.True(viewModel.IsPaneOpen);

            // Act again to toggle back
            viewModel.TriggerPaneCommand.Execute(null);

            // Assert
            Assert.False(viewModel.IsPaneOpen);
        }

        [AvaloniaFact]
        public async Task OpenAsync_ShouldLoadFileIntoModel()
        {
            // Arrange
            var fileUri = new Uri("file:///test.jpg");
            _mockFileService.Setup(f => f.PickFileToLoad()).ReturnsAsync(fileUri);
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);

            // Act
            await viewModel.OpenCommand.ExecuteAsync(null);

            // Assert
            _mockFileService.Verify(f => f.LoadFileIntoModelAsync(_mockImageModel.Object, fileUri), Times.Once);
        }

        [AvaloniaFact]
        public void CloseCommand_ShouldCloseFileFromModel()
        {
            // Arrange
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);

            // Act
            viewModel.CloseCommand.Execute(null);

            // Assert
            _mockFileService.Verify(f => f.CloseFileFromModel(_mockImageModel.Object), Times.Once);
        }

        [       AvaloniaFact]
        public async Task SaveAsync_ShouldSaveFileFromModel()
        {
            // Arrange
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);

            // Act
            await viewModel.SaveCommand.ExecuteAsync(null);

            // Assert
            _mockFileService.Verify(f => f.SaveFileFromModelAsync(_mockImageModel.Object), Times.Once);
        }

        [AvaloniaFact]
        public async Task SaveAsAsync_ShouldSaveFileAsFromModel()
        {
            // Arrange
            var mockStorageFile = new Mock<IStorageFile>();
            _mockFileService.Setup(f => f.PickFileToSave()).ReturnsAsync(mockStorageFile.Object);
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);

            // Act
            await viewModel.SaveAsCommand.ExecuteAsync(null);

            // Assert
            _mockFileService.Verify(f => f.SaveFileAsFromModelAsync(_mockImageModel.Object, mockStorageFile.Object), Times.Once);
        }

        [AvaloniaFact]
        public void ToggleThemeVariant_ShouldToggleIsDarkTheme()
        {
            // Arrange
            var initialIsDarkTheme = false;
            _mockSettingsService.SetupProperty(s => s.IsDarkTheme, initialIsDarkTheme);
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);

            // Act
            viewModel.ToggleThemeVariantCommand.Execute(null);

            // Assert
            Assert.True(viewModel.IsDarkTheme);
            _mockSettingsService.Verify(s => s.Save(), Times.Once);
            Assert.Equal(Avalonia.Styling.ThemeVariant.Dark, App.Current.RequestedThemeVariant);
        }

        [AvaloniaFact]
        public async Task OpenAboutCommand_ShouldShowAboutDialog()
        {
            // Arrange
            var viewModel = new MainViewModel(_mockImageModel.Object, _mockWindowOrientationModel.Object, _mockFileService.Object, _mockStartupFileService.Object, _mockSettingsService.Object, _mockMessageService.Object, _mockServiceProvider.Object);

            // Act
            await viewModel.OpenAboutCommand.ExecuteAsync(null);

            // Assert
            _mockMessageService.Verify(m => m.ShowAboutDialogAsync(), Times.Once);
        }
    }
}
