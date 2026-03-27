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
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Page
    {
        public Reg()
        {
            InitializeComponent();
        }

        private void Dannie_SelectionChanged(object sender, RoutedEventArgs e)
        {
            bool Check = (Login.Text.Length >= 5) && (Password.Text.Length >= 5);
            RegIn.IsEnabled = Check;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService?.GoBack();
            }
        }

        private void RegIn_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Text;
            if (Regist(login, password))
            {
                CUSTOMER customer = new CUSTOMER()
                {
                    LOGIN = login,
                    PASSWORD = password
                };

                User.curuser = customer;

                Core.Context.CUSTOMER.Add(customer);
                Core.Context.SaveChanges();

                MessageBox.Show("Регистрация успешна!");
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            //string login = Login.Text;
            //string password = Password.Text;

            //CUSTOMER existing = Core.Context.CUSTOMER.Where(c => c.LOGIN == login).FirstOrDefault();
            //if (existing != null)
            //{
            //    MessageBox.Show("Такой логин уже существует!");
            //    return;
            //}

            //CUSTOMER customer = new CUSTOMER()
            //{
            //    LOGIN = login,
            //    PASSWORD = password
            //};

            //User.curuser = customer;

            //Core.Context.CUSTOMER.Add(customer);
            //Core.Context.SaveChanges();

            //MessageBox.Show("Регистрация успешна!");
        }
        public bool Regist(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || login.Length < 5 || string.IsNullOrEmpty(password) || password.Length < 5)
            {
                MessageBox.Show("Неверный логин или пароль");
                return false;
            }

            using (var db = new CinemaEntities())
            {
                var user = db.CUSTOMER.AsNoTracking().FirstOrDefault(u => u.LOGIN == login && u.PASSWORD == password);

                if (user != null)
                {
                    MessageBox.Show("Пользователь с такими данными уже есть");
                    return false;
                }

                Login.Clear();
                Password.Clear();

                return true;
            }
        }
    }
}
