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
    /// Логика взаимодействия для MainPage.xaml (основной странице, на которой расоложены: боковое меню и Frame-ы)
    /// </summary>
    public partial class MainPage : Page
    {
        public static Frame CatalogFrame { get; set; }
        public MainPage()
        {
            InitializeComponent();
            CatalogFrame = frameCatalog;
            Loaded += MainPage_Loaded;
            LoadBtn();
        }

        /// <summary>
        /// При загрузке страницы проверяется авторизирован ли пользователь или нет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CheckAuthorization();
        }

        /// <summary>
        /// Проверка на то, авторизирован ли пользователь или нет. Если нет, то кидает на страницу авторизацции
        /// </summary>
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

        /// <summary>
        /// Скрытие окна авторизации и демонстрация интерфейса приложения
        /// </summary>
        private void ShowMainContent()
        {
            AuthFrame.Visibility = Visibility.Collapsed;
            MainTabControl.Visibility = Visibility.Visible;
            UpdateUIBasedOnRole();
        }

        /// <summary>
        /// Загрузка "интерфейса" в зависимости от роли пользователя
        /// </summary>
        private void UpdateUIBasedOnRole()
        {
            if (UserData.curUser == null) return;
            var catalogTab = TCatalog;
            var listsTab = TLists;
            var adminTab = TAdmin;
            var authorTab = TAuthor;
            var freezeTab = TFreeze;
            var profileTab = TProfile;


            if (UserData.curUser.IsFrozen)
            {
                freezeTab.Visibility = Visibility.Visible;
                adminTab.Visibility = Visibility.Collapsed;
                authorTab.Visibility = Visibility.Collapsed;
                return;
            }

            if (UserData.curUser.RoleID == 3)
            {
                adminTab.Visibility = Visibility.Visible;
                authorTab.Visibility = Visibility.Collapsed;
                freezeTab.Visibility = Visibility.Collapsed;
            }
            else if (UserData.curUser.RoleID == 2)
            {
                adminTab.Visibility = Visibility.Collapsed;
                authorTab.Visibility = Visibility.Visible;
                freezeTab.Visibility = Visibility.Collapsed;
            }
            else
            {
                adminTab.Visibility = Visibility.Collapsed;
                authorTab.Visibility = Visibility.Collapsed;
                freezeTab.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Поиск вкладок по Tooltip 
        /// </summary>
        /// <param name="toolTip"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Загрузка кнопки для смены учётки
        /// </summary>
        public void LoadBtn()
        {
            if (UserData.curUser == null)
            {
                LogoutBtn.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Опработка нажатия на кнопку "Выход"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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