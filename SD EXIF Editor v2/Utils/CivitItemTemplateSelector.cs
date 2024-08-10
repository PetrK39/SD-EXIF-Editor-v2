using SD_EXIF_Editor_v2.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace SD_EXIF_Editor_v2.Utils
{
    class CivitItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? CivitItemTemplate { get; set; }
        public DataTemplate? UnknownItemTemplate { get; set; }
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                CivitItemViewModel i when !i.IsUnknown => CivitItemTemplate!,
                CivitItemViewModel i when i.IsUnknown => UnknownItemTemplate!,
                _ => null,
            };
        }
    }
}
