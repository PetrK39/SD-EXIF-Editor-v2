using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using SD_EXIF_Editor_v2.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.ControlsTests
{
    public class ButtonWithTempContentTests
    {
        private const string _originalContent = "Original content";
        private const string _temporaryContent = "Temporary content";

        [AvaloniaFact]
        public async Task Click_ShouldChangeContentToTempContentAndRestoreAfterDuration()
        {
            // Arrange
            var button = new ButtonWithTempContent
            {
                Content = _originalContent,
                TempContent = _temporaryContent,
                TempDuration = 1
            };

            // Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            // Assert
            Assert.Equal(_temporaryContent, button.Content);

            // Wait for the timer to tick (simulating the duration)
            await Task.Delay(TimeSpan.FromSeconds(1.5));

            // Assert
            Assert.Equal(_originalContent, button.Content);
        }

        [AvaloniaFact]
        public void Click_ShouldNotRestoreContentIfNotClickedAgain()
        {
            // Arrange
            var button = new ButtonWithTempContent
            {
                Content = _originalContent,
                TempContent = _temporaryContent,
                TempDuration = 1
            };

            // Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            // Assert
            Assert.Equal(_temporaryContent, button.Content);

            // Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            // Assert
            Assert.Equal(_temporaryContent, button.Content);
        }

        [AvaloniaFact]
        public async Task Click_ShouldRestoreContentAfterMultipleClicks()
        {
            // Arrange
            var button = new ButtonWithTempContent
            {
                Content = _originalContent,
                TempContent = _temporaryContent,
                TempDuration = 1
            };

            // Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            // Assert
            Assert.Equal(_temporaryContent, button.Content);

            // Wait for the timer to tick (simulating the duration)
            await Task.Delay(TimeSpan.FromSeconds(1.5));

            // Assert
            Assert.Equal(_originalContent, button.Content);
        }
    }
}
