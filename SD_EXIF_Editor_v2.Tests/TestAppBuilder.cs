using Avalonia;
using Avalonia.Headless;
using SD_EXIF_Editor_v2.Tests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]
namespace SD_EXIF_Editor_v2.Tests
{

    public class TestAppBuilder
    {
        public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions());
    }
}
