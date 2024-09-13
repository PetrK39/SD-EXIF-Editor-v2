using Microsoft.Extensions.Logging;
using Moq;
using SD_EXIF_Editor_v2.Services;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.Tests.Services
{
    public class MetadataParserServiceTests
    {
        private readonly Mock<IMessageService> _messageServiceMock;
        private readonly Mock<ILogger<MetadataParserService>> _loggerMock;
        private readonly MetadataParserService _service;

        public MetadataParserServiceTests()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _loggerMock = new Mock<ILogger<MetadataParserService>>();
            _service = new MetadataParserService(_messageServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ParseFromRawMetadataAsync_EmptyMetadata_ReturnsEmptySDMetadata()
        {
            // Arrange
            var rawMetadata = "";

            // Act
            var result = await _service.ParseFromRawMetadataAsync(rawMetadata);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("", result.Prompt);
            Assert.Equal("", result.NegativePrompt);
            Assert.Null(result.Model);
            Assert.Empty(result.MetadataProperties);
            Assert.Empty(result.Loras);
        }

        [Fact]
        public async Task ParseFromRawMetadataAsync_InvalidMetadata_ReturnsSDMetadataWithErrors()
        {
            // Arrange
            var rawMetadata = "Invalid metadata string";

            // Act
            var result = await _service.ParseFromRawMetadataAsync(rawMetadata);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("", result.Prompt);
            Assert.Equal("", result.NegativePrompt);
            Assert.Null(result.Model);
            Assert.Empty(result.MetadataProperties);
            Assert.Empty(result.Loras);
        }

        [Fact]
        public async Task ParseFromRawMetadataAsync_ValidMetadata_ReturnsParsedSDMetadata()
        {
            // Arrange
            var rawMetadata = @"Prompt line 1
                Prompt line 2
                Negative prompt: Negative line 1
                Negative line 2
                Model: SDModel, Model hash: SDModelHash, Lora hashes: ""Lora1:hash1, Lora2:hash2"", OtherKey: OtherValue";

            // Act
            var result = await _service.ParseFromRawMetadataAsync(rawMetadata);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Prompt line 1\nPrompt line 2", result.Prompt);
            Assert.Equal("Negative line 1\nNegative line 2", result.NegativePrompt);
            Assert.Equal("SDModel", result.Model?.Name);
            Assert.Equal("SDModelHash", result.Model?.Hash);
            Assert.Single(result.MetadataProperties);
            Assert.Equal("OtherValue", result.MetadataProperties["OtherKey"]);
            Assert.Equal(2, result.Loras.Count);
            Assert.Equal("Lora1", result.Loras.ElementAt(0).Name);
            Assert.Equal("hash1", result.Loras.ElementAt(0).Hash);
            Assert.Equal("Lora2", result.Loras.ElementAt(1).Name);
            Assert.Equal("hash2", result.Loras.ElementAt(1).Hash);
        }

        [Fact]
        public async Task ParseFromRawMetadataAsync_MetadataWithLoraStrength_ReturnsParsedSDMetadataWithLoraStrength()
        {
            // Arrange
            var rawMetadata = @"<lora:Lora1:0.5>
                <lora:Lora2:0.7>
                Negative prompt: Negative line 1
                Negative line 2
                Model: SDModel, Model hash: SDModelHash, Lora hashes: ""Lora1:hash1, Lora2:hash2"", OtherKey: OtherValue";

            // Act
            var result = await _service.ParseFromRawMetadataAsync(rawMetadata);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("<lora:Lora1:0.5>\n<lora:Lora2:0.7>", result.Prompt);
            Assert.Equal("Negative line 1\nNegative line 2", result.NegativePrompt);
            Assert.Equal("SDModel", result.Model?.Name);
            Assert.Equal("SDModelHash", result.Model?.Hash);
            Assert.Single(result.MetadataProperties);
            Assert.Equal("OtherValue", result.MetadataProperties["OtherKey"]);
            Assert.Equal(2, result.Loras.Count);
            Assert.Equal("Lora1", result.Loras.ElementAt(0).Name);
            Assert.Equal("hash1", result.Loras.ElementAt(0).Hash);
            Assert.Equal(0.5f, result.Loras.ElementAt(0).Strength);
            Assert.Equal("Lora2", result.Loras.ElementAt(1).Name);
            Assert.Equal("hash2", result.Loras.ElementAt(1).Hash);
            Assert.Equal(0.7f, result.Loras.ElementAt(1).Strength);
        }

        [Fact]
        public async Task ParseFromRawMetadataAsync_MetadataWithInvalidLoraStrength_ReturnsParsedSDMetadataWithNaNStrength()
        {
            // Arrange
            var rawMetadata = @"<lora:Lora1:invalid>
                <lora:Lora2:0.7>
                Negative prompt: Negative line 1
                Negative line 2
                Model: SDModel, Model hash: SDModelHash, Lora hashes: ""Lora1:hash1, Lora2:hash2"", OtherKey: OtherValue";

            // Act
            var result = await _service.ParseFromRawMetadataAsync(rawMetadata);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("<lora:Lora1:invalid>\n<lora:Lora2:0.7>", result.Prompt);
            Assert.Equal("Negative line 1\nNegative line 2", result.NegativePrompt);
            Assert.Equal("SDModel", result.Model?.Name);
            Assert.Equal("SDModelHash", result.Model?.Hash);
            Assert.Single(result.MetadataProperties);
            Assert.Equal("OtherValue", result.MetadataProperties["OtherKey"]);
            Assert.Equal(2, result.Loras.Count);
            Assert.Equal("Lora1", result.Loras.ElementAt(0).Name);
            Assert.Equal("hash1", result.Loras.ElementAt(0).Hash);
            Assert.Equal(float.NaN, result.Loras.ElementAt(0).Strength);
            Assert.Equal("Lora2", result.Loras.ElementAt(1).Name);
            Assert.Equal("hash2", result.Loras.ElementAt(1).Hash);
            Assert.Equal(0.7f, result.Loras.ElementAt(1).Strength);
        }
    }
}
