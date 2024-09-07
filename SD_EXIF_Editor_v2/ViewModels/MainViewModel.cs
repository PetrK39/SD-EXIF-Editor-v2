using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SD_EXIF_Editor_v2.Messages;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Models;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.ViewModels
{
    public partial class MainViewModel : ObservableObject, IRecipient<WindowLoadedMessage>, IRecipient<WindowSizeChangedMessage>
    {
        [ObservableProperty]
        private bool _isPaneOpen;

        [ObservableProperty]
        private ObservableObject _currentPage = new ViewMetadataViewModel();

        [ObservableProperty]
        private ListItemTemplate? _selectedListItem;

        private readonly ImageModel _imageModel;
        private readonly WindowOrientationModel _windowOrientationModel;
        private readonly IFileService _fileService;
        private readonly IStartupFileService _startupFileService;

        private IDisposable? _previousViewModel = null;

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

                SelectedListItem = Items.First(vm => vm.ModelType == typeof(ViewMetadataViewModel));

            }
        }
        public MainViewModel(ImageModel imageModel, WindowOrientationModel windowOrientationModel, IFileService fileService, IStartupFileService startupFileService)
        {
            Items = new ObservableCollection<ListItemTemplate>(_templates);

            SelectedListItem = Items.First(vm => vm.ModelType == typeof(ViewMetadataViewModel));

            _imageModel = imageModel;
            _windowOrientationModel = windowOrientationModel;
            _fileService = fileService;
            _startupFileService = startupFileService;

            WeakReferenceMessenger.Default.Register<WindowLoadedMessage>(this);
            WeakReferenceMessenger.Default.Register<WindowSizeChangedMessage>(this);
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
                : Ioc.Default.GetService(value.ModelType);

            if (vm is not ObservableObject oo) return;

            if (_previousViewModel != null) _previousViewModel.Dispose();
            if (vm is IDisposable disposable) _previousViewModel = disposable;

            CurrentPage = oo;
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

        [RelayCommand(CanExecute = nameof(IsFileLoaded))]
        private async Task SaveAsync()
        {
            await _fileService.SaveFileFromModelAsync(_imageModel);
        }

        [RelayCommand(CanExecute = nameof(IsFileLoaded))]
        private async Task SaveAsAsync()
        {
            var newPath = await _fileService.PickFileToSave();

            if (newPath is null) return;

            await _fileService.SaveFileAsFromModelAsync(_imageModel, newPath);
        }
        #endregion
    }
}
