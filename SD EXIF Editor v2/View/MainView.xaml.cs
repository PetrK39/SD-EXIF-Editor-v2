using SD_EXIF_Editor_v2.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SD_EXIF_Editor_v2.View
{
    public partial class MainView : Window
    {
        public MainView(MainViewModel mvm)
        {
            InitializeComponent();
            DataContext = mvm;

            ApplicationCommands.Close.InputGestures.Add(new KeyGesture(Key.Escape));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseCommandExecuted));
        }

        private void CloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
