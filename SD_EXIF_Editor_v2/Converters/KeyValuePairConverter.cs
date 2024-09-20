using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Converters
{
    public class KeyValuePairConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is KeyValuePair<string, string> kvp && parameter is string param)
            {
                return param switch
                {
                    "Key" => kvp.Key,
                    "Value" => kvp.Value,
                    _ => value,
                };
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
