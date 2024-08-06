using System.Globalization;
using System.Windows.Data;

namespace SD_EXIF_Editor_v2.Utils
{
    class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double val)
            {
                var param = double.Parse(parameter as string);
                return val - param;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
