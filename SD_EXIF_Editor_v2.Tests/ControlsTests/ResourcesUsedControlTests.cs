using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using SD_EXIF_Editor_v2.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.ControlsTests
{
    public class ResourcesUsedControlTests
    {
        [AvaloniaFact]
        public void ShouldDisplayPlaceholder_WhenItemsSourceIsEmpty()
        {
            // Arrange
            var control = new ResourcesUsedControl
            {
                ItemsSource = new List<string>()
            };

            // Assert
            Assert.True(control.ShouldDisplayPlaceholder);
        }

        [AvaloniaFact]
        public void ShouldNotDisplayPlaceholder_WhenItemsSourceIsNotEmpty()
        {
            // Arrange
            var control = new ResourcesUsedControl
            {
                ItemsSource = new List<KeyValuePair<string, string>>
                {
                    new("Key1", "Value1")
                }
            };

            // Assert
            Assert.False(control.ShouldDisplayPlaceholder);
        }

        [AvaloniaFact]
        public void ShouldDisplayShowMoreLessButton_WhenItemsSourceExceedsMaximumLines()
        {
            // Arrange
            var control = new ResourcesUsedControl
            {
                ItemsSource = new List<KeyValuePair<string, string>> {
                new("Key1", "Value1"),
                new("Key2", "Value2"),
                new("Key3", "Value3"),
                new("Key4", "Value4")},
                MaximumLines = 2
            };

            // Assert
            Assert.True(control.ShouldDisplayShowMoreLessButton);
        }

        [AvaloniaFact]
        public void ShouldNotDisplayShowMoreLessButton_WhenItemsSourceDoesNotExceedMaximumLines()
        {
            // Arrange
            var control = new ResourcesUsedControl
            {
                ItemsSource = new List<KeyValuePair<string, string>> {
                new("Key1", "Value1"),
                new("Key2", "Value2")
                },
                MaximumLines = 3
            };

            // Assert
            Assert.False(control.ShouldDisplayShowMoreLessButton);
        }

        [AvaloniaFact]
        public async Task ShowMoreLessButton_Click_ShouldToggleTruncatedItemsSource()
        {
            // Arrange
            var control = new ResourcesUsedControl
            {
                ItemsSource = new List<KeyValuePair<string, string>> {
                new("Key1", "Value1"),
                new("Key2", "Value2"),
                new("Key3", "Value3"),
                new("Key4", "Value4")},
                MaximumLines = 2
            };
            var window = new Window() { Content = control };
            await TestUtils.MeasureAndArrangeControl(control);

            // Act
            var button = control.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "ShowMoreLessButton");
            button?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            // Assert
            Assert.Equal(4, control.TruncatedItemsSource.Cast<object>().Count());

            // Act
            button?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            // Assert
            Assert.Equal(2, control.TruncatedItemsSource.Cast<object>().Count());
        }
    }
}
