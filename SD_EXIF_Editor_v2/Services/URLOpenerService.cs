using SD_EXIF_Editor_v2.Services.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SD_EXIF_Editor_v2.Services
{
    public class UrlOpenerService : IUrlOpenerService
    {
        public async Task OpenUrlAsync(Uri url) => await OpenUrlAsync(url.ToString());
        public async Task OpenUrlAsync(string url)
        {
            if (OperatingSystem.IsAndroid())
            {
                await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            } else
            {
                var sInfo = new System.Diagnostics.ProcessStartInfo(url)
                {
                    UseShellExecute = true,
                };
                System.Diagnostics.Process.Start(sInfo);
            }
        }
    }
}
