using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SD_EXIF_Editor_v2.Control
{
    /// <summary>
    /// Логика взаимодействия для PromptControl.xaml
    /// </summary>
    public partial class PromptControl : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PromptControl), new PropertyMetadata("?"));

        public string Prompt
        {
            get { return (string)GetValue(PromptProperty); }
            set { SetValue(PromptProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Prompt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PromptProperty =
            DependencyProperty.Register("Prompt", typeof(string), typeof(PromptControl), new PropertyMetadata("?"));



        public bool ShouldDisplayHeader
        {
            get { return (bool)GetValue(ShouldDisplayHeaderProperty); }
            set { SetValue(ShouldDisplayHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShouldDisplayHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShouldDisplayHeaderProperty =
            DependencyProperty.Register("ShouldDisplayHeader", typeof(bool), typeof(PromptControl), new PropertyMetadata(false));


        public bool ShouldDisplayPlaceholder
        {
            get { return (bool)GetValue(ShouldDisplayPlaceholderProperty); }
            set { SetValue(ShouldDisplayPlaceholderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShouldDisplayPlaceholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShouldDisplayPlaceholderProperty =
            DependencyProperty.Register("ShouldDisplayPlaceholder", typeof(bool), typeof(PromptControl), new PropertyMetadata(false));




        public ICommand CopyCommand
        {
            get { return (ICommand)GetValue(CopyCommandProperty); }
            set { SetValue(CopyCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CopyCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CopyCommandProperty =
            DependencyProperty.Register("CopyCommand", typeof(ICommand), typeof(PromptControl), new PropertyMetadata(null));

        public PromptControl()
        {
            InitializeComponent();
        }
    }
}
