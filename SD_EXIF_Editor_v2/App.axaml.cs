using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SD_EXIF_Editor_v2.Factories;
using SD_EXIF_Editor_v2.Factories.Interfaces;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Properties;
using SD_EXIF_Editor_v2.Services;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.Utils;
using SD_EXIF_Editor_v2.ViewModels;
using SD_EXIF_Editor_v2.ViewModels.Interfaces;
using SD_EXIF_Editor_v2.Views;

namespace SD_EXIF_Editor_v2
{
    public partial class App : Application
    {
        private IHost _host;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            _host = Host.CreateDefaultBuilder()
            .ConfigureServices((services) =>
            {
                // Services
                services.AddTransient<IMessageService, MessageService>();
                services.AddTransient<IArgsParserService, ArgsParserService>();
                services.AddTransient<IMetadataParserService, MetadataParserService>();
                services.AddTransient<IFileService, FileService>();

                services.AddTransient<ICivitItemViewModelFactory, CivitItemViewModelFactory>();

                services.AddSingleton<ICivitService, CivitService>();
                services.AddSingleton<ISettingsService, SettingsService>();

                // Http Client
                services.AddHttpClient();

                // Models
                services.AddSingleton<ImageModel>();

                // ViewModels
                services.AddSingleton<MainViewModel>();
                services.AddTransient<ICivitItemViewModel, CivitItemViewModel>();
                services.AddTransient<ViewMetadataViewModel>();
                services.AddTransient<EditMetadataViewModel>();
                services.AddTransient<SettingsViewModel>();

                // Views
                services.AddSingleton<MainWindow>();
                services.AddTransient<CivitItemView>();
                services.AddTransient<ViewMetadataView>();
                services.AddTransient<EditMetadataView>();
                services.AddTransient<SettingsView>();

                services.AddSingleton<MainView>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddNLog(context.Configuration.GetSection("NLog"));
            })
            .Build();

            _host.Start();

            Ioc.Default.ConfigureServices(_host.Services);

            var vm = _host.Services.GetRequiredService<MainViewModel>();

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