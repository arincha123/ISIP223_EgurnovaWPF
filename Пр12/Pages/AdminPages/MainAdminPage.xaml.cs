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
            if (MainWindow.Current != null)
                MainWindow.Current.Btn_Back.Visibility = Visibility.Visible;

            LoadUsers();
        }

        private void LoadUsers()
        {
            UsersForAdmin = Core.Context.User.ToList();

            Clients1.ItemsSource = UsersForAdmin;
            Clients2.ItemsSource = UsersForAdmin;
        }

        private void ClearAddFields()
        {
            FirstName1.Text = "";
            LastName1.Text = "";
            MiddleName1.Text = "";
            Login1.Text = "";
            Password1.Text = "";
            Client1.IsChecked = true;
        }
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstName1.Text) ||
                string.IsNullOrWhiteSpace(LastName1.Text) ||
                string.IsNullOrWhiteSpace(Login1.Text) ||
                string.IsNullOrWhiteSpace(Password1.Text))
            {
                MessageBox.Show("Заполните все обязательные поля (Имя, Фамилия, Логин, Пароль)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var existingUser = Core.Context.User.FirstOrDefault(u => u.PhoneNumber == Login1.Text);

            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким номером телефона уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int roleId = 1;
            if (Master1.IsChecked == true)
                roleId = 2;
            else if (Manager1.IsChecked == true)
                roleId = 3;
            else if (Admin1.IsChecked == true)
                roleId = 4;

            User newUser = new User()
            {
                FirstName = FirstName1.Text,
                LastName = LastName1.Text,
                MiddleName = MiddleName1.Text ?? "",
                PhoneNumber = Login1.Text,
                Password = Password1.Text,
                RoleID = roleId
            };

            try
            {
                Core.Context.User.Add(newUser);
                Core.Context.SaveChanges();

                MessageBox.Show("Пользователь успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearAddFields();

                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DelPol_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber = Phone2.Text;

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Введите номер телефона пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = Core.Context.User.FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user == null)
            {
                MessageBox.Show("Пользователь с таким номером телефона не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DataOfUser.curuser != null && user.ID == DataOfUser.curuser.ID)
            {
                MessageBox.Show("Вы не можете удалить самого себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить пользователя {user.LastName} {user.FirstName}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Core.Context.User.Remove(user);
                    Core.Context.SaveChanges();

                    MessageBox.Show("Пользователь успешно удален!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    Phone2.Text = "";

                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChangePol_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber = Phone3.Text;

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Введите номер телефона пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int newRoleId = 0;
            if (Client3.IsChecked == true)
                newRoleId = 1;
            else if (Master3.IsChecked == true)
                newRoleId = 2;
            else if (Manager3.IsChecked == true)
                newRoleId = 3;
            else if (Admin3.IsChecked == true)
                newRoleId = 4;
            else
            {
                MessageBox.Show("Выберите новую роль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = Core.Context.User.FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user == null)
            {
                MessageBox.Show("Пользователь с таким номером телефона не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DataOfUser.curuser != null && user.ID == DataOfUser.curuser.ID)
            {
                MessageBox.Show("Вы не можете изменить роль самому себе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newRoleId == 4 && DataOfUser.curuser.RoleID != 4)
            {
                MessageBox.Show("Только администратор может назначать других администраторов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string roleName = "";
            switch (newRoleId)
            {
                case 1: roleName = "Клиента"; break;
                case 2: roleName = "Мастера"; break;
                case 3: roleName = "Менеджера"; break;
                case 4: roleName = "Администратора"; break;
            }

            var result = MessageBox.Show($"Вы действительно хотите изменить роль пользователя {user.LastName} {user.FirstName} на {roleName}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    user.RoleID = newRoleId;
                    Core.Context.SaveChanges();

                    MessageBox.Show("Роль пользователя успешно изменена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    Phone3.Text = "";
                    Client3.IsChecked = false;
                    Master3.IsChecked = false;
                    Manager3.IsChecked = false;
                    Admin3.IsChecked = false;

                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при изменении роли: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
