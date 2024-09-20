using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Converters
{
    public class OrientationToGridIndexConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is bool isHorisontal)
            {
                return isHorisontal ? 1 : 0;
            }

            return 0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
