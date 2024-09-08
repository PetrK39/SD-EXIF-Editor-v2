using System;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services.Interfaces
{
    public interface IUrlOpenerService
    {
        public Task OpenUrlAsync(Uri url);
        public Task OpenUrlAsync(string url);
    }
}
