using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.View;
using SD_EXIF_Editor_v2.ViewModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SD_EXIF_Editor_v2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 0 || e.Args.Length > 1) ExitWithMessage($"Usage: {Process.GetCurrentProcess().MainModule.ModuleName} %path_to_png_file");

            var file = new FileInfo(e.Args[0]);

            if (!file.Exists) ExitWithMessage($"File not exists \"{file.FullName}\"");
            if (file.Extension != ".png") ExitWithMessage("Only .png images are supported");

            var im = new Image(file.FullName);
            var mvm = new MainViewModel(im);
            var mv = new MainWindow(mvm);

            mv.ShowDialog();

            Shutdown();
        }
        private void ExitWithMessage(string message)
        {
            MessageBox.Show(message, "SD EXIF Editor", MessageBoxButton.OK);
            Environment.Exit(0);
        }
    }

}
