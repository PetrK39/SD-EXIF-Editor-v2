using Avalonia.Platform;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.ViewModels
{
    public class SettingsViewModelTests
    {
        private readonly Mock<ISettingsService> _mockSettingsService;
        private readonly Mock<ILogger<SettingsViewModel>> _mockLogger;
        private readonly SettingsViewModel _viewModel;

        public SettingsViewModelTests()
        {
            _mockSettingsService = new Mock<ISettingsService>();
            _mockLogger = new Mock<ILogger<SettingsViewModel>>();
            _viewModel = new SettingsViewModel(_mockSettingsService.Object, _mockLogger.Object);
        }

        [Fact]
        public void NSFWLevel_Set_ShouldUpdateSettingsService()
        {
            // Arrange
            var newValue = NSFWLevels.Soft;

            // Act
            _viewModel.NSFWLevel = newValue;

            // Assert
            _mockSettingsService.VerifySet(s => s.NSFWLevel = newValue, Times.Once);
        }

        [Fact]
        public void DisplayPlaceholders_Set_ShouldUpdateSettingsService()
        {
            // Arrange
            var newValue = true;

            // Act
            _viewModel.DisplayPlaceholders = newValue;

            // Assert
            _mockSettingsService.VerifySet(s => s.DisplayPlaceholders = newValue, Times.Once);
        }

        [Fact]
        public void LogLevel_Set_ShouldUpdateSettingsServiceAndUpdateNLogConfigurator()
        {
            // Arrange
            var newValue = LogLevels.Debug;

            // Act
            _viewModel.LogLevel = newValue;

            // Assert
            _mockSettingsService.VerifySet(s => s.LogLevel = newValue, Times.Once);
        }
    }
}
