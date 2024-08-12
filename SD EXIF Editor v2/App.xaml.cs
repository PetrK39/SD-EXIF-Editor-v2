using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Properties;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Services.Interfaces;
using SD_EXIF_Editor_v2.View;
using SD_EXIF_Editor_v2.ViewModel;
using System.Diagnostics;
using System.IO;
using System.Runtime;
using System.Windows;

namespace SD_EXIF_Editor_v2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;

        public App()
        {
            var startupDir = new FileInfo(Process.GetCurrentProcess()!.MainModule!.FileName).Directory!.FullName;
            ProfileOptimization.SetProfileRoot(startupDir); // (a writable directory)
            ProfileOptimization.StartProfile("Startup.Profile");

            host = Host.CreateDefaultBuilder()
            .ConfigureServices((services) =>
            {
                services.AddTransient<IMessageService, MessageService>();
                services.AddTransient<IArgsParserService, ArgsParserService>();
                services.AddTransient<IMetadataParserService, MetadataParserService>();

                services.AddSingleton<ICivitService, CivitService>();
                services.AddSingleton<ISettingsService, SettingsService>();

                services.AddSingleton<Image>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<ViewViewModel>();
                services.AddSingleton<EditViewModel>();
                services.AddSingleton<SettingsViewModel>();

                services.AddSingleton<MainView>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel((LogLevel)Settings.Default.LogLevel);
                logging.AddNLog(context.Configuration.GetSection("NLog"));
            })
            .Build();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var mv = host.Services.GetRequiredService<MainView>();
                mv.ShowDialog();
            }
            finally
            {
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                host.StopAsync().GetAwaiter().GetResult();
                host.Dispose();
            }
            finally
            {
                //LogManager.Configuration.AllTargets.OfType<BufferingTargetWrapper>().ToList().ForEach(b => b.Flush(e => { }));

                base.OnExit(e);
            }
        }
    }

}
