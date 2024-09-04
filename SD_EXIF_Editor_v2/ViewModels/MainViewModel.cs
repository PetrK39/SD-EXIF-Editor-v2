using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
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
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isPaneOpen;

        [ObservableProperty]
        private ObservableObject _currentPage = new ViewMetadataViewModel();

        [ObservableProperty]
        private ListItemTemplate? _selectedListItem;

        private readonly ImageModel _imageModel;
        private readonly IFileService _fileService;

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
                    FilePath = "/test/path/to/image.png",
                    RawMetadata = "prompt\r\nNegative prompt:negative\r\nVersion: design",
                    IsFileLoaded = true
                };
                _fileService = null!;
            }
        }
        public MainViewModel(ImageModel imageModel, IFileService fileService)
        {
            Items = new ObservableCollection<ListItemTemplate>(_templates);

            SelectedListItem = Items.First(vm => vm.ModelType == typeof(ViewMetadataViewModel));

            _imageModel = imageModel;
            _fileService = fileService;
        }

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;

            var vm = Design.IsDesignMode
                ? Activator.CreateInstance(value.ModelType)
                : Ioc.Default.GetService(value.ModelType);

            if (vm is not ObservableObject oo) return;

            if(_previousViewModel != null) _previousViewModel.Dispose();
            if(vm is IDisposable disposable) _previousViewModel = disposable;

            CurrentPage = oo;
        }

        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        #region Application Commands
        [RelayCommand]
        private async Task OpenAsync()
        {
            var file = await _fileService.PickFile();
            if(file is null) return;
            // TODO: android support requires a lot of additional work for content providers, paths and stuff
            _fileService.LoadFileIntoModel(_imageModel, file.LocalPath);
        }
        #endregion
    }
}
