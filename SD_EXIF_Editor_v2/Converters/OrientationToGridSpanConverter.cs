using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Converters
{
    public class OrientationToGridSpanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is bool isHorizontal)
            {
                return isHorizontal ? 1 : 2;
            }
            return 1;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
