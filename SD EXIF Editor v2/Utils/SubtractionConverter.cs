using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SD_EXIF_Editor_v2.Utils
{
    public class SubtractionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            return (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
