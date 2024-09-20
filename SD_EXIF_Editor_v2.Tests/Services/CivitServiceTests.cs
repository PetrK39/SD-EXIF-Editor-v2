using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SD_EXIF_Editor_v2.Services;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Net;

namespace SD_EXIF_Editor_v2.Tests.Services
{
    public class CivitServiceTests
    {
        private readonly Mock<IMessageService> _messageServiceMock;
        private readonly Mock<ILogger<CivitService>> _loggerMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public CivitServiceTests()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _loggerMock = new Mock<ILogger<CivitService>>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task GetItemFromHash_ShouldReturnCivitItem_WhenApiResponseIsSuccessful()
        {
            // Arrange
            var origName = "TestName";
            var origHash = "TestHash";
            var fallbackType = "TestType";
            var strength = 1.23f;

            var apiResponse = new
            {
                id = 1,
                modelId = 2,
                name = "TestVersion",
                model = new { name = "TestModel", type = "TestType" },
                files = new[] { new { sizeKB = 123.45 } },
                images = new[] { new { url = "http://test.com/image.jpg", nsfwLevel = NSFWLevels.None } },
                downloadUrl = "http://test.com/download"
            };

            var jsonResponse = JsonConvert.SerializeObject(apiResponse);
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var civitService = new CivitService(_messageServiceMock.Object, _loggerMock.Object, httpClient);

            // Act
            var result = await civitService.GetItemFromHash(origName, origHash, fallbackType, strength);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(origName, result.PromptName);
            Assert.Equal(strength, result.Strength);
            Assert.Equal("TestModel", result.OriginalName);
            Assert.Equal("TestVersion", result.OriginalVersion);
            Assert.Equal("TestType", result.Type);
            Assert.Equal(123.45, result.SizeKB);
            Assert.Single(result.Images);
            Assert.Equal("http://test.com/image.jpg", result.Images[0].Uri);
            Assert.Equal(NSFWLevels.None, result.Images[0].NSFWLevel);
            Assert.Equal("http://test.com/download", result.DownloadUri);
            Assert.Equal("https://civitai.com/models/2?modelVersionId=1", result.SiteUri);
        }

        [Fact]
        public async Task GetItemFromHash_ShouldReturnCivitItemWithFallback_WhenApiResponseIsNotFound()
        {
            // Arrange
            var origName = "TestName";
            var origHash = "TestHash";
            var fallbackType = "TestType";
            var strength = 1.23f;

            var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var civitService = new CivitService(_messageServiceMock.Object, _loggerMock.Object, httpClient);

            // Act
            var result = await civitService.GetItemFromHash(origName, origHash, fallbackType, strength);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(origName, result.PromptName);
            Assert.Equal(strength, result.Strength);
            Assert.Equal(fallbackType, result.Type);
            Assert.True(result.IsUnknown);
        }

        [Fact]
        public async Task GetItemFromHash_ShouldReturnCivitItemWithFallback_WhenApiResponseIsUnsuccessful()
        {
            // Arrange
            var origName = "TestName";
            var origHash = "TestHash";
            var fallbackType = "TestType";
            var strength = 1.23f;

            var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var civitService = new CivitService(_messageServiceMock.Object, _loggerMock.Object, httpClient);

            // Act
            var result = await civitService.GetItemFromHash(origName, origHash, fallbackType, strength);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(origName, result.PromptName);
            Assert.Equal(strength, result.Strength);
            Assert.Equal(fallbackType, result.Type);
            Assert.True(result.IsUnknown);
        }

        [Fact]
        public async Task GetItemFromHash_ShouldReturnCivitItemWithFallback_WhenHttpRequestExceptionOccurs()
        {
            // Arrange
            var origName = "TestName";
            var origHash = "TestHash";
            var fallbackType = "TestType";
            var strength = 1.23f;

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Test exception"));

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var civitService = new CivitService(_messageServiceMock.Object, _loggerMock.Object, httpClient);

            // Act
            var result = await civitService.GetItemFromHash(origName, origHash, fallbackType, strength);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(origName, result.PromptName);
            Assert.Equal(strength, result.Strength);
            Assert.Equal(fallbackType, result.Type);
            Assert.True(result.IsUnknown);
        }

        [Fact]
        public async Task GetItemFromHash_ShouldReturnCivitItemWithFallback_WhenJsonExceptionOccurs()
        {
            // Arrange
            var origName = "TestName";
            var origHash = "TestHash";
            var fallbackType = "TestType";
            var strength = 1.23f;

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("invalid json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var civitService = new CivitService(_messageServiceMock.Object, _loggerMock.Object, httpClient);

            // Act
            var result = await civitService.GetItemFromHash(origName, origHash, fallbackType, strength);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(origName, result.PromptName);
            Assert.Equal(strength, result.Strength);
            Assert.Equal(fallbackType, result.Type);
            Assert.True(result.IsUnknown);
        }
    }
}
