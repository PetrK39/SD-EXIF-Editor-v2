using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using System;

namespace SD_EXIF_Editor_v2.Utils
{
    public static class AvaloniaUtils
    {
        public static TopLevel? TopLevel { get; set; }
        public static IStorageProvider? GetStorageProvider()
        {
            if (TopLevel is null) throw new ArgumentNullException(nameof(TopLevel));

            return TopLevel.StorageProvider;
        }
        public static IClipboard? GetClipboard()
        {
            if (TopLevel is null) throw new ArgumentNullException(nameof(TopLevel));

            return TopLevel.Clipboard ?? throw new NotImplementedException("No clipboard for current application lifetime");
        }
    }
}
