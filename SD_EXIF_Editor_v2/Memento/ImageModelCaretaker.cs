using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SD_EXIF_Editor_v2.Memento
{
    public class ImageModelCaretaker : ObservableObject
    {
        private readonly ImageModel _originator;
        private readonly Stack<StringMemento> _undoStack = new Stack<StringMemento>();
        private readonly Stack<StringMemento> _redoStack = new Stack<StringMemento>();
        private bool _isUndoingOrRedoing = false;

        public bool CanUndo => _undoStack.Count > 1;
        public bool CanRedo => _redoStack.Count > 0;
        public bool HasChanged => _undoStack.LastOrDefault()?.State != _originator.RawMetadata;

        public ImageModelCaretaker(ImageModel originator)
        {
            _originator = originator;
            _undoStack.Push(_originator.SaveMemento());
            _originator.PropertyChanging += OnPropertyChanging;
            _originator.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_originator.RawMetadata))
            {
                OnPropertyChanged(nameof(HasChanged));
            }
        }

        private void OnPropertyChanging(object? sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == nameof(_originator.RawMetadata) && !_isUndoingOrRedoing)
            {
                _undoStack.Push(_originator.SaveMemento());

                OnPropertyChanged(nameof(CanUndo));
                OnPropertyChanged(nameof(CanRedo));

            }
        }

        public void Undo()
        {
            if (_undoStack.Count > 1)
            {
                _isUndoingOrRedoing = true;
                var memento = _undoStack.Pop();
                _redoStack.Push(_originator.SaveMemento());
                _originator.LoadMemento(memento);
                _isUndoingOrRedoing = false;

                OnPropertyChanged(nameof(CanUndo));
                OnPropertyChanged(nameof(CanRedo));
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                _isUndoingOrRedoing = true;
                var memento = _redoStack.Pop();
                _undoStack.Push(_originator.SaveMemento());
                _originator.LoadMemento(memento);
                _isUndoingOrRedoing = false;

                OnPropertyChanged(nameof(CanUndo));
                OnPropertyChanged(nameof(CanRedo));
            }
        }
    }
}
