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
    class EmptyToVisibilityMultiConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            switch (values)
            {
                case IList<object> list:
                    return list.All(i => i is not null && i is string str && str != "") ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return values is null || values.Length == 0 ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
