using SD_EXIF_Editor_v2.Converters;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.Converters
{
    public class NSFWLevelsToBoolConverterTests
    {
        private readonly NSFWLevelsToBoolConverter _converter = new NSFWLevelsToBoolConverter();

        [Fact]
        public void Convert_EnumValueWithFlag_ShouldReturnTrue()
        {
            // Arrange
            var value = NSFWLevels.None | NSFWLevels.Soft;
            var parameter = NSFWLevels.None;

            // Act
            var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.CurrentCulture);

            // Assert
            Assert.True(result is bool boolResult && boolResult);
        }

        [Fact]
        public void Convert_EnumValueWithoutFlag_ShouldReturnFalse()
        {
            // Arrange
            var value = NSFWLevels.None | NSFWLevels.Soft;
            var parameter = NSFWLevels.Mature;

            // Act
            var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.CurrentCulture);

            // Assert
            Assert.False(result is bool boolResult && boolResult);
        }

        [Fact]
        public void Convert_InvalidValue_ShouldReturnOriginalValue()
        {
            // Arrange
            var value = "not an enum";
            var parameter = NSFWLevels.None;

            // Act
            var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Convert_InvalidParameter_ShouldReturnOriginalValue()
        {
            // Arrange
            var value = NSFWLevels.None;
            var parameter = "not an enum";

            // Act
            var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            // Arrange
            var value = true;
            var parameter = NSFWLevels.None;

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _converter.ConvertBack(value, typeof(NSFWLevels), parameter, CultureInfo.CurrentCulture));
        }
    }
}
