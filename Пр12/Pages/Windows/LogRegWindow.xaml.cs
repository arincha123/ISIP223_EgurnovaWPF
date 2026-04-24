using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Пр12.Pages;
using Пр12.Pages.AdminPages;
using Пр12.Pages.ClientPages;
using Пр12.Pages.ManagerPages;
using Пр12.Pages.MasterPages;

namespace Пр12.Pages.Windows
{
    public partial class LogRegWindow : Window
    {
        NavigationService navigationService;
        public LogRegWindow(NavigationService navigationService)
        {
            this.navigationService = navigationService;
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
                if (existing.IsFrozen == true)
                {
                    MessageBox.Show("Ваш аккаунт заблокирован! Обратитесь к администратору.",
                                  "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (existing.Password == password)
                {
                    DataOfUser.curuser = existing;

                    MessageBox.Show($"Добро пожаловать, {existing.FirstName} {existing.LastName}!",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (DataOfUser.curuser != null)
                    {
                        switch (DataOfUser.curuser.RoleID)
                        {
                            case 1:
                                navigationService.Navigate(new MainClientPage());
                                break;
                            case 2:
                                navigationService.Navigate(new MainMasterPage());
                                break;
                            case 3:
                                navigationService.Navigate(new MainManagerPage());
                                break;
                            case 4:
                                navigationService.Navigate(new MainAdminPage());
                                break;
                            default:
                                navigationService.Navigate(new MainClientPage());
                                break;
                        }
                    }
                    this.Close();

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
                MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
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


            this.Close();
        }


    }
}