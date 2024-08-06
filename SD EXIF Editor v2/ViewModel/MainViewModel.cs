﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Service;
using SD_EXIF_Editor_v2.Utils;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SD_EXIF_Editor_v2.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly MetadataParserService _metadataParserService;

        private bool image_retrieved;
        private BitmapImage? bitmapImage;

        public string RawMetadata { get => image.RawMetadata!; set => image.RawMetadata = value; }
        public SDMetadata Metadata { get; set; }
        public string FilePath => image.FilePath!;
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
            OnPropertyChanged(nameof(bitmapImage));
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

        public MainViewModel(Image image, MetadataParserService metadataParserService)
        {
            _metadataParserService = metadataParserService;

            this.image = image;
            Metadata = _metadataParserService.ParseFromRawMetadata(RawMetadata);
        }

        #region Commands
        [RelayCommand]
        public void Save()
        {
            image.SaveChanges();

            ApplicationCommands.Close.Execute(null, null);
        }
        [RelayCommand]
        public void Copy(string parameter)
        {
            Debug.WriteLine($"Copied: {parameter}");
            Clipboard.SetDataObject(parameter);
        }
        [RelayCommand]
        public void Delete()
        {
            RawMetadata = "";
            Save();
        }
        #endregion
    }
}