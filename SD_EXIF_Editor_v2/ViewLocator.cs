using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using SD_EXIF_Editor_v2.ViewModels;
using SD_EXIF_Editor_v2.Views;
using System;
using System.Collections.Generic;

namespace SD_EXIF_Editor_v2
{
    public class ViewLocator : IDataTemplate
    {
        private readonly Dictionary<Type, Func<Control?>> _locator = [];

        public ViewLocator()
        {
            RegisterViewFactory<MainViewModel, MainWindow>();
            RegisterViewFactory<ViewMetadataViewModel, ViewMetadataView>();
            RegisterViewFactory<EditMetadataViewModel, EditMetadataView>();
            RegisterViewFactory<SettingsViewModel, SettingsView>();
        }
        public Control Build(object? data)
        {
            if (data is null)
                return new TextBlock { Text = "No VM provided" };

            _locator.TryGetValue(data.GetType(), out var factory);

            return factory?.Invoke() ?? new TextBlock { Text = $"VM Not Registered: {data.GetType()}" };
        }

        public bool Match(object? data)
        {
            return data is ObservableObject;
        }

        private void RegisterViewFactory<TViewModel, TView>()
        where TViewModel : class
        where TView : Control
        => _locator.Add(
            typeof(TViewModel),
            Design.IsDesignMode
                ? Activator.CreateInstance<TView>
                : Ioc.Default.GetService<TView>);
    }
}