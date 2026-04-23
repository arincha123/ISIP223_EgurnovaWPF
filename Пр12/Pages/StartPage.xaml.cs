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
            if (MainWindow.Current != null)
                MainWindow.Current.Btn_Back.Visibility = Visibility.Hidden;
            AccountIn.Visibility = Visibility.Hidden;
            AccountOut.Visibility = Visibility.Hidden;

            CheckAuthorization();

            AllServices = Core.Context.MasterService.ToList();
            ShowServ.ItemsSource = AllServices;

            var masters = Core.Context.User.Where(u => u.RoleID == 2).Select(u => u.FirstName + " " + u.LastName).ToList();
            masters.Insert(0, "Все мастера");
            MasterCBox.ItemsSource = masters;
            MasterCBox.SelectedIndex = 0;

            var categories = Core.Context.ServCategory.Select(c => c.Name).ToList();

            categories.Insert(0, "Все типы");

            CategoryCBox.ItemsSource = categories;
            CategoryCBox.SelectedIndex = 0;
        }

        private void CheckAuthorization()
        {
            if (DataOfUser.isLoged)
            {
                LogIn.Visibility = Visibility.Hidden;
                AccountIn.Visibility = Visibility.Visible;
                AccountOut.Visibility = Visibility.Visible;
            }
            else
            {
                LogIn.Visibility = Visibility.Visible;
                AccountIn.Visibility = Visibility.Hidden;
                AccountOut.Visibility = Visibility.Hidden;
            }
        }

        private void AccountIn_Click(object sender, RoutedEventArgs e)
        {
            if (DataOfUser.curuser != null)
            {
                switch (DataOfUser.curuser.RoleID)
                {
                    case 1:
                        NavigationService.Navigate(new MainClientPage());
                        break;
                    case 2:
                        NavigationService.Navigate(new MainMasterPage());
                        break;
                    case 3:
                        NavigationService.Navigate(new MainManagerPage());
                        break;
                    case 4:
                        NavigationService.Navigate(new MainAdminPage());
                        break;
                    default:
                        NavigationService.Navigate(new MainClientPage());
                        break;
                }
            }
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            Window lrwin = new LogRegWindow();
            var result = lrwin.ShowDialog();
            if (result == true)
            {
                CheckAuthorization();
            }
        }

        private void Tovari_Click(object sender, RoutedEventArgs e)
        {
            if (DataOfUser.curuser == null || !DataOfUser.isLoged)
            {
                MessageBox.Show("Войдите в аккаунт, чтобы просматривать товары!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DataOfUser.curuser.Role != null)
            {
                NavigationService.Navigate(new TovariPage());
            }
            else
            {
                MessageBox.Show("Товары могут просматривать только клиенты!", "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FilterAndShowServices();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!DataOfUser.isLoged || DataOfUser.curuser == null)
            {
                MessageBox.Show("Войдите в аккаунт!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DataOfUser.curuser.Role.Name != "Клиент")
            {
                MessageBox.Show("Записываться на услуги могут только клиенты!", "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Button btn = (Button)sender;
            MasterService selectedItem = btn.DataContext as MasterService;

            if (selectedItem == null || selectedItem.Service == null || selectedItem.User == null)
            {
                MessageBox.Show("Выберите услугу из списка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NavigationService.Navigate(new ChoosePage(selectedItem.Service.ServCategory, selectedItem.User, selectedItem.Service));
        }

        private void AccountOut_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите выйти из аккаунта?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DataOfUser.curuser = null;
                CheckAuthorization();
                MessageBox.Show("Вы успешно вышли из аккаунта!", "Выход", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}