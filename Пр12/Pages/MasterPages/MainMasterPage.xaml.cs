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
    public partial class MainMasterPage : Page
    {
        List<UserService> MasterZapisi;
        List<MasterService> MasterServices;
        List<Service> AllServices;
        private User currentMaster;

        private MasterService selectedMyService;
        private Service selectedAllService;

        public MainMasterPage()
        {
            InitializeComponent();
            if (MainWindow.Current != null)
                MainWindow.Current.Btn_Back.Visibility = Visibility.Visible;

            currentMaster = DataOfUser.curuser;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (currentMaster == null)
                {
                    MessageBox.Show("Мастер не авторизован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MasterZapisi = Core.Context.UserService
                    .Where(us => us.MasterID == currentMaster.ID)
                    .ToList();

                foreach (var zapis in MasterZapisi)
                {
                    zapis.Service = Core.Context.Service.Find(zapis.ServiceID);
                    zapis.User = Core.Context.User.Find(zapis.UserID);
                    zapis.Schedule = Core.Context.Schedule.Find(zapis.ScheduleID);
                }
                Services.ItemsSource = MasterZapisi;

                MasterServices = Core.Context.MasterService
                    .Where(ms => ms.MasterID == currentMaster.ID)
                    .ToList();

                foreach (var ms in MasterServices)
                {
                    ms.Service = Core.Context.Service.Find(ms.ServiceID);
                }
                YourServices.ItemsSource = MasterServices;

                AllServices = Core.Context.Service.Include("ServCategory").ToList();
                AllServicesList.ItemsSource = AllServices;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void AddSelectedServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAllService == null)
            {
                MessageBox.Show("Выберите услугу из списка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var exists = Core.Context.MasterService
                    .Any(ms => ms.MasterID == currentMaster.ID && ms.ServiceID == selectedAllService.ID);

                if (exists)
                {
                    MessageBox.Show("Эта услуга уже есть в вашем списке!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LoadData();
                    return;
                }

                MasterService newMasterService = new MasterService()
                {
                    MasterID = currentMaster.ID,
                    ServiceID = selectedAllService.ID,
                    Price = 0
                };

                Core.Context.MasterService.Add(newMasterService);
                Core.Context.SaveChanges();

                MessageBox.Show($"Услуга '{selectedAllService.Name}' успешно добавлена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveSelectedServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMyService == null)
            {
                MessageBox.Show("Выберите услугу для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var hasActiveRecords = Core.Context.UserService
                .Any(us => us.MasterID == currentMaster.ID && us.ServiceID == selectedMyService.ServiceID);

            if (hasActiveRecords)
            {
                MessageBox.Show("Нельзя удалить услугу, на которую есть активные записи!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Удалить услугу '{selectedMyService.Service.Name}'?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var serviceToDelete = Core.Context.MasterService.Find(selectedMyService.ID);
                    if (serviceToDelete != null)
                    {
                        Core.Context.MasterService.Remove(serviceToDelete);
                        Core.Context.SaveChanges();

                        MessageBox.Show("Услуга удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddServ_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddServ.Text))
            {
                MessageBox.Show("Введите название услуги!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var service = Core.Context.Service.FirstOrDefault(s => s.Name == AddServ.Text.Trim());

                if (service == null)
                {
                    MessageBox.Show("Услуга с таким названием не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var exists = Core.Context.MasterService
                    .Any(ms => ms.MasterID == currentMaster.ID && ms.ServiceID == service.ID);

                if (exists)
                {
                    MessageBox.Show("Эта услуга уже есть в вашем списке!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    AddServ.Text = "";
                    return;
                }

                MasterService newMasterService = new MasterService()
                {
                    MasterID = currentMaster.ID,
                    ServiceID = service.ID,
                    Price = 0
                };

                Core.Context.MasterService.Add(newMasterService);
                Core.Context.SaveChanges();

                MessageBox.Show($"Услуга '{service.Name}' успешно добавлена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                AddServ.Text = "";
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DelServ_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DelServ.Text))
            {
                MessageBox.Show("Введите ID услуги для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(DelServ.Text, out int serviceId))
            {
                MessageBox.Show("Введите корректный ID!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var serviceToDelete = Core.Context.MasterService
                    .FirstOrDefault(ms => ms.ID == serviceId && ms.MasterID == currentMaster.ID);

                if (serviceToDelete == null)
                {
                    MessageBox.Show("Услуга с таким ID не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var hasActiveRecords = Core.Context.UserService
                    .Any(us => us.MasterID == currentMaster.ID && us.ServiceID == serviceToDelete.ServiceID);

                if (hasActiveRecords)
                {
                    MessageBox.Show("Нельзя удалить услугу, на которую есть активные записи!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = MessageBox.Show($"Удалить услугу '{serviceToDelete.Service.Name}'?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Core.Context.MasterService.Remove(serviceToDelete);
                    Core.Context.SaveChanges();

                    MessageBox.Show("Услуга удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    DelServ.Text = "";
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Services_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UserService selectedServ = Services.SelectedItem as UserService;
            if (selectedServ != null)
            {
                ServiceInfo serviceInfo = new ServiceInfo(selectedServ);
                NavigationService.Navigate(serviceInfo);
            }
        }

    }
}