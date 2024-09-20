using SD_EXIF_Editor_v2.Converters;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Tests.Converters
{
    public class OrientationToGridIndexConverterTests
    {
        private readonly OrientationToGridIndexConverter _converter = new OrientationToGridIndexConverter();

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
        public void Convert_False_ShouldReturnZero()
        {
            // Arrange
            var value = false;

            // Act
            var result = _converter.Convert(value, typeof(int), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Convert_NonBoolean_ShouldReturnZero()
        {
            // Arrange
            var value = "not a boolean";

            // Act
            var result = _converter.Convert(value, typeof(int), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(0, result);
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
