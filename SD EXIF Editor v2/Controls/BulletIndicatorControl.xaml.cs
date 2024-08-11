using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для BulletIndicatorControl.xaml
    /// </summary>
    public partial class BulletIndicatorControl : UserControl
    {


        public ICollectionView CollectionView
        {
            get { return (ICollectionView)GetValue(CollectionViewProperty); }
            set { SetValue(CollectionViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollectionViewProperty =
            DependencyProperty.Register("CollectionView", typeof(ICollectionView), typeof(BulletIndicatorControl), new PropertyMetadata(null));



        public ICommand GoToItemCommand
        {
            get { return (ICommand)GetValue(GoToItemCommandProperty); }
            set { SetValue(GoToItemCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GoToItemCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoToItemCommandProperty =
            DependencyProperty.Register("GoToItemCommand", typeof(ICommand), typeof(BulletIndicatorControl), new PropertyMetadata(null));



        public BulletIndicatorControl()
        {
            InitializeComponent();
        }
    }
}
