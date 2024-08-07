using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SD_EXIF_Editor_v2.Control
{
    /// <summary>
    /// Логика взаимодействия для CivitItemsControl.xaml
    /// </summary>
    public partial class CivitItemsControl : UserControl
    {
        public ICommand OpenUriCommand
        {
            get { return (ICommand)GetValue(OpenUriCommandProperty); }
            set { SetValue(OpenUriCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpenUriCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenUriCommandProperty =
            DependencyProperty.Register("OpenUriCommand", typeof(ICommand), typeof(CivitItemsControl), new PropertyMetadata(null));

        public CivitItemsControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Popup popup = Resources.MergedDictionaries[1]["PopupTemplate"] as Popup;

            popup.PlacementTarget = (sender as Hyperlink).Parent as UIElement;
            popup.IsOpen = true;
        }
    }
}
