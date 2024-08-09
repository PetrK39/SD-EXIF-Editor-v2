using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace SD_EXIF_Editor_v2.Utils
{
    class NotEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case string str:
                    return str == "" ? Visibility.Visible : Visibility.Collapsed;
                case ListCollectionView view:
                    return view.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                case IList<object> list:
                    return list.Any() ? Visibility.Collapsed : Visibility.Visible;
                default:
                    return value is null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
