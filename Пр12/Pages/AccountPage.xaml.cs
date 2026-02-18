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

namespace Пр12.Pages
{
    /// <summary>
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
        }

        private void noReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Reg());

            if (NavigationService?.CanGoForward == true)
            {
                NavigationService.GoForward();
            }
        }

        private void GoIn_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Text;

            CUSTOMER existing = Core.Context.CUSTOMER.Where(c => c.LOGIN == login && c.PASSWORD == password).FirstOrDefault();
            if (existing != null)
            {

                NavigationService.Navigate(new HomePage());

                Login.Text = "";
                Password.Text = "";
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }

            User.curuser = existing;

        }

        private void Danni_SelectionChanged(object sender, RoutedEventArgs e)
        {
            bool Check = (Login.Text.Length >= 5) && (Password.Text.Length >= 5);
            GoIn.IsEnabled = Check;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService?.GoBack();
            }
        }
    }
}
