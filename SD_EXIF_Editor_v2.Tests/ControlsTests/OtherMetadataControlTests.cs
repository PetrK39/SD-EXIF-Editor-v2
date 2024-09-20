using Avalonia.Headless.XUnit;
using SD_EXIF_Editor_v2.Controls;

namespace SD_EXIF_Editor_v2.Tests.ControlsTests
{
    public class OtherMetadataControlTests
    {
        [AvaloniaFact]
        public void ShouldDisplayPlaceholder_WhenMetadataPropertiesIsEmpty()
        {
            // Arrange
            var control = new OtherMetadataControl();

            // Act
            control.MetadataProperties = new List<KeyValuePair<string, string>>();

            // Assert
            Assert.True(control.ShouldDisplayPlaceholder);
        }

        [AvaloniaFact]
        public void ShouldNotDisplayPlaceholder_WhenMetadataPropertiesIsNotEmpty()
        {
            // Arrange
            var control = new OtherMetadataControl();
            var metadata = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Key1", "Value1"),
                new KeyValuePair<string, string>("Key2", "Value2")
            };

            // Act
            control.MetadataProperties = metadata;

            // Assert
            Assert.False(control.ShouldDisplayPlaceholder);
        }

        [AvaloniaFact]
        public void ShouldUpdateDisplayPlaceholder_WhenMetadataPropertiesChanges()
        {
            // Arrange
            var control = new OtherMetadataControl();
            var metadata = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Key1", "Value1")
            };

            // Act
            control.MetadataProperties = metadata;
            control.MetadataProperties = new List<KeyValuePair<string, string>>();

            // Assert
            Assert.True(control.ShouldDisplayPlaceholder);
        }
    }
}
