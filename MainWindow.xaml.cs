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
using AutoGeistModel;
using System.Data.Entity;

namespace Topan_Sergiu_Lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum ActionState { New, Edit, Delete, Nothing };

    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoGeistEntitiesModel ctx = new AutoGeistEntitiesModel();
        CollectionViewSource carViewSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            carViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("carViewSource")));
            carViewSource.Source = ctx.Cars.Local;
            ctx.Cars.Load();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
