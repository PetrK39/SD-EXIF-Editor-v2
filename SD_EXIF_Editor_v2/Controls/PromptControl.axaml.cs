using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public static readonly StyledProperty<int> MaximumLinesProperty = 
        AvaloniaProperty.Register<PromptControl, int>(nameof(MaximumLines), 3);


    public static readonly DirectProperty<PromptControl, bool> ShouldDisplayControlProperty =
        AvaloniaProperty.RegisterDirect<PromptControl, bool>(nameof(ShouldDisplayControl), o => o.ShouldDisplayControl);

    public static readonly DirectProperty<PromptControl, bool> ShouldDisplayPlaceholderProperty =
        AvaloniaProperty.RegisterDirect<PromptControl, bool>(nameof(ShouldDisplayPlaceholder), o => o.ShouldDisplayPlaceholder);

    public static readonly DirectProperty<PromptControl, bool> ShouldDisplayShowMoreLessButtonProperty =
        AvaloniaProperty.RegisterDirect<PromptControl, bool>(nameof(ShouldDisplayShowMoreLessButton), o => o.ShouldDisplayShowMoreLessButton);

    public static readonly DirectProperty<PromptControl, string> TruncatedPromptProperty =
        AvaloniaProperty.RegisterDirect<PromptControl, string>(nameof(TruncatedPrompt), o => o.TruncatedPrompt);

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

    public int MaximumLines
    {
        get => GetValue(MaximumLinesProperty);
        set => SetValue(MaximumLinesProperty, value);
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

    private string _truncatedPrompt;
    public string TruncatedPrompt
    {
        get => _truncatedPrompt;
        set => SetAndRaise(TruncatedPromptProperty, ref _truncatedPrompt, value);
    }

    private bool _shouldDisplayShowMoreLessButton;
    public bool ShouldDisplayShowMoreLessButton
    {
        get => _shouldDisplayShowMoreLessButton;
        set => SetAndRaise(ShouldDisplayShowMoreLessButtonProperty, ref _shouldDisplayShowMoreLessButton, value);
    }

    private bool _isExpanded = false;

    public PromptControl()
    {
        InitializeComponent();

        PromptTextBlock.PropertyChanged += PromptTextBlock_PropertyChanged;
    }

    private void PromptTextBlock_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if(e.Property == BoundsProperty)
        {
            UpdateShowMoreLessButtonVisibility();
            UpdateTruncatedPrompt();
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PromptProperty || change.Property == DisplayPlaceholderProperty)
            UpdateDisplayProperties();

        if (change.Property == PromptProperty || change.Property == MaximumLinesProperty)
        {
            UpdateShowMoreLessButtonVisibility();
            UpdateTruncatedPrompt();
        }
    }

    private void UpdateDisplayProperties()
    {
        ShouldDisplayControl = !string.IsNullOrEmpty(Prompt) || DisplayPlaceholder;
        ShouldDisplayPlaceholder = string.IsNullOrEmpty(Prompt) && DisplayPlaceholder;
    }

    private void ShowMoreLessButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _isExpanded = !_isExpanded;

        if(sender is Button btn) btn.Content = _isExpanded ? "Show less" : "Show more";

        UpdateTruncatedPrompt();
    }

    private void UpdateTruncatedPrompt()
    {
        if (string.IsNullOrEmpty(Prompt)) return;

        var lines = GetTextLines();

        if(!_isExpanded && lines.Count > MaximumLines)
        {
            // No "..." as it overflows into MaximumLines+1
            TruncatedPrompt = string.Join("", lines.Take(MaximumLines).Select(tl => tl.TextRuns.Single().Text));
        }
        else
        {
            TruncatedPrompt = Prompt;
        }

    }
    private void UpdateShowMoreLessButtonVisibility()
    {
        ShouldDisplayShowMoreLessButton = GetTextLines().Count > MaximumLines;
    }
    private IReadOnlyList<TextLine> GetTextLines()
    {
        var textLayout = new TextLayout(Prompt,
            GetTypeface(),
            PromptTextBlock.FontSize,
            PromptTextBlock.Foreground,
            PromptTextBlock.TextAlignment,
            PromptTextBlock.TextWrapping,
            PromptTextBlock.TextTrimming,
            PromptTextBlock.TextDecorations,
            PromptTextBlock.FlowDirection,
            PromptTextBlock.Bounds.Width * 0.95); // taking 95% of controls width

        return textLayout.TextLines;
    }
    private Typeface GetTypeface() => new(PromptTextBlock.FontFamily, PromptTextBlock.FontStyle, PromptTextBlock.FontWeight, PromptTextBlock.FontStretch);
}