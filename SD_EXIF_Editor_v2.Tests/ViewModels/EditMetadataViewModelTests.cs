using Microsoft.Extensions.Logging;
using Moq;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.ViewModels
{
    public class EditMetadataViewModelTests
    {
        private readonly ImageModel _imageModel;
        private readonly Mock<ILogger<EditMetadataViewModel>> _mockLogger;

        public EditMetadataViewModelTests()
        {
            _imageModel = new ImageModel();
            _mockLogger = new Mock<ILogger<EditMetadataViewModel>>();
        }

        [Fact]
        public void RawMetadata_ShouldUpdateWhenImageModelRawMetadataChanges()
        {
            // Arrange
            var initialRawMetadata = "initial metadata";
            var updatedRawMetadata = "updated metadata";
            _imageModel.RawMetadata = initialRawMetadata;
            var viewModel = new EditMetadataViewModel(_imageModel, _mockLogger.Object);

            // Act
            _imageModel.RawMetadata = updatedRawMetadata;

            // Assert
            Assert.Equal(updatedRawMetadata, viewModel.RawMetadata);
        }

        [Fact]
        public void IsFileLoaded_ShouldUpdateWhenImageModelIsFileLoadedChanges()
        {
            // Arrange
            var initialIsFileLoaded = false;
            var updatedIsFileLoaded = true;
            _imageModel.IsFileLoaded = initialIsFileLoaded;
            var viewModel = new EditMetadataViewModel(_imageModel, _mockLogger.Object);

            // Act
            _imageModel.IsFileLoaded = updatedIsFileLoaded;

            // Assert
            Assert.Equal(updatedIsFileLoaded, viewModel.IsFileLoaded);
        }

        [Fact]
        public void ClearCommand_ShouldClearRawMetadata()
        {
            // Arrange
            var initialRawMetadata = "initial metadata";
            _imageModel.RawMetadata = initialRawMetadata;
            var viewModel = new EditMetadataViewModel(_imageModel, _mockLogger.Object);

            // Act
            viewModel.ClearCommand.Execute(null);

            // Assert
            Assert.Equal("", viewModel.RawMetadata);
        }
    }
}
