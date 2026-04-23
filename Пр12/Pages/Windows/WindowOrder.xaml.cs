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
using System.Windows.Shapes;

namespace Пр12.Pages.Windows
{
    public partial class WindowOrder : Window
    {
        private List<string> _payments;
        private List<PaymentMethod> _paymentMethods;
        private CartPage _cartPage;

        public WindowOrder(CartPage cartPage)
        {
            InitializeComponent();
            _cartPage = cartPage;
            LoadDate();
        }

        private void LoadDate()
        {
            _paymentMethods = Core.Context.PaymentMethod.ToList();
            _payments = _paymentMethods.Select(p => p.Name).ToList();
            ComboBoxPayments.ItemsSource = _payments;
            ComboBoxPayments.SelectedIndex = 0;
            TxtBlockTotalPrice.Text = $"{DataOfUser.UserCart.TotalAmount} Р";
            var currentdate = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            for (int i = 0; i <= 7; i++)
            {
                dates.Add(currentdate.AddDays(i));
            }
            ListBoxDates.ItemsSource = dates;
        }

        private void BtnDate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            DateTime date = (DateTime)btn.DataContext;

            MessageBoxResult result = MessageBox.Show($"Забрать заказ {date:dd.MM.yyyy}?", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                List<ProductInCart> prodincart = Core.Context.ProductInCart.Where(p => p.CartID == DataOfUser.UserCart.ID).ToList();

                if (prodincart.Count == 0)
                {
                    MessageBox.Show("Корзина пуста");
                    return;
                }

                try
                {
                    Order order = new Order
                    {
                        Date = DateTime.Now,
                        TotalAmount = DataOfUser.UserCart.TotalAmount,
                        UserID = DataOfUser.curuser.ID,
                        DeliveryDate = date,
                        IsClosed = false
                    };
                    Core.Context.Order.Add(order);
                    Core.Context.SaveChanges();

                    foreach (ProductInCart prod in prodincart)
                    {
                        OrderItems orderItems = new OrderItems
                        {
                            OrderID = order.ID,
                            ProductID = prod.ProductID,
                            Quantity = prod.Quantity,
                        };
                        Core.Context.OrderItems.Add(orderItems);
                    }

                    Core.Context.ProductInCart.RemoveRange(prodincart);

                    DataOfUser.UserCart.TotalAmount = 0;
                    DataOfUser.UserCart.TotalQuantity = 0;

                    var cartBD = Core.Context.Cart.FirstOrDefault(c => c.ID == DataOfUser.UserCart.ID);
                    if (cartBD != null)
                    {
                        cartBD.TotalAmount = 0;
                        cartBD.TotalQuantity = 0;
                    }

                    Core.Context.SaveChanges();

                    MessageBox.Show("Заказ оформлен! Корзина очищена.");

                    if (_cartPage != null)
                    {
                        _cartPage.LoadData();
                    }

                    // ЗАКРЫВАЕМ ОКНО ЗАКАЗА
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                }
            }
        }
    }
}