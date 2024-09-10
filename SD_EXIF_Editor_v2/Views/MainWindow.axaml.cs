using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Messaging;
using SD_EXIF_Editor_v2.Messages;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.ViewModels;
using System;
using System.Diagnostics;

namespace SD_EXIF_Editor_v2.Views
{
    public partial class MainWindow : Window, IRecipient<ImageModelChangedMessage>
    {
        private const string _title = "SD EXIF Editor";

        private readonly ISettingsService _settingsService;

        private bool _isRestoringSizePos = true;
        public MainWindow(MainViewModel vm, ISettingsService settingsService)
        {
            DataContext = vm;
            _settingsService = settingsService;
            InitializeComponent();

            WeakReferenceMessenger.Default.Register(this);
            Loaded += MainWindow_Loaded;
            SizeChanged += MainWindow_SizeChanged;
            PositionChanged += MainWindow_PositionChanged;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            _settingsService.Save();
        }

        private void MainWindow_PositionChanged(object? sender, PixelPointEventArgs e)
        {
            if (_isRestoringSizePos) return;

            _settingsService.WindowTop = Position.Y;
            _settingsService.WindowLeft = Position.X;
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

            _isRestoringSizePos = true;

            if (_settingsService.ShouldKeepSize)
            {
                if (_settingsService.WindowWidth > 0)
                    Width = _settingsService.WindowWidth;
                if (_settingsService.WindowHeight > 0)
                    Height = _settingsService.WindowHeight;
            }

            if (_settingsService.ShouldKeepPos)
            {
                Position = new PixelPoint(
                    Math.Clamp(_settingsService.WindowLeft, 0, Screens.Primary.Bounds.Width),
                    Math.Clamp(_settingsService.WindowTop, 0, Screens.Primary.Bounds.Height));
            }

            _isRestoringSizePos = false;
        }

        private void MainWindow_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            SendMessage(ClientSize.Width > ClientSize.Height);

            if (_isRestoringSizePos || double.IsNaN(Width) || double.IsNaN(Height)) return;

            _settingsService.WindowWidth = Width;
            _settingsService.WindowHeight = Height;
        }

        public void Receive(ImageModelChangedMessage message)
        {
            Title = _title + (message.Value ? "*" : "");
        }

        private void SendMessage(bool isHorizontal) => WeakReferenceMessenger.Default.Send(new WindowSizeChangedMessage(isHorizontal));

    }
}