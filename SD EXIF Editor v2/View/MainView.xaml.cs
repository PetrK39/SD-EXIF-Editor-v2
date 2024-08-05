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

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseCommandExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, mvm.SaveCommandExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, mvm.CopyCommandExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, mvm.DeleteCommandExecuted));

            TextBox.TextChanged += TextBox_TextChanged;
            TextBox.Focus();
        }

        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            TextBox.TextChanged -= TextBox_TextChanged;
            TextBox.CaretIndex = TextBox.Text.Length;
        }

        private void CloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
