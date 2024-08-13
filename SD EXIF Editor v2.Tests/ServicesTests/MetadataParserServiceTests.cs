using Microsoft.Extensions.Logging;
using Moq;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.ServicesTests
{
    public class MetadataParserServiceTests
    {
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<ILogger<MetadataParserService>> _mockLogger;
        private readonly MetadataParserService _metadataParserService;

        public MetadataParserServiceTests()
        {
            _mockMessageService = new Mock<IMessageService>();
            _mockLogger = new Mock<ILogger<MetadataParserService>>();
            _metadataParserService = new MetadataParserService(_mockMessageService.Object, _mockLogger.Object);
        }

        [Fact]
        public void ParseFromRawMetadata_EmptyMetadata_ReturnsEmptySDMetadata()
        {
            // Arrange
            string rawMetadata = "";

            // Act
            var result = _metadataParserService.ParseFromRawMetadata(rawMetadata);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("", result.Prompt);
            Assert.Equal("", result.NegativePrompt);
        }

        [Fact]
        public void ParseFromRawMetadata_GeneralRegexFail_ReturnsEmptySDMetadata()
        {
            // Arrange
            string rawMetadata = "InvalidMetadata";

            // Act
            var result = _metadataParserService.ParseFromRawMetadata(rawMetadata);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("", result.Prompt);
            Assert.Equal("", result.NegativePrompt);
        }

        [Fact]
        public void ParseFromRawMetadata_MetadataRegexFail_ReturnsPartialSDMetadata()
        {
            // Arrange
            string rawMetadata = "test prompt\nNegative prompt: test negative prompt\nInvalidMetadata";

            // Act
            var result = _metadataParserService.ParseFromRawMetadata(rawMetadata);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test prompt", result.Prompt);
            Assert.Equal("test negative prompt", result.NegativePrompt);
        }

        [Fact]
        public void ParseFromRawMetadata_ValidMetadata_ReturnsCompleteSDMetadata()
        {
            // Arrange
            string rawMetadata = "test prompt, <lora:test lora:-1.23>\nNegative prompt: test negative prompt\nSteps: 20, Sampler: sampler, Schedule type: schedule, CFG scale: 7.5, Seed: 12345, Size: 512x512, Model hash: modelhash, Model: modelname, Lora hashes: \"test lora: test lora hash\", Version: version";

            // Act
            var result = _metadataParserService.ParseFromRawMetadata(rawMetadata);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test prompt, <lora:test lora:-1.23>", result.Prompt);
            Assert.Equal("test negative prompt", result.NegativePrompt);
            Assert.Equal(20, result.Steps);
            Assert.Equal("sampler", result.Sampler);
            Assert.Equal("schedule", result.ScheduleType);
            Assert.Equal(7.5f, result.CFGScale);
            Assert.Equal(12345L, result.Seed);
            Assert.Equal(new Size(512, 512), result.Size);
            Assert.Equal("modelname", result.Model.Name);
            Assert.Equal("modelhash", result.Model.Hash);
            Assert.Equal("version", result.Version);

            Assert.Equal("test lora", result.Loras.Single().Name);
            Assert.Equal("test lora hash", result.Loras.Single().Hash);
            Assert.Equal(-1.23f, result.Loras.Single().Strength);
        }
    }
}
