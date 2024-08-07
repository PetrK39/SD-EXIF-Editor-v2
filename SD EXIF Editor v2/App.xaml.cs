using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
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
        public App()
        {
            host = Host.CreateDefaultBuilder()
            .ConfigureServices((services) =>
            {
                services.AddTransient<MessageService>();
                services.AddTransient<ArgsParserService>();
                services.AddTransient<MetadataParserService>();
                services.AddTransient<CivitService>();

                services.AddSingleton<Image>();

                services.AddSingleton<MainViewModel>();

                services.AddSingleton<MainView>();
            })
            .Build();
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var file = host.Services.GetRequiredService<ArgsParserService>().ParseArgs(e);

            host.Services.GetRequiredService<Image>().LoadFromFilePath(file.FullName);

            var mv = host.Services.GetRequiredService<MainView>();

            mv.ShowDialog();

            Shutdown();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            host.StopAsync();
            host.Dispose();

            base.OnExit(e);
        }
    }

}
