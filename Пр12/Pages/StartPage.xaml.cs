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
            MainWindow.Current.Btn_Back.Visibility = Visibility.Hidden;

            AllServices = Core.Context.MasterService.ToList();
            ShowServ.ItemsSource = AllServices;


        }

        private void MasterCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CategoryCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
    }
}
