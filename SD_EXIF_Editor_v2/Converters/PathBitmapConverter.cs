using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Converters
{
    public class PathBitmapConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (value is not string rawUri || !targetType.IsAssignableFrom(typeof(Bitmap)))
            {
                throw new NotSupportedException();
            }

            if (rawUri.StartsWith("avares://"))
            {
                return new Bitmap(AssetLoader.Open(new Uri(rawUri)));
            }
            else
            {
                return new Bitmap(rawUri);
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
