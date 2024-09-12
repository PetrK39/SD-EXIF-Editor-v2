using Avalonia;
using Avalonia.Markup.Xaml;

namespace SD_EXIF_Editor_v2.Tests
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}