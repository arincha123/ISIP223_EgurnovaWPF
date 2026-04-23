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
    public partial class ServiceInfo : Page
    {
        public UserService CurrentServ { get; set; }

        public ServiceInfo(UserService service)
        {
            InitializeComponent();
            CurrentServ = service;
            DataContext = this;
            LoadServData();
        }

        private void LoadServData()
        {
            if (CurrentServ != null)
            {
                LoadServ(CurrentServ.ID);
            }
        }

        private void LoadServ(int servId)
        {
            try
            {
                var service = Core.Context.UserService.Find(servId);

                if (service != null)
                {
                    service.Service = Core.Context.Service.Find(service.ServiceID);
                    service.Service.ServCategory = Core.Context.ServCategory.Find(service.Service.CategoryID);
                    service.User = Core.Context.User.Find(service.UserID);
                    service.Schedule = Core.Context.Schedule.Find(service.ScheduleID);

                    ServName.Text = service.Service.Name;
                    ServCat.Text = service.Service.ServCategory?.Name ?? "Не указана";
                    ServDate.Text = service.Date.ToString("dd.MM.yyyy");
                    ServTime.Text = $"{service.Schedule.StartTime:HH:mm} - {service.Schedule.EndTime:HH:mm}";
                    ServFIO.Text = $"{service.User.LastName} {service.User.FirstName} {service.User.MiddleName}";
                    ServPhone.Text = service.User.PhoneNumber;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseZapis_Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть запись?\nУслуга будет считаться выполненной.",
                "Закрытие записи", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var service = Core.Context.UserService.Find(CurrentServ.ID);

                    if (service != null)
                    {
                        var schedule = Core.Context.Schedule.Find(service.ScheduleID);
                        if (schedule != null)
                        {
                            schedule.IsAvailable = true;
                        }

                        Core.Context.UserService.Remove(service);
                        Core.Context.SaveChanges();

                        MessageBox.Show("Запись успешно закрыта!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        if (NavigationService.CanGoBack)
                            NavigationService.GoBack();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при закрытии записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}