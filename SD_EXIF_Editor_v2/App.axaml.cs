using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SD_EXIF_Editor_v2.ViewModels;
using SD_EXIF_Editor_v2.Views;

namespace SD_EXIF_Editor_v2
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();

            // Services

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<ViewMetadataViewModel>();
            services.AddTransient<EditMetadataViewModel>();
            services.AddTransient<SettingsViewModel>();

            // Views
            services.AddSingleton<MainWindow>();
            services.AddTransient<ViewMetadataView>();
            services.AddTransient<EditMetadataView>();
            services.AddTransient<SettingsView>();

            var provider = services.BuildServiceProvider();

            Ioc.Default.ConfigureServices(provider);

            var vm = Ioc.Default.GetRequiredService<MainViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow(vm);
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView { DataContext = vm };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}