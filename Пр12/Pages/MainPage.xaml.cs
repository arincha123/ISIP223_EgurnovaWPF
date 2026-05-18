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
        public static Frame MainFrame { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CheckAuthorization();
        }

        private void CheckAuthorization()
        {
            if (UserData.curUser == null)
            {
                var welcomeWindow = new AuthReg();
                welcomeWindow.Owner = Application.Current.MainWindow;

                if (welcomeWindow.ShowDialog() == true)
                {
                    UpdateUIBasedOnRole();
                }
                else
                {
                    Application.Current.MainWindow?.Close();
                }
            }
            else
            {
                UpdateUIBasedOnRole();
            }
        }

        private void OnLoginSuccess()
        {
            UpdateUIBasedOnRole();
        }

        private void UpdateUIBasedOnRole()
        {
            if (UserData.curUser == null) return;

            var adminTab = FindTabItemByToolTip("Администрирование");
            var authorTab = FindTabItemByToolTip("Страница автора");
            var freezeTab = FindTabItemByToolTip("Предупреждение о заморозке");
            var profileTab = FindTabItemByToolTip("Профиль");

            if (UserData.curUser.RoleID == 3)
            {
                if (adminTab != null) adminTab.Visibility = Visibility.Visible;
                if (authorTab != null) authorTab.Visibility = Visibility.Visible;
                if (freezeTab != null) freezeTab.Visibility = Visibility.Visible;
            }
            else if (UserData.curUser.RoleID == 2)
            {
                if (adminTab != null) adminTab.Visibility = Visibility.Collapsed;
                if (authorTab != null) authorTab.Visibility = Visibility.Visible;
                if (freezeTab != null) freezeTab.Visibility = Visibility.Visible;
            }
            else
            {
                if (adminTab != null) adminTab.Visibility = Visibility.Collapsed;
                if (authorTab != null) authorTab.Visibility = Visibility.Collapsed;
                if (freezeTab != null) freezeTab.Visibility = Visibility.Collapsed;
            }
        }

        private TabItem FindTabItemByToolTip(string toolTip)
        {
            foreach (TabItem item in MainTabControl.Items)
            {
                if (item.ToolTip?.ToString() == toolTip)
                {
                    return item;
                }
            }
            return null;
        }

        public void Logout()
        {
            UserData.curUser = null;
            CheckAuthorization();
        }
    }
}
