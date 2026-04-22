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
using Пр12.Pages.AdminPages;
using Пр12.Pages.ClientPages;
using Пр12.Pages.ManagerPages;
using Пр12.Pages.MasterPages;
using Пр12.Pages.Windows;

namespace Пр12.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        List<MasterService> AllServices;
        public StartPage()
        {
            InitializeComponent();
            MainWindow.get_Btn_Back.Visibility = Visibility.Hidden;

            AllServices = Core.Context.MasterService.ToList();
            ShowServ.ItemsSource = AllServices;

            var masters = Core.Context.User.Where(u => u.RoleID == 2).Select(u => u.FirstName + " " + u.LastName).ToList();
            masters.Insert(0, "Все мастера");
            MasterCBox.ItemsSource = masters;
            MasterCBox.SelectedIndex = 0;

            var categories = Core.Context.ServCategory.Select(c => c.Name).ToList();

            categories.Insert(0, "Все типы" );

            CategoryCBox.ItemsSource = categories;
            CategoryCBox.SelectedIndex = 0;
        }

        private void FilterAndShowServices()
        {
            var result = AllServices.ToList();

            string selectedMaster = MasterCBox.SelectedItem?.ToString();
            if (selectedMaster != null && selectedMaster != "Все мастера")
            {
                result = result.Where(s => (s.User.FirstName + " " + s.User.LastName) == selectedMaster).ToList();
            }

            string selectedCategory = CategoryCBox.SelectedItem?.ToString();
            if (selectedCategory != null && selectedCategory != "Все типы")
            {
                result = result.Where(s => s.Service.ServCategory.Name == selectedCategory).ToList();
            }

            ShowServ.ItemsSource = result;
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            Window lrwin = new LogRegWindow();
            lrwin.Show();

        }

        private void Tovari_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TovariPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FilterAndShowServices();
        }
    }
}
