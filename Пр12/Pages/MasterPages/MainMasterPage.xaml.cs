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

namespace Пр12.Pages.MasterPages
{
    /// <summary>
    /// Логика взаимодействия для MainMasterPage.xaml
    /// </summary>
    public partial class MainMasterPage : Page
    {
        List<UserService> MasterZapisi;
        List<MasterService> MasterServices;
        public MainMasterPage()
        {
            InitializeComponent();
            if (MainWindow.Current != null)
                MainWindow.Current.Btn_Back.Visibility = Visibility.Visible;

            MasterZapisi = Core.Context.UserService.ToList();
            Services.ItemsSource = MasterZapisi;

            MasterServices = Core.Context.MasterService.ToList();
            YourServices.ItemsSource = MasterServices;
        }

        private void Services_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UserService selectedServ = Services.SelectedItem as UserService;
            if (selectedServ != null) {
                ServiceInfo serviceInfo = new ServiceInfo(selectedServ);
                NavigationService.Navigate(serviceInfo);
            }
        }

        private void AddServ_Btn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DelServ_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
