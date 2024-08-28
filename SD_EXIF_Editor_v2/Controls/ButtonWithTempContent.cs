using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;

namespace SD_EXIF_Editor_v2.Controls
{
    public class ButtonWithTempContent : Button
    {
        public static readonly StyledProperty<object> TempContentProperty =
            AvaloniaProperty.Register<ButtonWithTempContent, object>(nameof(TempContent), defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<int> TempDurationProperty =
            AvaloniaProperty.Register<ButtonWithTempContent, int>(nameof(TempDuration), defaultValue: 3);

        private object? _originalContent;
        private readonly DispatcherTimer _timer;

        public object TempContent
        {
            get { return GetValue(TempContentProperty); }
            set { SetValue(TempContentProperty, value); }
        }

        public int TempDuration
        {
            get { return GetValue(TempDurationProperty); }
            set { SetValue(TempDurationProperty, value); }
        }

        public ButtonWithTempContent()
        {
            Click += TempContentButton_Click;
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
        }

        private void TempContentButton_Click(object? sender, RoutedEventArgs e)
        {
            _originalContent ??= Content;
            Content = TempContent;
            _timer.Interval = TimeSpan.FromSeconds(TempDuration);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Content = _originalContent!;
            _originalContent = null;
            _timer.Stop();
        }
    }
}