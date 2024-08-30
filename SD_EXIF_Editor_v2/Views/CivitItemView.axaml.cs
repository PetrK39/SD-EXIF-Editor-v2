using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using SD_EXIF_Editor_v2.ViewModels;

namespace SD_EXIF_Editor_v2;

public partial class CivitItemView : UserControl
{
    public CivitItemView()
    {
        InitializeComponent();
    }

    private void UserControl_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var vm = DataContext as CivitItemViewModel;
        if (vm is null || vm.IsUnknown) return;

        var ctl = sender as Control;
        if (ctl != null)
        {
            FlyoutBase.ShowAttachedFlyout(ctl);
        }
    }
}