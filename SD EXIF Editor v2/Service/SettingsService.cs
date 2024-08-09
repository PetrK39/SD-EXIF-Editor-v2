using CommunityToolkit.Mvvm.ComponentModel;
using SD_EXIF_Editor_v2.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Service
{
    public enum NSFWLevels
    {
        None = 1,
        Soft = 2,
        Mature = 4,
        X = 8,
        xxx = 16
    }
    public class SettingsService : ObservableObject
    {
        public NSFWLevels NSFWLevel
        {
            get => (NSFWLevels)Settings.Default.NSFWLevel;
            set
            {
                Settings.Default.NSFWLevel = (int)value;
                OnPropertyChanged();
            }
        }

        public void Save()
        {
            Settings.Default.Save();
        }
    }
}