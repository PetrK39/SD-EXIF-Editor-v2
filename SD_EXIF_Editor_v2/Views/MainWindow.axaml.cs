using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using SD_EXIF_Editor_v2.Messages;
using SD_EXIF_Editor_v2.ViewModels;

namespace SD_EXIF_Editor_v2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();

            Loaded += (_, _) => WeakReferenceMessenger.Default.Send(new WindowLoadedMessage(true));
        }
    }
}