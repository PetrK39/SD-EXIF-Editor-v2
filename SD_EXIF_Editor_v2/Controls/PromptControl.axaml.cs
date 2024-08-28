using Avalonia;
using Avalonia.Controls;

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



    public PromptControl()
    {
        InitializeComponent();
    }
}