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
using System.Data;

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
        Binding bodyStyleTextBoxBinding = new Binding();
        Binding modelTextBoxBinding = new Binding();
        Binding makeTextBoxBinding = new Binding();

        CollectionViewSource customerViewSource;
        Binding firstnameTextBoxBinding = new Binding();
        Binding lastnameTextBoxBinding = new Binding();
        Binding purchaseDateTextBinding = new Binding();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            modelTextBoxBinding.Path = new PropertyPath("Model");
            makeTextBoxBinding.Path = new PropertyPath("Make");
            bodyStyleTextBoxBinding.Path = new PropertyPath("BodyStyle");
            modelTextBox.SetBinding(TextBox.TextProperty, modelTextBoxBinding);
            makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
            bodyStyleTextBox.SetBinding(TextBox.TextProperty, bodyStyleTextBoxBinding);

            firstnameTextBoxBinding.Path = new PropertyPath("FirstName");
            lastnameTextBoxBinding.Path = new PropertyPath("LastName");
            purchaseDateTextBinding.Path = new PropertyPath("PurchaseDate");
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstnameTextBoxBinding);
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastnameTextBoxBinding);
            purchaseDateDatePicker.SetBinding(DatePicker.TextProperty, purchaseDateTextBinding);
            



        }

        private void gbOperations_Click(object sender, RoutedEventArgs e)
        {
            Button SelectedButton = (Button)e.OriginalSource;
            Panel panel = (Panel)SelectedButton.Parent;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                if (B != SelectedButton)
                    B.IsEnabled = false;
            }
            gbActions.IsEnabled = true;
        }

        private void ReInitialize()
        {
            Panel panel = gbOperations.Content as Panel;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
            gbActions.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReInitialize();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            TabItem ti = tbCtrlAutoGeist.SelectedItem as TabItem;
            switch (ti.Header)
            {
                case "Cars":
                    SaveCars();
                    break;
                case "Customers":
                    SaveInventory();
                    break;
                case "Orders":
                    break;
            }
            ReInitialize();
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

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            TabItem ti = tbCtrlAutoGeist.SelectedItem as TabItem;
            switch (ti.Header)
            {
                case "Cars":
                    BindingOperations.ClearBinding(bodyStyleTextBox, TextBox.TextProperty);
                    BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
                    BindingOperations.ClearBinding(modelTextBox, TextBox.TextProperty);
                    bodyStyleTextBox.Text = "";
                    makeTextBox.Text = "";
                    modelTextBox.Text = "";
                    Keyboard.Focus(bodyStyleTextBox);
                    break;
                case "Customers":
                    BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
                    BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
                    firstNameTextBox.Text = "";
                    lastNameTextBox.Text = "";
                    Keyboard.Focus(firstNameTextBox);
                    break;
                case "Orders":
                    break;
            }

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            carViewSource.View.MoveCurrentToNext();
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            carViewSource.View.MoveCurrentToPrevious();
        }

        private void SaveCars()
        {
            Car car = null;
            if (action == ActionState.New)
            {
                try
                {
                    car = new Car()
                    {
                        Make = makeTextBox.Text.Trim(),
                        Model = modelTextBox.Text.Trim(),
                        BodyStyle = bodyStyleTextBox.Text.Trim(),
                    };
                    ctx.Cars.Add(car);
                    carViewSource.View.Refresh();
                    carViewSource.View.MoveCurrentTo(car);
                    ctx.SaveChanges();
                }
                // using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
                modelTextBox.SetBinding(TextBox.TextProperty, modelTextBoxBinding);
                bodyStyleTextBox.SetBinding(TextBox.TextProperty,
               bodyStyleTextBoxBinding);
            }
            else if (action == ActionState.Edit)
            {
                try
                {
                    car = (Car)carDataGrid.SelectedItem;
                    car.Make = makeTextBox.Text.Trim();
                    car.Model = modelTextBox.Text.Trim();
                    car.BodyStyle = bodyStyleTextBox.Text.Trim();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                carViewSource.View.Refresh();
                carViewSource.View.MoveCurrentTo(car);
                makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
                modelTextBox.SetBinding(TextBox.TextProperty, modelTextBoxBinding);
                bodyStyleTextBox.SetBinding(TextBox.TextProperty,
               bodyStyleTextBoxBinding);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    car = (Car)carDataGrid.SelectedItem;
                    ctx.Cars.Remove(car);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                carViewSource.View.Refresh();
                makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
                modelTextBox.SetBinding(TextBox.TextProperty, modelTextBoxBinding);
                bodyStyleTextBox.SetBinding(TextBox.TextProperty,
               bodyStyleTextBoxBinding);
            }
        }

        // save customers

        private void SaveInventory()
        {
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim(),
                       
                    };
                    ctx.Customers.Add(customer);

                    customerViewSource.View.Refresh();
                    customerViewSource.View.MoveCurrentTo(customer);
                    ctx.SaveChanges();
                }
                // using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                firstNameTextBox.SetBinding(TextBox.TextProperty, firstnameTextBoxBinding);
                lastNameTextBox.SetBinding(TextBox.TextProperty, lastnameTextBoxBinding);
                purchaseDateDatePicker.SetBinding(DatePicker.TextProperty, purchaseDateTextBinding);
            }
            else if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Customer)carDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    customer.PurchaseDate = DateTime.Today;

                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                customerViewSource.View.MoveCurrentTo(customer);
                firstNameTextBox.SetBinding(TextBox.TextProperty, firstnameTextBoxBinding);
                lastNameTextBox.SetBinding(TextBox.TextProperty, lastnameTextBoxBinding);
                purchaseDateDatePicker.SetBinding(DatePicker.TextProperty, purchaseDateTextBinding);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)carDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                customerViewSource.View.MoveCurrentTo(customer);
                firstNameTextBox.SetBinding(TextBox.TextProperty, firstnameTextBoxBinding);
                lastNameTextBox.SetBinding(TextBox.TextProperty, lastnameTextBoxBinding);
                purchaseDateDatePicker.SetBinding(DatePicker.TextProperty, purchaseDateTextBinding);
            }
        }

        private void tbCtrlAutoGeist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
