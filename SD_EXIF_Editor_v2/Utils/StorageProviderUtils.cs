using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System;

namespace SD_EXIF_Editor_v2.Utils
{
    public static class StorageProviderUtils
    {
        public static IStorageProvider GetStorageProvider()
        {
            if (App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow!.StorageProvider;
            }
            else if (App.Current.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                return TopLevel.GetTopLevel(singleViewPlatform.MainView)!.StorageProvider;
            }

            throw new NotImplementedException();
        }
    }
}
