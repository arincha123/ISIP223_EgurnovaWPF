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

namespace Пр12.Pages.ClientPages
{
    /// <summary>
    /// Логика взаимодействия для MainClientPage.xaml
    /// </summary>
    public partial class MainClientPage : Page
    {
        List<UserService> ServicesForUser;
        public MainClientPage()
        {
            InitializeComponent();
            MainWindow.get_Btn_Back.Visibility = Visibility.Visible;

            ServicesForUser = Core.Context.UserService.ToList();
            ZapisiList.ItemsSource = ServicesForUser;
        }
    }
}
