using Avalonia.Data;
using Avalonia.Data.Converters;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Converters
{
    internal class NSFWLevelsToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is NSFWLevels enumValue && parameter is NSFWLevels flag)
            {
                return enumValue.HasFlag(flag);
            }

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {     
            throw new NotImplementedException();
        }
    }
}
