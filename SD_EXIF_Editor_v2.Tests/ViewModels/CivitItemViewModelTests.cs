using Microsoft.Extensions.Logging;
using Moq;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.ViewModels;
using System.ComponentModel;

namespace SD_EXIF_Editor_v2.Tests.ViewModels
{
    public class CivitItemViewModelTests
    {
        private readonly Mock<ISettingsService> _mockSettingsService;
        private readonly Mock<IUrlOpenerService> _mockUrlOpenerService;
        private readonly Mock<ILogger<CivitItemViewModel>> _mockLogger;

        public CivitItemViewModelTests()
        {
            _mockSettingsService = new Mock<ISettingsService>();
            _mockUrlOpenerService = new Mock<IUrlOpenerService>();
            _mockLogger = new Mock<ILogger<CivitItemViewModel>>();
        }

        [Fact]
        public void Initialize_ShouldSetCivitItem()
        {
            // Arrange
            var civitItem = new CivitItem("promptName", -1.23f, "originalName", "originalVersion", "type", 123, new List<CivitItemImage>(), "/download/uri", "site/uri");
            var viewModel = new CivitItemViewModel(_mockSettingsService.Object, _mockUrlOpenerService.Object, _mockLogger.Object);

            // Act
            viewModel.Initialize(civitItem);

            // Assert
            Assert.Equal(civitItem.OriginalName, viewModel.OriginalName);
            Assert.Equal(civitItem.OriginalVersion, viewModel.OriginalVersion);
            Assert.Equal(civitItem.Type, viewModel.Type);
            Assert.Equal(civitItem.SizeKB, viewModel.SizeKB);
            Assert.Equal(civitItem.PromptName, viewModel.PromptName);
            Assert.Equal(civitItem.Strength, viewModel.Strength);
            Assert.Equal(civitItem.SiteUri, viewModel.SiteUri);
            Assert.Equal(civitItem.DownloadUri, viewModel.DownloadUri);
        }

        [Fact]
        public async Task OpenUrlAsync_ShouldCallUrlOpenerService()
        {
            // Arrange
            var uri = "http://example.com";
            _mockUrlOpenerService.Setup(u => u.OpenUrlAsync(uri)).Returns(Task.CompletedTask);
            var viewModel = new CivitItemViewModel(_mockSettingsService.Object, _mockUrlOpenerService.Object, _mockLogger.Object);

            // Act
            await viewModel.OpenUrlAsync(uri);

            // Assert
            _mockUrlOpenerService.Verify(u => u.OpenUrlAsync(uri), Times.Once);
        }

        [Fact]
        public async Task OpenUrlAsync_ShouldLogErrorOnException()
        {
            // Arrange
            var uri = "http://example.com";
            var exception = new Exception("Test exception");
            _mockUrlOpenerService.Setup(u => u.OpenUrlAsync(uri)).ThrowsAsync(exception);
            var viewModel = new CivitItemViewModel(_mockSettingsService.Object, _mockUrlOpenerService.Object, _mockLogger.Object);

            // Act
            await viewModel.OpenUrlAsync(uri);

            // Assert
            _mockLogger.Verify(
                l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public void FilteredImages_ShouldFilterImagesBasedOnNSFWLevel()
        {
            // Arrange
            var nsfwLevel = NSFWLevels.None | NSFWLevels.Soft;
            var images = new List<CivitItemImage>
            {
                new CivitItemImage("", NSFWLevels.None),
                new CivitItemImage ("", NSFWLevels.Soft),
                new CivitItemImage ("", NSFWLevels.Mature)
            };
            var civitItem = new CivitItem("promptName", -1.23f, "originalName", "originalVersion", "type", 123, images, "/download/uri", "site/uri");
            _mockSettingsService.Setup(s => s.NSFWLevel).Returns(nsfwLevel);
            var viewModel = new CivitItemViewModel(_mockSettingsService.Object, _mockUrlOpenerService.Object, _mockLogger.Object);
            viewModel.Initialize(civitItem);

            // Act
            var filteredImages = viewModel.FilteredImages;

            // Assert
            Assert.Equal(2, filteredImages.Count());
            Assert.All(filteredImages, img => Assert.True((img.NSFWLevel & nsfwLevel) == img.NSFWLevel));
        }

        [Fact]
        public void IsFilteredImagesEmpty_ShouldReturnTrueIfNoFilteredImages()
        {
            // Arrange
            var nsfwLevel = NSFWLevels.X | NSFWLevels.XXX;
            var images = new List<CivitItemImage>
            {
                new CivitItemImage("", NSFWLevels.None),
                new CivitItemImage ("", NSFWLevels.Soft),
                new CivitItemImage ("", NSFWLevels.Mature)
            };
            var civitItem = new CivitItem("promptName", -1.23f, "originalName", "originalVersion", "type", 123, images, "/download/uri", "site/uri");
            _mockSettingsService.Setup(s => s.NSFWLevel).Returns(nsfwLevel);
            var viewModel = new CivitItemViewModel(_mockSettingsService.Object, _mockUrlOpenerService.Object, _mockLogger.Object);
            viewModel.Initialize(civitItem);

            // Act
            var isFilteredImagesEmpty = viewModel.IsFilteredImagesEmpty;

            // Assert
            Assert.True(isFilteredImagesEmpty);
        }
        [Fact]
        public void FilteredImages_ShouldUpdateWhenNSFWLevelChanges()
        {
            // Arrange
            var nsfwLevel = NSFWLevels.None;
            var images = new List<CivitItemImage>
            {
                new CivitItemImage("", NSFWLevels.None),
                new CivitItemImage ("", NSFWLevels.Soft),
                new CivitItemImage ("", NSFWLevels.Mature)
            };
            var civitItem = new CivitItem("promptName", -1.23f, "originalName", "originalVersion", "type", 123, images, "/download/uri", "site/uri");
            _mockSettingsService.Setup(s => s.NSFWLevel).Returns(nsfwLevel);
            var viewModel = new CivitItemViewModel(_mockSettingsService.Object, _mockUrlOpenerService.Object, _mockLogger.Object);
            viewModel.Initialize(civitItem);

            // Act
            var initialFilteredImages = viewModel.FilteredImages;

            // Assert
            Assert.Equal(1, initialFilteredImages.Count());

            // Arrange for property change
            var newNsfwLevel = NSFWLevels.Soft | NSFWLevels.Mature;
            _mockSettingsService.Setup(s => s.NSFWLevel).Returns(newNsfwLevel);
            var propertyChangedRaised = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(CivitItemViewModel.FilteredImages))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act: Trigger property change
            _mockSettingsService.Raise(s => s.PropertyChanged += null, new PropertyChangedEventArgs(nameof(ISettingsService.NSFWLevel)));

            // Assert
            Assert.True(propertyChangedRaised);
            var updatedFilteredImages = viewModel.FilteredImages;
            Assert.Equal(2, updatedFilteredImages.Count());
        }
    }
}
