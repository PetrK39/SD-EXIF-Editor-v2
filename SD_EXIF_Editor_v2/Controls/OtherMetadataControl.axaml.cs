using Avalonia;
using Avalonia.Controls;
using System.Collections.Generic;
using System.Linq;

namespace SD_EXIF_Editor_v2.Controls;

public partial class OtherMetadataControl : UserControl
{
    public static readonly StyledProperty<IEnumerable<KeyValuePair<string, string>>> MetadataPropertiesProperty =
        AvaloniaProperty.Register<OtherMetadataControl, IEnumerable<KeyValuePair<string, string>>>(nameof(MetadataProperties));

    public static readonly DirectProperty<OtherMetadataControl, bool> ShouldDisplayPlaceholderProperty =
        AvaloniaProperty.RegisterDirect<OtherMetadataControl, bool>(nameof(ShouldDisplayPlaceholder), o => o.ShouldDisplayPlaceholder);

    public IEnumerable<KeyValuePair<string, string>> MetadataProperties
    {
        get => GetValue(MetadataPropertiesProperty);
        set => SetValue(MetadataPropertiesProperty, value);
    }

    private bool _shouldDisplayPlaceholder;
    public bool ShouldDisplayPlaceholder
    {
        get => _shouldDisplayPlaceholder;
        set => SetAndRaise(ShouldDisplayPlaceholderProperty, ref _shouldDisplayPlaceholder, value);
    }

    public OtherMetadataControl()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == MetadataPropertiesProperty)
        {
            UpdateDisplayProperties();
        }
    }
    private void UpdateDisplayProperties()
    {
        ShouldDisplayPlaceholder = !MetadataProperties?.Cast<object>().Any() ?? true;
    }
}