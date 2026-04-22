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
        List<Order> OrdersHistory;

        public MainClientPage()
        {
            InitializeComponent();
            if (MainWindow.Current != null)
                MainWindow.Current.Btn_Back.Visibility = Visibility.Visible;

            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                if (DataOfUser.curuser == null)
                {
                    MessageBox.Show("Пользователь не авторизован!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ServicesForUser = Core.Context.UserService
                    .Where(us => us.UserID == DataOfUser.curuser.ID)
                    .ToList();

                foreach (var us in ServicesForUser)
                {
                    us.Service = Core.Context.Service.Find(us.ServiceID);
                    us.User1 = Core.Context.User.Find(us.MasterID); // Мастер
                    us.Schedule = Core.Context.Schedule.Find(us.ScheduleID);
                    us.PaymentMethod = Core.Context.PaymentMethod.Find(us.PaymentMethodID);
                }
                ZapisiList.ItemsSource = ServicesForUser;

                OrdersHistory = Core.Context.Order.Where(o => o.UserID == DataOfUser.curuser.ID).OrderByDescending(o => o.Date).ToList();

                OrdersHistoryList.ItemsSource = OrdersHistory;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}