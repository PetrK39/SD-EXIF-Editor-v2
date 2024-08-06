using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SD_EXIF_Editor_v2.ViewModel
{
    class DesignTimeMainViewModel : PropertyChangedBase
    {
        private bool image_retrieved;
        private BitmapImage? bitmapImage;

        public string RawMetadata { get; set; }
        public SDMetadata Metadata { get; set; }
        public string FilePath => image.FilePath;
        public BitmapImage? BitmapImage
        {
            get
            {
                if (!image_retrieved) getImageAsync();
                return bitmapImage;
            }
        }

        private async Task getImageAsync()
        {
            image_retrieved = true;
            bitmapImage = await CreateImageAsync(FilePath).ConfigureAwait(true);
            RaisePropertyChanged(nameof(BitmapImage));
        }
        private async Task<BitmapImage?> CreateImageAsync(string filename)
        {
            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                try
                {
                    byte[] buffer = await ReadAllFileAsync(filename).ConfigureAwait(false);
                    MemoryStream ms = new MemoryStream(buffer);
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    image.Freeze();
                    return image;
                }
                catch
                {
                    return null;
                }
            }
            else return null;
        }
        private async Task<byte[]> ReadAllFileAsync(string filename)
        {
            try
            {
                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    byte[] buff = new byte[file.Length];
                    await file.ReadAsync(buff, 0, (int)file.Length).ConfigureAwait(false);
                    return buff;
                }
            }
            catch
            {
                return null;
            }
        }

        private readonly Image image;

        public DesignTimeMainViewModel()
        {
            image = new Image();
            image.LoadFromFilePath(@"C:\sdwebui\webui\outputs\txt2img-images\2024-08-02\00009-2710025737.png");

            RawMetadata = image.RawMetadata;
            Metadata = new MetadataParserService(null).ParseFromRawMetadata(RawMetadata);
        }
    }
}
