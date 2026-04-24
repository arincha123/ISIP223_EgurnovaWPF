using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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

namespace Пр12.Pages.ManagerPages
{
    /// <summary>
    /// Логика взаимодействия для MainManagerPage.xaml
    /// </summary>
    public partial class MainManagerPage : Page
    {
        List<Service> ServicesForManager;
        List<ServCategory> ServicCategoriesForMAnager;
        List<UserService> UserServicesForManager;
        List<Order> OrdersForManager;
        List<Product> ProductsForManager;
        List<ProdCategory> ProdCategoriesForManager;
        List<Manufacturer> ManufacturersForManager;

        public MainManagerPage()
        {
            InitializeComponent();
            if (MainWindow.Current != null)
                MainWindow.Current.Btn_Back.Visibility = Visibility.Visible;


            LoadData();
        }

        private void LoadData()
        {
            ServicCategoriesForMAnager = Core.Context.ServCategory.ToList();
            ServCList.ItemsSource = ServicCategoriesForMAnager;

            UserServicesForManager = Core.Context.UserService.ToList();
            ZapisiList.ItemsSource = UserServicesForManager;

            OrdersForManager = Core.Context.Order.Where(o => !o.IsClosed).ToList();
            OrdersList.ItemsSource = OrdersForManager;



            ProductsForManager = Core.Context.Product.ToList();
            ProductsList.ItemsSource = ProductsForManager;

            ProdCategoriesForManager = Core.Context.ProdCategory.ToList();
            ProductTypesList.ItemsSource = ProdCategoriesForManager;

            ManufacturersForManager = Core.Context.Manufacturer.ToList();
            ManufacturersList.ItemsSource = ManufacturersForManager;

            var manufacturers = Core.Context.Manufacturer.ToList();
            ProductManufacturerCombo.ItemsSource = manufacturers;
            ProductManufacturerCombo.DisplayMemberPath = "Name";
            ProductManufacturerCombo.SelectedValuePath = "ID";

            var categories = Core.Context.ProdCategory.ToList();
            ProductTypeCombo.ItemsSource = categories;
            ProductTypeCombo.DisplayMemberPath = "Name";
            ProductTypeCombo.SelectedValuePath = "ID";


            var services = Core.Context.Service.Select(s => s.Name).ToList();
            NewServiceCombo.ItemsSource = services;

            var masters = Core.Context.User.Where(u => u.RoleID == 2).Select(s => s.FirstName + s.LastName + s.MiddleName).ToList();
            NewMasterCombo.ItemsSource = masters;

            //var times = List<String>()
            //NewTimeCombo.ItemsSource = 

        }

        private void ClearFields()
        {
            AddManufacturerNameBox.Text = "";
            ProductNameBox.Text = "";
            ProductPriceBox.Text = "";
            ProductDiscountBox.Text = "0";
            ProductManufacturerCombo.SelectedIndex = -1;
            ProductTypeCombo.SelectedIndex = -1;


        }

        //Услуги
        private void AddST_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddServType.Text))
            {
                MessageBox.Show("Введите название типа услуги!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var existing = Core.Context.ServCategory.FirstOrDefault(sc => sc.Name == AddServType.Text);
            if (existing != null)
            {
                MessageBox.Show("Такой тип услуги уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                ServCategory newCategory = new ServCategory() { 
                    Name = AddServType.Text 
                };

                Core.Context.ServCategory.Add(newCategory);
                Core.Context.SaveChanges();

                MessageBox.Show("Тип услуги успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                AddServType.Text = "";
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public ServCategory selectedST;
        private void ServСList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServCList.SelectedItem != null)
            {
                Service selectedService = ServCList.SelectedItem as Service;

                if (selectedService != null && selectedService.CategoryID > 0)
                {
                    selectedST = Core.Context.ServCategory.Find(selectedService.CategoryID);

                    if (selectedST != null)
                    {
                        int servicesCount = Core.Context.Service.Count(s => s.CategoryID == selectedST.ID);
                        bool hasOtherServices = servicesCount > 1;

                        if (hasOtherServices)
                        {
                            DeleteST_Btn.IsEnabled = false;
                            MessageBox.Show("Этот тип услуг используется в других услугах. Сначала удалите или измените услуги с этим типом!",
                                "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            DeleteST_Btn.IsEnabled = true;
                        }
                    }
                }
            }
            else
            {
                DeleteST_Btn.IsEnabled = false;
                selectedST = null;
            }
        }
 
        private void DeleteST_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DelServType.Text))
            {
                MessageBox.Show("Введите номер типа услуги для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(DelServType.Text, out int typeId))
            {
                MessageBox.Show("Введите корректный номер типа услуги!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var typeToDelete = Core.Context.ServCategory.Find(typeId);

            if (typeToDelete == null)
            {
                MessageBox.Show($"Тип услуги с номером {typeId} не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool hasServices = Core.Context.Service.Any(s => s.CategoryID == typeToDelete.ID);

            if (hasServices)
            {
                MessageBox.Show("Нельзя удалить тип услуги, который используется в услугах!\n" +
                    "Сначала удалите или измените все услуги с этим типом.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить тип услуги '{typeToDelete.Name}' (ID: {typeToDelete.ID})?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Core.Context.ServCategory.Remove(typeToDelete);
                    Core.Context.SaveChanges();

                    MessageBox.Show("Тип услуги успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    DelServType.Text = "";
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        //Записи
        //По идее, когда пишешь в TextBox SearchClientBox по имени или номеру телефона, то результаты появляются тут и тут взаимодействуешь с записью
        public User selectedClient;
        private void SearchClientBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchClientBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                SearchResultsList.ItemsSource = null;
                return;
            }

            try
            {
                var results = Core.Context.User
                    .Where(u => u.RoleID == 1 &&
                               (u.LastName.Contains(searchText) ||
                                u.FirstName.Contains(searchText) ||
                                (u.MiddleName != null && u.MiddleName.Contains(searchText)) ||
                                (u.PhoneNumber != null && u.PhoneNumber.Contains(searchText))))
                    .ToList();

                var displayResults = results.Select(u => new
                {
                    User = u,
                    FullNameWithPhone = $"{u.LastName} {u.FirstName} {u.MiddleName} - {u.PhoneNumber}"
                }).ToList();

                SearchResultsList.ItemsSource = displayResults;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SearchResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResultsList.SelectedItem != null)
            {
                dynamic selected = SearchResultsList.SelectedItem;
                selectedClient = selected.User;

                MessageBox.Show($"Выбран клиент: {selectedClient.LastName} {selectedClient.FirstName} {selectedClient.MiddleName}",
                    "Клиент выбран", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public UserService selectedRecord;
        private void CancelRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ZapisiList.SelectedItem == null)
            {
                MessageBox.Show("Выделите запись, которую хотите отменить!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            selectedRecord = (UserService)ZapisiList.SelectedItem;

            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите отменить запись клиента {selectedRecord.User.LastName} {selectedRecord.User.FirstName}?",
                "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var recordToDelete = Core.Context.UserService.Find(selectedRecord.ID);

                    if (recordToDelete != null)
                    {
                        var schedule = Core.Context.Schedule.Find(recordToDelete.ScheduleID);
                        if (schedule != null)
                        {
                            schedule.IsAvailable = true;
                        }

                        Core.Context.UserService.Remove(recordToDelete);
                        Core.Context.SaveChanges();

                        MessageBox.Show("Запись успешно отменена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отмене: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RescheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ZapisiList.SelectedItem == null)
            {
                MessageBox.Show("Выделите запись, которую хотите перенести!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            selectedRecord = (UserService)ZapisiList.SelectedItem;

            var dialog = new Window
            {
                Title = $"Перенос записи - {selectedRecord.User.LastName} {selectedRecord.User.FirstName}",
                Width = 400,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var stackPanel = new StackPanel { Margin = new Thickness(10) };

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Текущая запись: {selectedRecord.Date:dd.MM.yyyy} с {selectedRecord.Schedule.StartTime:HH:mm} до {selectedRecord.Schedule.EndTime:HH:mm}",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            stackPanel.Children.Add(new TextBlock { Text = "Выберите новую дату:", FontSize = 14, Margin = new Thickness(0, 0, 0, 5) });
            var newDatePicker = new DatePicker { Margin = new Thickness(0, 0, 0, 10) };
            stackPanel.Children.Add(newDatePicker);

            stackPanel.Children.Add(new TextBlock { Text = "Выберите новое время:", FontSize = 14, Margin = new Thickness(0, 0, 0, 5) });
            var newTimeCombo = new ComboBox { Margin = new Thickness(0, 0, 0, 10), DisplayMemberPath = "Time" };
            stackPanel.Children.Add(newTimeCombo);

            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            var okButton = new Button { Content = "Перенести", Width = 100, Height = 30, Margin = new Thickness(5) };
            var cancelButton = new Button { Content = "Отмена", Width = 100, Height = 30, Margin = new Thickness(5) };
            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);
            stackPanel.Children.Add(buttonPanel);

            dialog.Content = stackPanel;

            newDatePicker.SelectedDateChanged += (s, args) =>
            {
                if (newDatePicker.SelectedDate.HasValue)
                {
                    var availableTimes = Core.Context.Schedule
                        .Where(sch => sch.MasterID == selectedRecord.MasterID &&
                                     sch.StartTime.Date == newDatePicker.SelectedDate.Value.Date &&
                                     sch.IsAvailable == true)
                        .ToList();
                    newTimeCombo.ItemsSource = availableTimes;
                }
            };

            okButton.Click += (s, args) =>
            {
                if (newDatePicker.SelectedDate == null || newTimeCombo.SelectedItem == null)
                {
                    MessageBox.Show("Выберите новую дату и время!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    var recordToUpdate = Core.Context.UserService.Find(selectedRecord.ID);
                    Schedule newSchedule = (Schedule)newTimeCombo.SelectedItem;

                    if (recordToUpdate != null)
                    {
                        var oldSchedule = Core.Context.Schedule.Find(recordToUpdate.ScheduleID);
                        if (oldSchedule != null)
                        {
                            oldSchedule.IsAvailable = true;
                        }

                        recordToUpdate.ScheduleID = newSchedule.ID;
                        recordToUpdate.Date = newDatePicker.SelectedDate.Value;

                        newSchedule.IsAvailable = false;

                        Core.Context.SaveChanges();

                        MessageBox.Show("Запись успешно перенесена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        dialog.Close();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при переносе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            cancelButton.Click += (s, args) => dialog.Close();
            dialog.ShowDialog();
        }


        private void AddRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClient == null)
            {
                MessageBox.Show("Сначала выберите клиента из списка поиска!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewServiceCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите услугу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewTimeCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите время!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewMasterCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите мастера!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Service selectedService = (Service)NewServiceCombo.SelectedItem;
                User selectedMaster = (User)NewMasterCombo.SelectedItem;
                Schedule selectedSchedule = (Schedule)NewTimeCombo.SelectedItem;

                UserService newRecord = new UserService()
                {
                    UserID = selectedClient.ID,
                    MasterID = selectedMaster.ID,
                    ServiceID = selectedService.ID,
                    Date = NewDatePicker.SelectedDate.Value,
                    ScheduleID = selectedSchedule.ID,
                    PaymentMethodID = 1,
                    Comment = ""
                };

                selectedSchedule.IsAvailable = false;

                Core.Context.UserService.Add(newRecord);
                Core.Context.SaveChanges();

                MessageBox.Show("Клиент успешно записан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                selectedClient = null;
                SearchClientBox.Text = "";
                SearchResultsList.ItemsSource = null;
                NewServiceCombo.SelectedIndex = -1;
                NewDatePicker.SelectedDate = null;
                NewTimeCombo.ItemsSource = null;
                NewMasterCombo.SelectedIndex = -1;

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Заказы
        public Order selectedOrder;
        private void OrdersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOrder = OrdersList.SelectedItem as Order;
        }
        private void CloseOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Выделите заказ!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show($"Закрыть заказ №{selectedOrder.ID}?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var order = Core.Context.Order.Find(selectedOrder.ID);
                    if (order != null)
                    {
                        order.IsClosed = true;
                        Core.Context.SaveChanges();

                        MessageBox.Show("Заказ закрыт!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData(); 
                        selectedOrder = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        //Товары
        public Product selectedProduct;

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            {
                if (ProductsList.SelectedItem != null)
                {
                    selectedProduct = (Product)ProductsList.SelectedItem;

                    ProductNameBox.Text = selectedProduct.Name;
                    ProductPriceBox.Text = selectedProduct.Cost.ToString();
                    ProductDiscountBox.Text = selectedProduct.Discount.ToString();

                    if (selectedProduct.Manufacturer != null)
                    {
                        ProductManufacturerCombo.SelectedValue = selectedProduct.ManufacturerID;
                    }
                    else
                    {
                        var manufacturer = Core.Context.Manufacturer.Find(selectedProduct.ManufacturerID);
                        ProductManufacturerCombo.SelectedValue = manufacturer?.ID;
                    }

                    if (selectedProduct.ProdCategory != null)
                    {
                        ProductTypeCombo.SelectedValue = selectedProduct.CategoryID;
                    }
                    else
                    {
                        var category = Core.Context.ProdCategory.Find(selectedProduct.CategoryID);
                        ProductTypeCombo.SelectedValue = category?.ID;
                    }
                }
                else
                {
                    selectedProduct = null;
                    ClearFields();
                }
            }
        }
        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductNameBox.Text))
            {
                MessageBox.Show("Введите название товара!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(ProductPriceBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену товара!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ProductManufacturerCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите производителя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ProductTypeCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип товара!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(ProductDiscountBox.Text, out double discount))
            {
                discount = 0;
            }

            try
            {
                Product newProduct = new Product()
                {
                    Name = ProductNameBox.Text.Trim(),
                    Cost = price,
                    Discount = discount,
                    ManufacturerID = (int)ProductManufacturerCombo.SelectedValue,
                    CategoryID = (int)ProductTypeCombo.SelectedValue,
                    IsFrozen = false,
                    Description = "",
                    Rating = 0,
                    Image = "default.jpg"
                };

                Core.Context.Product.Add(newProduct);
                Core.Context.SaveChanges();

                MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearFields();
                LoadData(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void EditProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Выделите товар, который хотите изменить!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(ProductNameBox.Text))
            {
                MessageBox.Show("Введите название товара!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(ProductPriceBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену товара!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ProductManufacturerCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите производителя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ProductTypeCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип товара!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(ProductDiscountBox.Text, out double discount))
            {
                discount = 0;
            }

            try
            {
                var productToUpdate = Core.Context.Product.Find(selectedProduct.ID);

                if (productToUpdate != null)
                {
                    productToUpdate.Name = ProductNameBox.Text.Trim();
                    productToUpdate.Cost = price;
                    productToUpdate.Discount = discount;
                    productToUpdate.ManufacturerID = (int)ProductManufacturerCombo.SelectedValue;
                    productToUpdate.CategoryID = (int)ProductTypeCombo.SelectedValue;

                    Core.Context.SaveChanges();

                    MessageBox.Show("Товар успешно изменен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    ClearFields();
                    LoadData();
                    selectedProduct = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FreezeProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Выделите товар, который хотите заморозить!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedProduct.IsFrozen)
            {
                MessageBox.Show("Товар уже заморожен!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите заморозить товар '{selectedProduct.Name}'?\nЗамороженный товар не будет продаваться.",
                "Подтверждение заморозки", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var product = Core.Context.Product.Find(selectedProduct.ID);
                    if (product != null)
                    {
                        product.IsFrozen = true;
                        Core.Context.SaveChanges();

                        MessageBox.Show("Товар заморожен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при заморозке: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UnfreezeProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Выделите товар, который хотите разморозить!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!selectedProduct.IsFrozen)
            {
                MessageBox.Show("Товар не заморожен!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите разморозить товар '{selectedProduct.Name}'?",
                "Подтверждение разморозки", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var product = Core.Context.Product.Find(selectedProduct.ID);
                    if (product != null)
                    {
                        product.IsFrozen = false;
                        Core.Context.SaveChanges();

                        MessageBox.Show("Товар разморожен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при разморозке: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ApplyDiscountBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Выделите товар, для которого хотите изменить скидку!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(ProductDiscountBox.Text, out double discount))
            {
                MessageBox.Show("Введите корректный процент скидки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть от 0 до 100%!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var product = Core.Context.Product.Find(selectedProduct.ID);
                if (product != null)
                {
                    product.Discount = discount;
                    Core.Context.SaveChanges();

                    MessageBox.Show($"Скидка на товар '{product.Name}' изменена на {discount}%!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении скидки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DelProductBtn_Click(object sender, RoutedEventArgs e)
        {
            {
                if (selectedProduct == null)
                {
                    MessageBox.Show("Выделите товар, который хотите удалить!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool hasOrderItems = Core.Context.OrderItems.Any(oi => oi.ProductID == selectedProduct.ID);

                if (hasOrderItems)
                {
                    MessageBox.Show("Нельзя удалить товар, который присутствует в заказах!\n" +
                        "Сначала удалите заказы с этим товаром.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool hasCartItems = Core.Context.ProductInCart.Any(pc => pc.ProductID == selectedProduct.ID);

                if (hasCartItems)
                {
                    MessageBox.Show("Нельзя удалить товар, который находится в корзинах пользователей!\n" +
                        "Сначала удалите товар из корзин.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить товар '{selectedProduct.Name}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var productToDelete = Core.Context.Product.Find(selectedProduct.ID);

                        if (productToDelete != null)
                        {
                            Core.Context.Product.Remove(productToDelete);
                            Core.Context.SaveChanges();

                            MessageBox.Show("Товар успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                            ClearFields();
                            selectedProduct = null;

                            LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        //Типы товара
        private void AddProductTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AddProductTypeBox.Text))
            {
                MessageBox.Show("Заполните поле Название типа товара!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var existingType = Core.Context.ProdCategory.FirstOrDefault(pc => pc.Name == AddProductTypeBox.Text);

            if (existingType != null)
            {
                MessageBox.Show("Тип товара с таким названием уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ProdCategory newCategory = new ProdCategory()
            {
                Name = AddProductTypeBox.Text
            };

            try
            {
                Core.Context.ProdCategory.Add(newCategory);
                Core.Context.SaveChanges();
                MessageBox.Show("Тип товара успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                AddProductTypeBox.Text = "";
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DelProductTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DelProductTypeBox.Text))
            {
                MessageBox.Show("Введите ID типа товара для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(DelProductTypeBox.Text, out int typeId))
            {
                MessageBox.Show("Введите корректный ID типа товара!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var typeToDelete = Core.Context.ProdCategory.Find(typeId);

            if (typeToDelete == null)
            {
                MessageBox.Show($"Тип товара с ID {typeId} не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool hasProducts = Core.Context.Product.Any(p => p.CategoryID == typeToDelete.ID);

            if (hasProducts)
            {
                MessageBox.Show("Нельзя удалить тип товара, который используется в товарах!\n" +
                    "Сначала удалите или измените все товары с этим типом.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить тип товара '{typeToDelete.Name}' (ID: {typeToDelete.ID})?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Core.Context.ProdCategory.Remove(typeToDelete);
                    Core.Context.SaveChanges();

                    MessageBox.Show("Тип товара успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    DelProductTypeBox.Text = "";
                    LoadData(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        //Производители
        private void AddManufacturerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AddManufacturerNameBox.Text))
            {
                MessageBox.Show("Заполните все обязательные поля Название производителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var existingMan = Core.Context.Manufacturer.FirstOrDefault(m => m.Name == AddManufacturerNameBox.Text);

            if (existingMan != null)
            {
                MessageBox.Show("Производитель с таким названием уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Manufacturer manuf = new Manufacturer()
            {
                Name = AddManufacturerNameBox.Text
            };

            try
            {
                Core.Context.Manufacturer.Add(manuf);
                Core.Context.SaveChanges();
                MessageBox.Show("Производитель успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearFields();

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DelManufacturerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DelManufacturerNameBox.Text))
            {
                MessageBox.Show("Введите ID производителя для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(DelManufacturerNameBox.Text, out int manufacturerId))
            {
                MessageBox.Show("Введите корректный ID производителя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var manufacturerToDelete = Core.Context.Manufacturer.Find(manufacturerId);

            if (manufacturerToDelete == null)
            {
                MessageBox.Show($"Производитель с ID {manufacturerId} не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool hasProducts = Core.Context.Product.Any(p => p.ManufacturerID == manufacturerToDelete.ID);

            if (hasProducts)
            {
                MessageBox.Show("Нельзя удалить производителя, у которого есть товары!\n" +
                    "Сначала удалите или измените все товары этого производителя.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить производителя '{manufacturerToDelete.Name}' (ID: {manufacturerToDelete.ID})?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Core.Context.Manufacturer.Remove(manufacturerToDelete);
                    Core.Context.SaveChanges();

                    MessageBox.Show("Производитель успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    DelManufacturerNameBox.Text = "";
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}