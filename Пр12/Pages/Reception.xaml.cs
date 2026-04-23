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

namespace Пр12.Pages
{
    /// <summary>
    /// Логика взаимодействия для Reception.xaml
    /// </summary>
    public partial class Reception : Page
    {
        private User _master;
        private Service _service;
        private List<Schedule> _schedules;
        private List<string> _payments;
        private List<PaymentMethod> _paymentMethods;

        public Reception(User master, Service service)
        {
            InitializeComponent();
            _master = master;
            _service = service;

            LoadData();

        }

        private void LoadData()
        {
            _paymentMethods = Core.Context.PaymentMethod.ToList();
            _payments = _paymentMethods.Select(p => p.Name).ToList();
            ComboBoxPaymentMethod.ItemsSource = _payments;
            StackService.DataContext = _service;
            StackMaster.DataContext = _master;
            _schedules = Core.Context.Schedule.Where(s => s.IsAvailable == true && s.MasterID == _master.ID && s.ServiceID == _service.ID).ToList();
            ListBoxAppointments.ItemsSource = _schedules;
        }

        private void BtnAppointment_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Schedule schedule = btn.DataContext as Schedule;
            if (schedule != null)
            {
                var service = Core.Context.Service.Find(schedule.ServiceID);
                string serviceName = service != null ? service.Name : "Неизвестная услуга";

                MessageBoxResult result = MessageBox.Show($"Хотите записаться на {serviceName} в {schedule.StartTime:HH:mm}?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string pay = ComboBoxPaymentMethod.SelectedItem.ToString();
                    PaymentMethod paymentMethod = _paymentMethods.FirstOrDefault(s => s.Name == pay);
                    try
                    {

                        UserService userService = new UserService()
                        {
                            UserID = DataOfUser.curuser.ID,
                            MasterID = _master.ID,
                            Date = schedule.StartTime,
                            ServiceID = _service.ID,
                            PaymentMethodID = paymentMethod.ID,
                            Comment = TxtBoxComment.Text,
                            ScheduleID = schedule.ID
                        };

                        Core.Context.UserService.Add(userService);

                        var sch = Core.Context.Schedule.First(s => s.ID == schedule.ID);
                        sch.IsAvailable = false;
                        Core.Context.SaveChanges();
                        MessageBox.Show("Запись подтверждена!");
                        NavigationService.Navigate(new StartPage());

                    }
                    catch
                    {
                        MessageBox.Show("Ошибка записи");
                    }
                }
                else return;
            }
        }



        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxAppointments.ItemsSource = _schedules.Where(s => s.StartTime.Date == Calendar.SelectedDate).ToList();
        }

        private void ListBoxAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}