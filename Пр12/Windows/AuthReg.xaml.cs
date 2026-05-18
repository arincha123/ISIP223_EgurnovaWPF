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

namespace Пр12.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthReg.xaml
    /// </summary>
    public partial class AuthReg : Window
    {
        public AuthReg()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var user = Core.Context.User.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    if (user.IsFrozen)
                    {
                        MessageBox.Show("Ваш аккаунт заморожен! Обратитесь к администратору.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    UserData.curUser = user;

                    MessageBox.Show($"Добро пожаловать, {user.Name}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = RegNameTextBox.Text.Trim();
            string login = RegLoginTextBox.Text.Trim();
            string email = RegEmailTextBox.Text.Trim();
            string password = RegPasswordBox.Password;
            string passwordConfirm = RegConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(passwordConfirm))
            {
                MessageBox.Show("Все поля обязательны для заполнения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Введите корректный email!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 3)
            {
                MessageBox.Show("Пароль должен содержать минимум 3 символа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != passwordConfirm)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (Core.Context.User.Any(u => u.Login == login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Core.Context.User.Any(u => u.Email == email))
                {
                    MessageBox.Show("Пользователь с таким email уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newUser = new User
                {
                    Name = name,
                    Login = login,
                    Email = email,
                    Password = password,
                    RoleID = 1,
                    IsFrozen = false
                };

                Core.Context.User.Add(newUser);
                Core.Context.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно! Теперь войдите.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                MainTabControl.SelectedIndex = 0;

                LoginTextBox.Text = login;
                PasswordBox.Password = "";

                RegNameTextBox.Text = "";
                RegLoginTextBox.Text = "";
                RegEmailTextBox.Text = "";
                RegPasswordBox.Password = "";
                RegConfirmPasswordBox.Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}