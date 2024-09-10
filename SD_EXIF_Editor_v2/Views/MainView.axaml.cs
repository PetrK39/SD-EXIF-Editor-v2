using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Messaging;
using SD_EXIF_Editor_v2.Messages;
using System;
using System.Diagnostics;
using System.Linq;

namespace SD_EXIF_Editor_v2.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            SizeChanged += MainView_SizeChanged;

            BorderDragDrop.AddHandler(DragDrop.DragEnterEvent, DragEnter);
            BorderDragDrop.AddHandler(DragDrop.DragLeaveEvent, DragLeave);
            BorderDragDrop.AddHandler(DragDrop.DropEvent, Drop);
        }
        private void MainView_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            SendMessage(DesiredSize.Width > DesiredSize.Height);
        }
        private void SendMessage(bool isHorizontal) => WeakReferenceMessenger.Default.Send(new WindowSizeChangedMessage(isHorizontal));

        private void DragEnter(object? sender, DragEventArgs e)
        {
            var control = sender as Control;

            if (!TryGetUriFromDrop(e.Data, out _))
            {
                control.Cursor = new Cursor(StandardCursorType.No);
                return;
            }

            control.Cursor = Cursor.Default;
            DragDropDisplay.IsVisible = true;
        }
        private void DragLeave(object? sender, DragEventArgs e)
        {
            var control = sender as Control;

            DragDropDisplay.IsVisible = false;
            control.Cursor = Cursor.Default;
        }
        private void Drop(object? sender, DragEventArgs e)
        {
            var control = sender as Control;

            control.Cursor = Cursor.Default;
            DragDropDisplay.IsVisible = false;

            if (!TryGetUriFromDrop(e.Data, out var uri)) return;

            WeakReferenceMessenger.Default.Send(new DragDropOpenFileMessage(uri));
        }
        private bool TryGetUriFromDrop(IDataObject dataObject, out Uri? uri)
        {
            uri = default;

            var files = dataObject.GetFiles();

            if (files is null || !files.Any()) return false;

            var firstFile = files.First();

            if (firstFile is null || !firstFile.Name.ToLower().EndsWith(".png")) return false;

            uri = firstFile.Path;

            return true;
        }
    }
}