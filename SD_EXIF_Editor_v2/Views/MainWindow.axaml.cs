using Avalonia.Controls;
using SD_EXIF_Editor_v2.ViewModels;

namespace SD_EXIF_Editor_v2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}