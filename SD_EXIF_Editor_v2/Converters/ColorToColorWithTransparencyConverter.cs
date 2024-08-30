using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Converters
{
    public class ColorToColorWithTransparencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color && parameter is string alphaString && float.TryParse(alphaString, CultureInfo.InvariantCulture, out float alpha))
            {
                alpha = Math.Clamp(alpha, 0, 1); // Ensure alpha is between 0 and 1
                return Color.FromArgb((byte)(alpha * 255), color.R, color.G, color.B);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
