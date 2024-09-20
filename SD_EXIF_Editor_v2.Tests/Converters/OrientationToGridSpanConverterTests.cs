using SD_EXIF_Editor_v2.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.Converters
{
    public class OrientationToGridSpanConverterTests
    {
        private readonly OrientationToGridSpanConverter _converter = new OrientationToGridSpanConverter();

        [Fact]
        public void Convert_True_ShouldReturnOne()
        {
            // Arrange
            var value = true;

            // Act
            var result = _converter.Convert(value, typeof(int), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Convert_False_ShouldReturnTwo()
        {
            // Arrange
            var value = false;

            // Act
            var result = _converter.Convert(value, typeof(int), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Convert_NonBoolean_ShouldReturnOne()
        {
            // Arrange
            var value = "not a boolean";

            // Act
            var result = _converter.Convert(value, typeof(int), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            // Arrange
            var value = 1;

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _converter.ConvertBack(value, typeof(bool), null, CultureInfo.CurrentCulture));
        }
    }
}
