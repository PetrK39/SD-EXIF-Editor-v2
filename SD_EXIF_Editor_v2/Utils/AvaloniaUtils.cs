using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using System;

namespace SD_EXIF_Editor_v2.Utils
{
    public static class AvaloniaUtils
    {
        public static IStorageProvider GetStorageProvider()
        {
            return GetTopLevel().StorageProvider;
        }
        public static IClipboard GetClipboard()
        {
            return GetTopLevel().Clipboard ?? throw new NotImplementedException("No clipboard for current application lifetime");
        }

        public static TopLevel GetTopLevel()
        {
            if (App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow!;
            }
            else if (App.Current.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                return TopLevel.GetTopLevel(singleViewPlatform.MainView)!;
            }

            throw new NotImplementedException();
        }
    }
}
