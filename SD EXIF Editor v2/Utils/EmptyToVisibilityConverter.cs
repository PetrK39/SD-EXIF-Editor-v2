using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SD_EXIF_Editor_v2.Utils
{
    class EmptyToVisibilityConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var param = values.FirstOrDefault();

            if (param is string str)
                return str == "" ? Visibility.Collapsed : Visibility.Visible;
            else if (param is IList<object> list)
                return list.Any() ? Visibility.Visible : Visibility.Collapsed;
            else
                return param is null ? Visibility.Collapsed : Visibility.Visible;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
