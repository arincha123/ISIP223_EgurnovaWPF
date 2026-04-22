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
using System.Windows.Shapes;

namespace Пр12.Pages.Windows
{
    public partial class LogRegWindow : Window
    {
        public LogRegWindow()
        {
            InitializeComponent();
        }

        private void LogInFields_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool check = (WinPhone1.Text.Length >= 5) && (WinPassword1.Text.Length >= 1);
            LogIn_Btn.IsEnabled = check;
        }

        private void LogIn_Btn_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber = WinPhone1.Text;
            string password = WinPassword1.Text;

            User existing = Core.Context.User.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
            if (existing != null)
            {
                if (existing.Password == password)
                {
                    DataOfUser.curuser = existing;

                    MessageBox.Show($"Добро пожаловать, {existing.FirstName} {existing.LastName}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);


                    DialogResult = true;

                    //this.Close();

                }
                else
                {
                    MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пользователь с таким номером телефона не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegFields_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool check = (WinName2.Text.Length >= 1) &&
                        (WinSureName2.Text.Length >= 1) &&
                        (WinPhone2.Text.Length >= 5) &&
                        (WinPassword2.Text.Length >= 1);
            RegIn_Btn.IsEnabled = check;
        }

        private void RegIn_Btn_Click(object sender, RoutedEventArgs e)
        {
            string firstName = WinName2.Text;
            string lastName = WinSureName2.Text;
            string middleName = WinMiddleName2.Text;
            string phoneNumber = WinPhone2.Text;
            string password = WinPassword2.Text;

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User existing = Core.Context.User.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
            if (existing != null)
            {
                MessageBox.Show("Пользователь с таким номером телефона уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User newUser = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName ?? "",
                PhoneNumber = phoneNumber,
                Password = password,
                RoleID = 1
            };
            DataOfUser.curuser = newUser;

            Core.Context.User.Add(newUser);
            Core.Context.SaveChanges();

            MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
            //this.Close();
        }


    }
}