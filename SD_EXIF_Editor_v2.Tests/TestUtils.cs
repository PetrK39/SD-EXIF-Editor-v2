using Avalonia.Controls;
using Avalonia.Threading;

namespace SD_EXIF_Editor_v2.Tests
{
    public static class TestUtils
    {
        public static async Task MeasureAndArrangeControl(Control control)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                control.Measure(new Avalonia.Size(double.PositiveInfinity, double.PositiveInfinity));
                control.Arrange(new Avalonia.Rect(control.DesiredSize));
            });
        }
    }
}
