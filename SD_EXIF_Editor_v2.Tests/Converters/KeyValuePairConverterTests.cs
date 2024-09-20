using SD_EXIF_Editor_v2.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.Converters
{
    public class KeyValuePairConverterTests
    {
        private readonly KeyValuePairConverter _converter = new KeyValuePairConverter();

        [Fact]
        public void Convert_KeyValuePair_ShouldReturnKey()
        {
            // Arrange
            var value = new KeyValuePair<string, string>("key", "value");
            var parameter = "Key";

            // Act
            var result = _converter.Convert(value, typeof(string), parameter, null);

            // Assert
            Assert.Equal("key", result);
        }

        [Fact]
        public void Convert_KeyValuePair_ShouldReturnValue()
        {
            // Arrange
            var value = new KeyValuePair<string, string>("key", "value");
            var parameter = "Value";

            // Act
            var result = _converter.Convert(value, typeof(string), parameter, null);

            // Assert
            Assert.Equal("value", result);
        }

        [Fact]
        public void Convert_InvalidParameter_ShouldReturnOriginalValue()
        {
            // Arrange
            var value = new KeyValuePair<string, string>("key", "value");
            var parameter = "InvalidParameter";

            // Act
            var result = _converter.Convert(value, typeof(string), parameter, null);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Convert_NonKeyValuePair_ShouldReturnOriginalValue()
        {
            // Arrange
            var value = "not a key-value pair";
            var parameter = "Key";

            // Act
            var result = _converter.Convert(value, typeof(string), parameter, null);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            // Arrange
            var value = "value";
            var parameter = "Key";

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _converter.ConvertBack(value, typeof(KeyValuePair<string, string>), parameter, null));
        }
    }
}
