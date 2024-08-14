using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Tests.ServicesTests
{
    public class CivitServiceTests
    {
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<ILogger<CivitService>> _mockLogger;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public CivitServiceTests()
        {
            _mockMessageService = new Mock<IMessageService>();
            _mockLogger = new Mock<ILogger<CivitService>>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task GetItemFromHash_ReturnsItem_WhenApiCallIsSuccessful()
        {
            // Arrange
            var origName = "TestName";
            var origHash = "TestHash";

            var responseContent = JsonConvert.SerializeObject(new
            {
                id = 1,
                modelId = 2,
                name = "TestModelVersion",
                model = new { name = "TestModel", type = "TestType" },
                files = new[] { new { sizeKB = 100.0 } },
                images = new[] { new { url = "http://test.com/image", nsfwLevel = NSFWLevels.Soft } },
                downloadUrl = "http://test.com/download"
            });

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            var service = new CivitService(_mockMessageService.Object, _mockLogger.Object, _mockHttpMessageHandler.Object);

            // Act
            var result = await service.GetItemFromHash(origName, origHash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(origName, result.PromptName);
            Assert.Equal("TestModel", result.OriginalName);
            Assert.Equal("TestModelVersion", result.OriginalVersion);
            Assert.Equal("TestType", result.Type);
            Assert.Equal(100.0d, result.SizeKB);
            Assert.Equal("http://test.com/image", result.Images.Single().Uri);
            Assert.Equal(NSFWLevels.Soft, result.Images.Single().NSFWLevel);
            Assert.Equal("http://test.com/download", result.DownloadUri);
            Assert.Equal($"https://civitai.com/models/{2}?modelVersionId={1}", result.SiteUri);
        }

        [Fact]
        public async Task GetItemFromHash_ReturnsBasicItem_WhenApiCallReturnsNotFound()
        {
            // Arrange
            var origName = "TestName";
            var origHash = "TestHash";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Not Found")
                });

            var service = new CivitService(_mockMessageService.Object, _mockLogger.Object, _mockHttpMessageHandler.Object);

            // Act
            var result = await service.GetItemFromHash(origName, origHash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(origName, result.PromptName);
        }

        [Fact]
        public async Task GetItemFromHash_ReturnsBasicItem_WhenApiCallThrowsHttpRequestException()
        {
            // Arrange
            var origName = "TestName";
            var origHash = "TestHash";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Request failed"));

            var service = new CivitService(_mockMessageService.Object, _mockLogger.Object, _mockHttpMessageHandler.Object);

            // Act
            var result = await service.GetItemFromHash(origName, origHash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(origName, result.PromptName);
        }

        [Fact]
        public async Task GetItemFromHash_ReturnsBasicItem_WhenApiCallThrowsJsonException()
        {
            // Arrange
            var origName = "TestName";
            var origHash = "TestHash";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Invalid JSON")
                });

            var service = new CivitService(_mockMessageService.Object, _mockLogger.Object, _mockHttpMessageHandler.Object);

            // Act
            var result = await service.GetItemFromHash(origName, origHash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(origName, result.PromptName);
        }
    }
}
