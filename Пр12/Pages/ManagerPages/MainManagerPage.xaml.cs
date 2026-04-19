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

namespace Пр12.Pages.ManagerPages
{
    /// <summary>
    /// Логика взаимодействия для MainManagerPage.xaml
    /// </summary>
    public partial class MainManagerPage : Page
    {
        List<Service> ServicesForManager;
        List<UserService> UserServicesForManager;
        List<Order> OrdersForManager;
        List<Product> ProductsForManager;
        List<ProdCategory> ProdCategoriesForManager;
        List<Manufacturer> ManufacturersForManager;

        public MainManagerPage()
        {
            InitializeComponent();
            MainWindow.get_Btn_Back.Visibility = Visibility.Visible;

            ServicesForManager = Core.Context.Service.ToList();
            ServList.ItemsSource = ServicesForManager;

            UserServicesForManager = Core.Context.UserService.ToList();
            ZapisiList.ItemsSource = UserServicesForManager;

            OrdersForManager = Core.Context.Order.ToList();
            OrdersList.ItemsSource = OrdersForManager;

            ProductsForManager = Core.Context.Product.ToList();
            ProductsList.ItemsSource = ProductsForManager;

            ProdCategoriesForManager = Core.Context.ProdCategory.ToList();
            ProductTypesList.ItemsSource= ProdCategoriesForManager;

            ManufacturersForManager = Core.Context.Manufacturer.ToList();
            ManufacturersList.ItemsSource = ManufacturersForManager;
        }

        private void ChangeST_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddST_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CancelRecordBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RescheduleBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddRecordBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseOrderBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditProductBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FreezeProductBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnfreezeProductBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyDiscountBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProductTypeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeProductTypeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddManufacturerBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeManufacturerBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
