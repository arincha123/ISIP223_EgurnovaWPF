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
            if (Auth(Login.Text, Password.Text))
            {
                CUSTOMER existing = new CUSTOMER()
                {
                    LOGIN = Login.Text,
                    PASSWORD = Password.Text
                };

                User.curuser = existing;
            }
        }

        public bool Auth(string log, string pass)
        {
            if (string.IsNullOrEmpty(log) && log.Length > 5 || string.IsNullOrEmpty(pass) && pass.Length > 5)
            {
                MessageBox.Show("Неверный логин или пароль");
            }

            using (var db = new CinemaEntities())
            {
                var user = db.CUSTOMER.AsNoTracking().FirstOrDefault(u => u.LOGIN == log && u.PASSWORD == pass);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден");
                    return false;
                }

                MessageBox.Show("Пользователь успешно найден");
                Login.Clear();
                Password.Clear();

                return true;
            }
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
