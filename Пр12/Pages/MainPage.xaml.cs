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
using Пр12.Windows;

namespace Пр12.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            LoadBtn();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CheckAuthorization();
        }

        private void CheckAuthorization()
        {
            if (UserData.curUser == null)
            {
                AuthFrame.Visibility = Visibility.Visible;
                MainTabControl.Visibility = Visibility.Collapsed;

                var authPage = new AuthRegPage();
                authPage.LoginSuccess = true;
                AuthFrame.Navigate(authPage);
            }
            else
            {
                ShowMainContent();
            }
        }

        private void ShowMainContent()
        {
            AuthFrame.Visibility = Visibility.Collapsed;
            MainTabControl.Visibility = Visibility.Visible;
            UpdateUIBasedOnRole();
        }

        private void UpdateUIBasedOnRole()
        {
            if (UserData.curUser == null) return;

            var catalogTab = FindTabItemByToolTip("Каталог книг");
            var listsTab = FindTabItemByToolTip("Списки книг");
            var adminTab = FindTabItemByToolTip("Администрирование");
            var authorTab = FindTabItemByToolTip("Страница автора");
            var freezeTab = FindTabItemByToolTip("Предупреждение о заморозке");
            var profileTab = FindTabItemByToolTip("Профиль");

            if (UserData.curUser.IsFrozen)
            {
                if (catalogTab != null) catalogTab.Visibility = Visibility.Visible;
                if (listsTab != null) listsTab.Visibility = Visibility.Visible;
                if (profileTab != null) profileTab.Visibility = Visibility.Visible;
                if (freezeTab != null) freezeTab.Visibility = Visibility.Visible;

                if (adminTab != null) adminTab.Visibility = Visibility.Collapsed;
                if (authorTab != null) authorTab.Visibility = Visibility.Collapsed;

                return;
            }

            if (UserData.curUser.RoleID == 3)
            {
                if (catalogTab != null) catalogTab.Visibility = Visibility.Visible;
                if (listsTab != null) listsTab.Visibility = Visibility.Visible;
                if (adminTab != null) adminTab.Visibility = Visibility.Visible;
                if (authorTab != null) authorTab.Visibility = Visibility.Collapsed;
                if (profileTab != null) profileTab.Visibility = Visibility.Visible;
                if (freezeTab != null) freezeTab.Visibility = Visibility.Collapsed;
            }
            else if (UserData.curUser.RoleID == 2)
            {
                if (catalogTab != null) catalogTab.Visibility = Visibility.Visible;
                if (listsTab != null) listsTab.Visibility = Visibility.Visible;
                if (adminTab != null) adminTab.Visibility = Visibility.Collapsed;
                if (authorTab != null) authorTab.Visibility = Visibility.Visible;
                if (profileTab != null) profileTab.Visibility = Visibility.Visible;
                if (freezeTab != null) freezeTab.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (catalogTab != null) catalogTab.Visibility = Visibility.Visible;
                if (listsTab != null) listsTab.Visibility = Visibility.Visible;
                if (adminTab != null) adminTab.Visibility = Visibility.Collapsed;
                if (authorTab != null) authorTab.Visibility = Visibility.Collapsed;
                if (profileTab != null) profileTab.Visibility = Visibility.Visible;
                if (freezeTab != null) freezeTab.Visibility = Visibility.Collapsed;
            }
        }

        private TabItem FindTabItemByToolTip(string toolTip)
        {
            if (MainTabControl == null) return null;

            foreach (TabItem item in MainTabControl.Items)
            {
                if (item.ToolTip?.ToString() == toolTip)
                {
                    return item;
                }
            }
            return null;
        }

        public void LoadBtn()
        {
            if (UserData.curUser == null)
            {
                LogoutBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                UserData.curUser = null;

                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.Content = new MainPage();
                }
            }
        }
    }
}