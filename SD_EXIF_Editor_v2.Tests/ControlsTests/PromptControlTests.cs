using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Moq;
using SD_EXIF_Editor_v2.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SD_EXIF_Editor_v2.Tests;

namespace SD_EXIF_Editor_v2.Tests.ControlsTests
{
    public class PromptControlTests
    {
        [AvaloniaFact]
        public void ShouldDisplayControl_WhenPromptIsNotEmpty()
        {
            // Arrange
            var control = new PromptControl();

            // Act
            control.Prompt = "Test Prompt";

            // Assert
            Assert.True(control.ShouldDisplayControl);
        }

        [AvaloniaFact]
        public void ShouldNotDisplayControl_WhenPromptIsEmptyAndDisplayPlaceholderIsFalse()
        {
            // Arrange
            var control = new PromptControl
            {
                Prompt = string.Empty,
                DisplayPlaceholder = false
            };

            // Assert
            Assert.False(control.ShouldDisplayControl);
        }

        [AvaloniaFact]
        public void ShouldDisplayPlaceholder_WhenPromptIsEmptyAndDisplayPlaceholderIsTrue()
        {
            // Arrange
            var control = new PromptControl
            {
                Prompt = string.Empty,
                DisplayPlaceholder = true
            };

            // Assert
            Assert.True(control.ShouldDisplayPlaceholder);
        }

        [AvaloniaFact]
        public void ShouldNotDisplayPlaceholder_WhenPromptIsNotEmpty()
        {
            // Arrange
            var control = new PromptControl
            {
                Prompt = "Test Prompt",
                DisplayPlaceholder = true
            };

            // Assert
            Assert.False(control.ShouldDisplayPlaceholder);
        }

        [AvaloniaFact]
        public void ShouldDisplayShowMoreLessButton_WhenTextExceedsMaximumLines()
        {
            // Arrange
            var control = new PromptControl
            {
                Prompt = "This\r\nis\r\na\r\nlong\r\nprompt",
                MaximumLines = 1
            };

            // Assert
            Assert.True(control.ShouldDisplayShowMoreLessButton);
        }

        [AvaloniaFact]
        public async Task ShouldNotDisplayShowMoreLessButton_WhenTextDoesNotExceedMaximumLines()
        {
            // Arrange
            var control = new PromptControl
            {
                Prompt = "Short prompt",
                MaximumLines = 1
            };
            var window = new Window { Content = control };
            await TestUtils.MeasureAndArrangeControl(control);

            // Assert
            Assert.False(control.ShouldDisplayShowMoreLessButton);
        }

        [AvaloniaFact]
        public async Task ShowMoreLessButton_Click_ShouldToggleTextBlockMaxLines()
        {
            // Arrange
            var control = new PromptControl
            {
                Prompt = "This\r\nis\r\na\r\nlong\r\nprompt",
                MaximumLines = 1
            };
            var window = new Window { Content = control };
            await TestUtils.MeasureAndArrangeControl(control);

            // Act
            var button = control.GetLogicalDescendants().OfType<Button>().FirstOrDefault(b => b.Content == "Show more");
            button?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            // Assert
            Assert.Equal(int.MaxValue, control.PromptTextBlock.MaxLines);

            // Act
            button?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            // Assert
            Assert.Equal(1, control.PromptTextBlock.MaxLines);
        }
    }
}
