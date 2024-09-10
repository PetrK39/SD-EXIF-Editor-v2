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

        private readonly MainViewModel _mainViewModel;
        private readonly ISettingsService _settingsService;

        private bool _isRestoringSizePos = true;
        public MainWindow(MainViewModel vm, ISettingsService settingsService)
        {
            DataContext = vm;
            _mainViewModel = vm;
            _settingsService = settingsService;
            InitializeComponent();

            WeakReferenceMessenger.Default.Register(this);
            Loaded += MainWindow_Loaded;
            SizeChanged += MainWindow_SizeChanged;
            PositionChanged += MainWindow_PositionChanged;
            Closing += MainWindow_Closing;
            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            if (IsTextBoxFocused()) return;

            e.Handled = true;

            if (e.Key == Key.O && e.KeyModifiers == KeyModifiers.Control)
            {
                _mainViewModel.OpenCommand.Execute(null);
            }
            else if (e.Key == Key.S && e.KeyModifiers == KeyModifiers.Control)
            {
                _mainViewModel.SaveCommand.Execute(null);
            }
            else if (e.Key == Key.S && e.KeyModifiers.HasFlag(KeyModifiers.Control) && e.KeyModifiers.HasFlag(KeyModifiers.Alt))
            {
                _mainViewModel.SaveAsCommand.Execute(null);
            }
            else if (e.Key == Key.Escape)
            {
                _mainViewModel.ExitCommand.Execute(null);
            }
            else if (e.Key == Key.Z && e.KeyModifiers == KeyModifiers.Control)
            {
                _mainViewModel.UndoCommand.Execute(null);
            }
            else if (e.Key == Key.Y && e.KeyModifiers == KeyModifiers.Control)
            {
                _mainViewModel.RedoCommand.Execute(null);
            }
            else if (e.Key == Key.F1)
            {
                _mainViewModel.OpenAboutCommand.Execute(null);
            }
            else
            {
                e.Handled = false;
            }
        }
        private bool IsTextBoxFocused() => FocusManager?.GetFocusedElement() is TextBox;

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