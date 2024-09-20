using Avalonia;
using SD_EXIF_Editor_v2.ViewModels;

namespace SD_EXIF_Editor_v2.Utils
{
    public class CivitItemViewModelBindingProxy : AvaloniaObject
    {
        public static readonly StyledProperty<CivitItemViewModel> VMProperty =
            AvaloniaProperty.Register<CivitItemViewModelBindingProxy, CivitItemViewModel>(nameof(VM));

        public object VM
        {
            get => GetValue(VMProperty);
            set => SetValue(VMProperty, value);
        }
    }
}
