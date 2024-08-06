using SD_EXIF_Editor_v2.Model;
using System.Windows;
using System.Windows.Controls;

namespace SD_EXIF_Editor_v2.Utils
{
    class CivitItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CivitItemWithStrength { get; set; }
        public DataTemplate CivitItemNoStrength { get; set; }
        public DataTemplate UnknownItemWithStrength { get; set; }
        public DataTemplate UnknownItemNoStrength { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is CivitItem civitItem)
            {
                switch (civitItem)
                {
                    case CivitItem i when !i.IsUnknown && i.Strength is not null:
                        return CivitItemWithStrength;
                    case CivitItem i when !i.IsUnknown && i.Strength is null:
                        return CivitItemNoStrength;
                    case CivitItem i when i.IsUnknown && i.Strength is not null:
                        return UnknownItemWithStrength;
                    case CivitItem i when i.IsUnknown && i.Strength is null:
                        return UnknownItemNoStrength;
                    default:
                        return null;
                }
            }
            else return null;
        }
    }
}
