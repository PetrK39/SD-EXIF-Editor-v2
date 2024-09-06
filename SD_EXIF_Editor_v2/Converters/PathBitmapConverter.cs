using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using SD_EXIF_Editor_v2.Utils;
using System;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Converters
{
    public class PathBitmapConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (value is not Uri uri || !targetType.IsAssignableFrom(typeof(Bitmap)))
            {
                throw new NotSupportedException();
            }

            if (uri.ToString().StartsWith("avares://"))
            {
                return new Bitmap(AssetLoader.Open(uri));
            }
            else
            {
                var file = AvaloniaUtils.GetStorageProvider().TryGetFileFromPathAsync(uri).Result;
                using var stream = file.OpenReadAsync().Result;
                return new Bitmap(stream);
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
