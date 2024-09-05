using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Converters
{
    public class BoolToFloatConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue) return boolValue ? 1f : 0f;

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
