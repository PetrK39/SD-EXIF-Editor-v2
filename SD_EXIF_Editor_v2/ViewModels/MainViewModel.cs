using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SD_EXIF_Editor_v2.Memento;
using SD_EXIF_Editor_v2.Messages;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Models;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.Utils;
using SD_EXIF_Editor_v2.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SD_EXIF_Editor_v2.ViewModels
{
    public partial class MainViewModel : ObservableObject, 
        IRecipient<WindowLoadedMessage>, 
        IRecipient<WindowSizeChangedMessage>,
        IRecipient<DragDropOpenFileMessage>
    {
        [ObservableProperty]
        private bool _isPaneOpen;

        public bool IsDarkTheme
        {
            get => _settingsService.IsDarkTheme;
            set
            {
                if (SetProperty(_settingsService.IsDarkTheme, value, _settingsService, (s, v) => { s.IsDarkTheme = v; }))
                {
                    _settingsService.Save();
                    UpdateThemeVariant(value);
                }
            }
        }

        [ObservableProperty]
        private ObservableObject _currentPage = new ViewMetadataViewModel();

        [ObservableProperty]
        private ListItemTemplate? _selectedListItem;

        private readonly ImageModel _imageModel;
        private readonly WindowOrientationModel _windowOrientationModel;
        private readonly IFileService _fileService;
        private readonly IStartupFileService _startupFileService;
        private readonly ISettingsService _settingsService;

        private IDisposable? _previousViewModel = null;

        private ImageModelCaretaker? _caretaker;

        public ObservableCollection<ListItemTemplate> Items { get; }

        private readonly List<ListItemTemplate> _templates =
        [
            new ListItemTemplate(typeof(ViewMetadataViewModel), "InputCursorText", "View"),
            new ListItemTemplate(typeof(EditMetadataViewModel), "Pencil", "Edit"),
            new ListItemTemplate(typeof(SettingsViewModel), "Gear", "Settings"),
        ];

        // Design only
        public MainViewModel()
        {
            if (Design.IsDesignMode)
            {
                _imageModel = new ImageModel()
                {
                    RawMetadata = "prompt\r\nNegative prompt:negative\r\nVersion: design",
                    IsFileLoaded = true
                };
                _fileService = null!;
                _startupFileService = null!;

                Items = new ObservableCollection<ListItemTemplate>(_templates);

                SelectedListItem = Items.First(vm => vm.ModelType == typeof(IViewMetadataViewModel));

            }
        }
        public MainViewModel(ImageModel imageModel, WindowOrientationModel windowOrientationModel, IFileService fileService, IStartupFileService startupFileService, ISettingsService settingsService)
        {
            Items = new ObservableCollection<ListItemTemplate>(_templates);

            SelectedListItem = Items.First(vm => vm.ModelType == typeof(ViewMetadataViewModel));

            _imageModel = imageModel;
            _windowOrientationModel = windowOrientationModel;
            _fileService = fileService;
            _startupFileService = startupFileService;
            _settingsService = settingsService;

            _imageModel.PropertyChanged += imageModel_PropertyChanged;

            WeakReferenceMessenger.Default.Register<WindowLoadedMessage>(this);
            WeakReferenceMessenger.Default.Register<WindowSizeChangedMessage>(this);
            WeakReferenceMessenger.Default.Register<DragDropOpenFileMessage>(this);

            UpdateThemeVariant(_settingsService.IsDarkTheme);
        }

        private void imageModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_imageModel.FileUri))
            {
                _caretaker = null;
            }
            else if (e.PropertyName == nameof(_imageModel.RawMetadata))
            {
                if (_caretaker is not null) return;

                _caretaker = new(_imageModel);
                _caretaker.PropertyChanged += caretaker_PropertyChanged;
            }
        }

        private void caretaker_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_caretaker is null) return;

            switch (e.PropertyName)
            {
                case nameof(_caretaker.CanUndo):
                    UndoCommand.NotifyCanExecuteChanged();
                    return;
                case nameof(_caretaker.CanRedo):
                    RedoCommand.NotifyCanExecuteChanged();
                    return;
                case nameof(_caretaker.HasChanged):
                    WeakReferenceMessenger.Default.Send(new ImageModelChangedMessage(_caretaker!.HasChanged));
                    return;
                default:
                    break;
            }
        }

        public async void Receive(WindowLoadedMessage message)
        {
            if (message.Value)
                await InitializeStartupFile();
        }
        public void Receive(WindowSizeChangedMessage message)
        {
            _windowOrientationModel.IsHorizontal = message.Value;
        }
        public async void Receive(DragDropOpenFileMessage message)
        {
            await _fileService.LoadFileIntoModelAsync(_imageModel, message.Value);
        }

        private async Task InitializeStartupFile()
        {
            var filePath = await _startupFileService.GetStartupFileAsync();

            if (filePath is null) return;

            var uri = new Uri(filePath);

            await _fileService.LoadFileIntoModelAsync(_imageModel, uri);
        }

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;

            var vm = Design.IsDesignMode
                ? Activator.CreateInstance(value.ModelType)
                : Ioc.Default.GetService(value.ModelType.GetInterfaces().Single(i => i.Name.EndsWith("ViewModel")));

            if (vm is not ObservableObject oo) return;

            if (_previousViewModel != null) _previousViewModel.Dispose();
            if (vm is IDisposable disposable) _previousViewModel = disposable;

            CurrentPage = oo;
        }

        private void UpdateThemeVariant(bool isDarkTheme)
        {
            App.Current!.RequestedThemeVariant = isDarkTheme ? Avalonia.Styling.ThemeVariant.Dark : Avalonia.Styling.ThemeVariant.Light;
        }

        private bool IsFileLoaded => _imageModel.IsFileLoaded;

        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        #region Application Commands
        [RelayCommand]
        private async Task OpenAsync()
        {
            var file = await _fileService.PickFileToLoad();
            if (file is null) return;
            // TODO: android support requires a lot of additional work for content providers, paths and stuff
            await _fileService.LoadFileIntoModelAsync(_imageModel, file);
        }

        [RelayCommand(CanExecute = nameof(IsFileLoaded))]
        private void Close()
        {
            _fileService.CloseFileFromModel(_imageModel);
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveAsync()
        {
            await _fileService.SaveFileFromModelAsync(_imageModel);
        }
        private bool CanSave() => IsFileLoaded && (_caretaker?.HasChanged ?? false);

        [RelayCommand(CanExecute = nameof(IsFileLoaded))]
        private async Task SaveAsAsync()
        {
            var newPath = await _fileService.PickFileToSave();

            if (newPath is null) return;

            await _fileService.SaveFileAsFromModelAsync(_imageModel, newPath);
        }

        [RelayCommand]
        private void ToggleThemeVariant()
        {
            IsDarkTheme = !IsDarkTheme;
        }

        [RelayCommand(CanExecute = nameof(CanUndo))]
        private void Undo() => _caretaker?.Undo();
        private bool CanUndo() => _caretaker?.CanUndo ?? false;
        [RelayCommand(CanExecute = nameof(CanRedo))]
        private void Redo() => _caretaker?.Redo();
        private bool CanRedo() => _caretaker?.CanRedo ?? false;

        [RelayCommand]
        private void Exit()
        {
            Environment.Exit(0);
        }

        [RelayCommand]
        private void OpenAbout()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
