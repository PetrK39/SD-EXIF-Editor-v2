using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public ObservableCollection<ListItemTemplate> Items { get; }

        private readonly List<ListItemTemplate> _templates =
        [
            new ListItemTemplate(typeof(ViewMetadataViewModel), "InputCursorText", "View"),
            new ListItemTemplate(typeof(EditMetadataViewModel), "Pencil", "Edit"),
            new ListItemTemplate(typeof(SettingsViewModel), "GearFill", "Settings"),
        ];

        public MainViewModel()
        {
            Items = new ObservableCollection<ListItemTemplate>(_templates);

            SelectedListItem = Items.First(vm => vm.ModelType == typeof(ViewMetadataViewModel));
        }

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;

            var vm = Design.IsDesignMode
                ? Activator.CreateInstance(value.ModelType)
                : Ioc.Default.GetService(value.ModelType);

            if (vm is not ObservableObject oo) return;

            CurrentPage = oo;
        }

        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }
    }
}
