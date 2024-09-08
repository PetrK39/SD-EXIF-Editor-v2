using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Converters
{
    public class IntToDisplayStringConverter : IValueConverter
    {
        private const string _showAlwaysStr = "Show always";
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                switch (intValue)
                {
                    case int.MaxValue:
                        return _showAlwaysStr;
                    default:
                        return intValue.ToString();
                }
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                switch (stringValue)
                {
                    case _showAlwaysStr:
                        return int.MaxValue;
                    default:
                        return int.Parse(stringValue);
                }
            }
            return value;
        }
    }
}
