using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SD_EXIF_Editor_v2.Control
{
    public class ButtonWithTempContent : Button
    {
        public static readonly DependencyProperty TempContentProperty =
            DependencyProperty.Register("TempContent", typeof(object), typeof(ButtonWithTempContent), new PropertyMetadata(null));

        public static readonly DependencyProperty TempDurationProperty =
            DependencyProperty.Register("TempDuration", typeof(int), typeof(ButtonWithTempContent), new PropertyMetadata(3));

        private object? originalContent;
        private DispatcherTimer timer;

        public object TempContent
        {
            get { return GetValue(TempContentProperty); }
            set { SetValue(TempContentProperty, value); }
        }

        public int TempDuration
        {
            get { return (int)GetValue(TempDurationProperty); }
            set { SetValue(TempDurationProperty, value); }
        }

        public ButtonWithTempContent()
        {
            Click += TempContentButton_Click;
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
        }

        private void TempContentButton_Click(object sender, RoutedEventArgs e)
        {
            originalContent ??= Content;
            Content = TempContent;
            timer.Interval = TimeSpan.FromSeconds(TempDuration);
            timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            Content = originalContent;
            originalContent = null;
            timer.Stop();
        }
    }

}
