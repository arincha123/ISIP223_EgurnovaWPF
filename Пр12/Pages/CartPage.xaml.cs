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
using Пр12.Pages.Windows;

namespace Пр12.Pages
{
    public partial class CartPage : Page
    {
        private Cart usercart;
        private List<ProductInCartViewModel> products = new List<ProductInCartViewModel>();

        public CartPage(Cart cart)
        {
            InitializeComponent();
            usercart = cart;
            LoadData();
        }

        public void LoadData()
        {
            products.Clear();

            var productsInCart = Core.Context.ProductInCart
                .Where(p => p.CartID == usercart.ID)
                .ToList();

            foreach (var item in productsInCart)
            {
                var product = Core.Context.Product.Find(item.ProductID);
                if (product != null)
                {
                    var productVM = new ProductInCartViewModel
                    {
                        ProductID = item.ProductID,
                        ProductInCartID = item.ID,
                        Name = product.Name,
                        Price = product.Cost - (product.Cost * (decimal)(product.Discount / 100)),
                        Quantity = item.Quantity,
                        Image = product.Image
                    };
                    products.Add(productVM);
                }
            }

            ListBoxProductsInCart.ItemsSource = products;
            UpdateTotalQuantity();
        }

        private void UpdateProductQuantity(ProductInCartViewModel productInCart, int newQuantity)
        {
            var prod = Core.Context.ProductInCart
                .FirstOrDefault(p => p.ProductID == productInCart.ProductID && p.CartID == usercart.ID);

            if (newQuantity >= 1)
            {
                if (prod != null)
                {
                    prod.Quantity = newQuantity;
                    Core.Context.SaveChanges();
                }
                productInCart.Quantity = newQuantity;
            }
            else
            {
                if (prod != null)
                {
                    Core.Context.ProductInCart.Remove(prod);
                    Core.Context.SaveChanges();
                }
                products.Remove(productInCart);
            }

            ListBoxProductsInCart.ItemsSource = null;
            ListBoxProductsInCart.ItemsSource = products;
            UpdateTotalQuantity();
        }

        private void UpdateTotalQuantity()
        {
            int totalQuantity = products.Sum(u => u.Quantity);
            decimal totalPrice = products.Sum(u => u.Price * u.Quantity);

            TxtBlockCartQuantity.Text = totalQuantity.ToString();
            TxtBlcTotalPrice.Text = $"{totalPrice:F2} ₽";

            var cartBD = Core.Context.Cart.FirstOrDefault(c => c.ID == usercart.ID);
            if (cartBD != null)
            {
                cartBD.TotalQuantity = totalQuantity;
                cartBD.TotalAmount = totalPrice;
                Core.Context.SaveChanges();
            }

            if (DataOfUser.UserCart != null)
            {
                DataOfUser.UserCart.TotalQuantity = totalQuantity;
                DataOfUser.UserCart.TotalAmount = totalPrice;
            }

            if (totalQuantity == 0)
            {
                TxtBlockEmptyCart.Visibility = Visibility.Visible;
                ListBoxProductsInCart.Visibility = Visibility.Collapsed;
                StackTotalPrice.Visibility = Visibility.Collapsed;
                TxtBlockCartQuantity.Text = "0";
            }
            else
            {
                TxtBlockEmptyCart.Visibility = Visibility.Collapsed;
                ListBoxProductsInCart.Visibility = Visibility.Visible;
                StackTotalPrice.Visibility = Visibility.Visible;
            }
        }

        private void BtnMinusProd_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var selectedproduct = btn.DataContext as ProductInCartViewModel;
            if (selectedproduct != null)
            {
                UpdateProductQuantity(selectedproduct, selectedproduct.Quantity - 1);
            }
        }

        private void BtnPlusProd_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var selectedproduct = btn.DataContext as ProductInCartViewModel;
            if (selectedproduct != null)
            {
                UpdateProductQuantity(selectedproduct, selectedproduct.Quantity + 1);
            }
        }

        private void BtnDeleteProd_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var selectedproduct = btn.DataContext as ProductInCartViewModel;
            if (selectedproduct != null)
            {
                UpdateProductQuantity(selectedproduct, 0);
            }
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (products.Count == 0)
            {
                MessageBox.Show("Корзина пуста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var wind = new WindowOrder(this);
            wind.Owner = Window.GetWindow(this);
            wind.ShowDialog();
            LoadData();
        }
    }
}