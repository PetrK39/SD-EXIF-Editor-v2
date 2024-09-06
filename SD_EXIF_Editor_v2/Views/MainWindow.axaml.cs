using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using SD_EXIF_Editor_v2.Messages;
using SD_EXIF_Editor_v2.ViewModels;
using System.Diagnostics;

namespace SD_EXIF_Editor_v2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();

            Loaded += (_, _) => WeakReferenceMessenger.Default.Send(new WindowLoadedMessage(true));
            SizeChanged += MainWindow_SizeChanged;
        }
        private void MainWindow_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            SendMessage(ClientSize.Width > ClientSize.Height);
        }
        private void SendMessage(bool isHorizontal) => WeakReferenceMessenger.Default.Send(new WindowSizeChangedMessage(isHorizontal));

    }
}