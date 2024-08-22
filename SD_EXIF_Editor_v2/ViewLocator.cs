using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SD_EXIF_Editor_v2.ViewModels;
using System;

namespace SD_EXIF_Editor_v2
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is null)
                return null;

            var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}