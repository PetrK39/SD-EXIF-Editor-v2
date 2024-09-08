using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Messaging;
using SD_EXIF_Editor_v2.Messages;
using SD_EXIF_Editor_v2.ViewModels;
using System.Diagnostics;

namespace SD_EXIF_Editor_v2.Views
{
    public partial class MainWindow : Window, IRecipient<ImageModelChangedMessage>
    {
        private const string _title = "SD EXIF Editor";
        public MainWindow(MainViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();

            WeakReferenceMessenger.Default.Register(this);
            Loaded += MainWindow_Loaded;
            SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new WindowLoadedMessage(true));

            if (Content is MainView mainView && mainView.DataContext is MainViewModel mainViewModel)
            {
                KeyBindings.Clear();

                KeyBindings.Add(new KeyBinding { Command = mainViewModel.OpenCommand, Gesture = KeyGesture.Parse("Ctrl+O") });

                KeyBindings.Add(new KeyBinding { Command = mainViewModel.SaveCommand, Gesture = KeyGesture.Parse("Ctrl+S") });
                KeyBindings.Add(new KeyBinding { Command = mainViewModel.SaveAsCommand, Gesture = KeyGesture.Parse("Ctrl+Alt+S") });

                KeyBindings.Add(new KeyBinding { Command = mainViewModel.ExitCommand, Gesture = KeyGesture.Parse("Escape") });

                KeyBindings.Add(new KeyBinding { Command = mainViewModel.UndoCommand, Gesture = KeyGesture.Parse("Ctrl+Z") });
                KeyBindings.Add(new KeyBinding { Command = mainViewModel.RedoCommand, Gesture = KeyGesture.Parse("Ctrl+Y") });

                KeyBindings.Add(new KeyBinding { Command = mainViewModel.OpenAboutCommand, Gesture = KeyGesture.Parse("F1") });



            }
        }

        private void MainWindow_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            SendMessage(ClientSize.Width > ClientSize.Height);
        }

        public void Receive(ImageModelChangedMessage message)
        {
            Title = _title + (message.Value ? "*" : "");
        }

        private void SendMessage(bool isHorizontal) => WeakReferenceMessenger.Default.Send(new WindowSizeChangedMessage(isHorizontal));

    }
}