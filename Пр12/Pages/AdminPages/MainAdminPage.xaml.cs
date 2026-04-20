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

namespace Пр12.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для MainAdminPage.xaml
    /// </summary>
    public partial class MainAdminPage : Page
    {
        List<User> UsersForAdmin;
        public MainAdminPage()
        {
            InitializeComponent();
            MainWindow.get_Btn_Back.Visibility = Visibility.Visible;

            UsersForAdmin = Core.Context.User.ToList();
            Clients1.ItemsSource = UsersForAdmin;
            Clients2.ItemsSource = UsersForAdmin;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelPol_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangePol_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
