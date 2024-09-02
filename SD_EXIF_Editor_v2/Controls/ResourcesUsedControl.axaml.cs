using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;

namespace SD_EXIF_Editor_v2.Controls;

public partial class ResourcesUsedControl : UserControl
{
    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
        AvaloniaProperty.Register<ResourcesUsedControl, IEnumerable>(nameof(ItemsSource));

    public static readonly StyledProperty<int> MaximumLinesProperty =
        AvaloniaProperty.Register<ResourcesUsedControl, int>(nameof(MaximumLines), 3);

    public static readonly DirectProperty<ResourcesUsedControl, bool> ShouldDisplayShowMoreLessButtonProperty =
        AvaloniaProperty.RegisterDirect<ResourcesUsedControl, bool>(nameof(ShouldDisplayShowMoreLessButton), o => o.ShouldDisplayShowMoreLessButton);

    public static readonly DirectProperty<ResourcesUsedControl, IEnumerable> TruncatedItemsSourceProperty =
        AvaloniaProperty.RegisterDirect<ResourcesUsedControl, IEnumerable>(nameof(TruncatedItemsSource), o => o.TruncatedItemsSource);

    public IEnumerable ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public int MaximumLines
    {
        get => GetValue(MaximumLinesProperty);
        set => SetValue(MaximumLinesProperty, value);
    }

    private bool _shouldDisplayShowMoreLessButton;
    public bool ShouldDisplayShowMoreLessButton
    {
        get => _shouldDisplayShowMoreLessButton;
        set => SetAndRaise(ShouldDisplayShowMoreLessButtonProperty, ref _shouldDisplayShowMoreLessButton, value);
    }

    private IEnumerable _truncatedItemsSource;
    public IEnumerable TruncatedItemsSource
    {
        get => _truncatedItemsSource;
        set => SetAndRaise(TruncatedItemsSourceProperty, ref _truncatedItemsSource, value);
    }

    private bool _isExpanded = false;

    public ResourcesUsedControl()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ItemsSourceProperty ||
            change.Property == MaximumLinesProperty)
        {
            UpdateDisplayProperties();
            UpdateShowMoreLessButtonContent();
            UpdateTruncatedItemsSource();
        }

        if (change.Property == ItemsSourceProperty)
        {
            if (change.OldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= OnItemsSourceChanged;
            }

            if (change.NewValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += OnItemsSourceChanged;
            }
        }
    }

    private void ShowMoreLessButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _isExpanded = !_isExpanded;

        UpdateShowMoreLessButtonContent();

        UpdateTruncatedItemsSource();
    }
    private void UpdateTruncatedItemsSource()
    {
        if (ItemsSource is null)
        {
            TruncatedItemsSource = null!;
            return;
        }

        var count = GetItemsSourceCount();

        if (!_isExpanded && count > MaximumLines)
        {
            TruncatedItemsSource = ItemsSource.Cast<object>().Take(MaximumLines).ToList();
        }
        else
        {
            // Without casting it to list the region above throws out of range exception on second image loading
            TruncatedItemsSource = ItemsSource.Cast<object>().ToList();
        }
    }
    private void UpdateShowMoreLessButtonContent()
    {
        var count = GetItemsSourceCount();

        ShowMoreLessButton.Content = _isExpanded ? "Show less" : $"Show {count - MaximumLines} more";
    }

    private void UpdateDisplayProperties()
    {
        ShouldDisplayShowMoreLessButton = GetItemsSourceCount() > MaximumLines;
    }

    private void OnItemsSourceChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateDisplayProperties();
        UpdateShowMoreLessButtonContent();
        UpdateTruncatedItemsSource();
    }

    private int GetItemsSourceCount() => ItemsSource?.Cast<object>().Count() ?? 0;
}