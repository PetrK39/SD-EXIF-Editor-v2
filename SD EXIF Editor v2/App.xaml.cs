using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.View;
using SD_EXIF_Editor_v2.ViewModel;
using System.Windows;

namespace SD_EXIF_Editor_v2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
        private ILoggingService loggingService;
        public App()
        {
            host = Host.CreateDefaultBuilder()
            .ConfigureServices((services) =>
            {
                services.AddSingleton(provider => new NLogService(LogManager.GetCurrentClassLogger()));

                services.AddTransient<MessageService>();
                services.AddTransient<ArgsParserService>();
                services.AddTransient<MetadataParserService>();
                services.AddTransient<CivitService>();

                services.AddSingleton<SettingsService>();

                services.AddSingleton<Image>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<ViewViewModel>();
                services.AddSingleton<EditViewModel>();
                services.AddSingleton<SettingsViewModel>();

                services.AddSingleton<MainView>();
            })
            .Build();

            loggingService = host.Services.GetRequiredService<ILoggingService>();
            loggingService.Trace("App initialized.");
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            loggingService.Trace("Application starting up.");

            try
            {
                var file = host.Services.GetRequiredService<ArgsParserService>().ParseArgs(e);

                host.Services.GetRequiredService<Image>().LoadFromFilePath(file.FullName);

                var mv = host.Services.GetRequiredService<MainView>();

                loggingService.Trace("Application started up successfully, displaying main window.");

                mv.ShowDialog();

                loggingService.Trace("Main window is closed, shutting down.");
            }
            catch (Exception ex)
            {
                loggingService.Fatal($"Unhandled exception: {ex.Message}", ex);
                throw;
            }
            finally
            {
                Shutdown();
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            loggingService.Trace("Application exiting.");

            try
            {
                host.StopAsync().GetAwaiter().GetResult();
                host.Dispose();
            }
            catch (Exception ex)
            {
                loggingService.Fatal($"Unhandled exception during application exit: {ex.Message}", ex);
                throw;
            }
            finally
            {
                loggingService.Trace("Application exit completed.");
            }

            base.OnExit(e);
        }
    }

}
