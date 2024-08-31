using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace SD_EXIF_Editor_v2.Controls;

public partial class PromptControl : UserControl
{

    /// <summary>
    /// Title StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<PromptControl, string>(nameof(Title));

    /// <summary>
    /// Prompt StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<string> PromptProperty =
        AvaloniaProperty.Register<PromptControl, string>(nameof(Prompt));

    /// <summary>
    /// CopyCommand StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<ICommand> CopyCommandProperty =
        AvaloniaProperty.Register<PromptControl, ICommand>(nameof(CopyCommand));

    /// <summary>
    /// CopyCommand StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<bool> DisplayPlaceholderProperty =
        AvaloniaProperty.Register<PromptControl, bool>(nameof(DisplayPlaceholder));


    public static readonly DirectProperty<PromptControl, bool> ShouldDisplayControlProperty =
        AvaloniaProperty.RegisterDirect<PromptControl, bool>(nameof(ShouldDisplayControl), o => o.ShouldDisplayControl);

    public static readonly DirectProperty<PromptControl, bool> ShouldDisplayPlaceholderProperty =
        AvaloniaProperty.RegisterDirect<PromptControl, bool>(nameof(ShouldDisplayPlaceholder), o => o.ShouldDisplayPlaceholder);

    /// <summary>
    /// Gets or sets the Title property. This StyledProperty
    /// indicates title of the PromptControl.
    /// </summary>
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the Prompt property. This StyledProperty
    /// indicates prompt of the PromptControl.
    /// </summary>
    public string Prompt
    {
        get => GetValue(PromptProperty);
        set => SetValue(PromptProperty, value);
    }

    /// <summary>
    /// Gets or sets the CopyCommand property. This StyledProperty
    /// indicates copy command of the PromptControl.
    /// </summary>
    public ICommand CopyCommand
    {
        get => GetValue(CopyCommandProperty);
        set => SetValue(CopyCommandProperty, value);
    }

    public bool DisplayPlaceholder
    {
        get => GetValue(DisplayPlaceholderProperty);
        set => SetValue(DisplayPlaceholderProperty, value);
    }

    private bool _shouldDisplayControl;
    public bool ShouldDisplayControl
    {
        get => _shouldDisplayControl;
        set => SetAndRaise(ShouldDisplayControlProperty, ref _shouldDisplayControl, value);
    }

    private bool _shouldDisplayPlaceholder;
    public bool ShouldDisplayPlaceholder
    {
        get => _shouldDisplayPlaceholder;
        set => SetAndRaise(ShouldDisplayPlaceholderProperty, ref _shouldDisplayPlaceholder, value);
    }

    public PromptControl()
    {
        InitializeComponent();

        UpdateProperties();
    }
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PromptProperty || change.Property == DisplayPlaceholderProperty)
            UpdateProperties();
    }

    private void UpdateProperties()
    {
        ShouldDisplayControl =  !string.IsNullOrEmpty(Prompt) || DisplayPlaceholder;
        ShouldDisplayPlaceholder = string.IsNullOrEmpty(Prompt) && DisplayPlaceholder;
    }
}