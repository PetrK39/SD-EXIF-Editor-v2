using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SD_EXIF_Editor_v2.Factories;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.ViewModels;
using SD_EXIF_Editor_v2.ViewModels.Interfaces;

namespace SD_EXIF_Editor_v2.Tests.Factories
{
    public class CivitItemViewModelFactoryTests
    {
        private IServiceProvider CreateServiceProvider()
        {
            var settingsServiceMock = new Mock<ISettingsService>();
            var urlOpenerServiceMock = new Mock<IUrlOpenerService>();

            var host = Host.CreateDefaultBuilder().ConfigureServices(
                s =>
                {
                    s.AddSingleton<ICivitItemViewModel, CivitItemViewModel>();
                    s.AddSingleton(settingsServiceMock.Object);
                    s.AddSingleton(urlOpenerServiceMock.Object);
                })
                .ConfigureLogging(c => { })
                .Build();

            return host.Services;
        }

        [Fact]
        public void Create_ShouldReturnInitializedCivitItemViewModel()
        {
            // Arrange
            var civitItem = new CivitItem("promptName", 1.23f, "originalName", "originalVersion", "type", 123, new List<CivitItemImage>(), "downloadUri", "siteUri");
            var serviceProvider = CreateServiceProvider();
            var factory = new CivitItemViewModelFactory(serviceProvider);

            // Act
            var result = factory.Create(civitItem);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CivitItemViewModel>(result);
            Assert.Equal("promptName", result.PromptName);
        }
    }
}
