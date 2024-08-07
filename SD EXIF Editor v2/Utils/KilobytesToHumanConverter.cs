using System.Globalization;
using System.Windows.Data;

namespace SD_EXIF_Editor_v2.Utils
{
    class KilobytesToHumanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
            {
                string[] suf = {"KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
                
                if (val == 0)
                    return "0" + suf[0];

                int place = System.Convert.ToInt32(Math.Floor(Math.Log(val, 1024)));
                double num = Math.Round(val / Math.Pow(1024, place), 1);

                return (Math.Sign(val) * num).ToString() + suf[place];
            }
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
