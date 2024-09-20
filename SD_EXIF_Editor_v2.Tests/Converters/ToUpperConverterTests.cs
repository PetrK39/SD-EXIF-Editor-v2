using SD_EXIF_Editor_v2.Converters;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Tests.Converters
{
    public class ToUpperConverterTests
    {
        private readonly ToUpperConverter _converter = new ToUpperConverter();

        [Fact]
        public void Convert_String_ShouldReturnUppercase()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = _converter.Convert(value, typeof(string), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal("HELLO", result);
        }

        [Fact]
        public void Convert_NonString_ShouldReturnOriginalValue()
        {
            // Arrange
            var value = 123;

            // Act
            var result = _converter.Convert(value, typeof(string), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            // Arrange
            var value = "HELLO";

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _converter.ConvertBack(value, typeof(string), null, CultureInfo.CurrentCulture));
        }
    }
}
