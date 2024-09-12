using SD_EXIF_Editor_v2.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.Converters
{
    public class IntToDisplayStringConverterTests
    {
        private readonly IntToDisplayStringConverter _converter = new IntToDisplayStringConverter();

        [Fact]
        public void Convert_IntValue_ShouldReturnString()
        {
            // Arrange
            var value = 39;

            // Act
            var result = _converter.Convert(value, typeof(string), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal("39", result);
        }

        [Fact]
        public void Convert_IntMaxValue_ShouldReturnShowAlways()
        {
            // Arrange
            var value = int.MaxValue;

            // Act
            var result = _converter.Convert(value, typeof(string), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal("Show always", result);
        }

        [Fact]
        public void Convert_NonIntValue_ShouldReturnOriginalValue()
        {
            // Arrange
            var value = "not an integer";

            // Act
            var result = _converter.Convert(value, typeof(string), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void ConvertBack_ShowAlways_ShouldReturnIntMaxValue()
        {
            // Arrange
            var value = "Show always";

            // Act
            var result = _converter.ConvertBack(value, typeof(int), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(int.MaxValue, result);
        }

        [Fact]
        public void ConvertBack_StringValue_ShouldReturnInt()
        {
            // Arrange
            var value = "39";

            // Act
            var result = _converter.ConvertBack(value, typeof(int), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(39, result);
        }

        [Fact]
        public void ConvertBack_NonStringValue_ShouldReturnOriginalValue()
        {
            // Arrange
            var value = 39;

            // Act
            var result = _converter.ConvertBack(value, typeof(int), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(value, result);
        }
    }
}
