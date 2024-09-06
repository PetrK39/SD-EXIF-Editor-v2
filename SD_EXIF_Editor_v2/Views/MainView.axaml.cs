using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using SD_EXIF_Editor_v2.Messages;

namespace SD_EXIF_Editor_v2.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            SizeChanged += MainView_SizeChanged;
        }
        private void MainView_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            SendMessage(DesiredSize.Width > DesiredSize.Height);
        }
        private void SendMessage(bool isHorizontal) => WeakReferenceMessenger.Default.Send(new WindowSizeChangedMessage(isHorizontal));
    }
}