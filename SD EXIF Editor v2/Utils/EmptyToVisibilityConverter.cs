using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SD_EXIF_Editor_v2.Utils
{
    class EmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case string str:
                    return str == "" ? Visibility.Collapsed : Visibility.Visible;
                case IList<object> list:
                    return list.Any() ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return value is null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
