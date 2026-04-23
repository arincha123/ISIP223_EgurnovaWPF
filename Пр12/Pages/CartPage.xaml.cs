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
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        private Cart usercart;
        private List<ProductInCartViewModel> products = new List<ProductInCartViewModel>();
        public CartPage(Cart cart)
        {
            InitializeComponent();
            usercart = cart;
        }
        public void LoadData()
        {

            var productsInCart = Core.Context.ProductInCart.Where(p => p.CartID == usercart.ID).ToList();

            for (int i = 0; i < productsInCart.Count; i++)
            {
                var product = new ProductInCartViewModel
                {
                    ProductID = productsInCart[i].ProductID,
                    ProductInCartID = productsInCart[i].ID,
                    Name = productsInCart[i].Product.Name,
                    Price = productsInCart[i].Product.Cost - (productsInCart[i].Product.Cost * (decimal)(productsInCart[i].Product.Discount / 100)),
                    Quantity = productsInCart[i].Quantity,
                    Image = productsInCart[i].Product.Image

                };
                products.Add(product);
            }

            ListBoxProductsInCart.ItemsSource = products;
            UpdateTotalQuantity();

        }

        private void UpdateProductQuantity(ProductInCartViewModel productInCart, int newQuantity)
        {
            var prod = Core.Context.ProductInCart.FirstOrDefault(p => p.ProductID == productInCart.ProductID);
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
            usercart.TotalQuantity = totalQuantity;
            usercart.TotalAmount = totalPrice;
            DataOfUser.UserCart.TotalQuantity = totalQuantity;
            DataOfUser.UserCart.TotalAmount = totalPrice;
            TxtBlockCartQuantity.Text = totalQuantity.ToString();
            TxtBlcTotalPrice.Text = $"{totalPrice} Р";

            var cartBD = Core.Context.Cart.FirstOrDefault(c => c.ID == usercart.ID);
            if (cartBD != null)
            {
                cartBD.TotalQuantity = totalQuantity;
                cartBD.TotalAmount = totalPrice;
                Core.Context.SaveChanges();
            }

            if (totalQuantity == 0)
            {

                TxtBlockEmptyCart.Visibility = Visibility.Visible;
                ListBoxProductsInCart.Visibility = Visibility.Collapsed;
                StackTotalPrice.Visibility = Visibility.Collapsed;
            }
        }
        private void BtnMinusProd_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var selectedproduct = btn.DataContext as ProductInCartViewModel;

            UpdateProductQuantity(selectedproduct, selectedproduct.Quantity - 1);
        }

        private void BtnPlusProd_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var selectedproduct = btn.DataContext as ProductInCartViewModel;

            UpdateProductQuantity(selectedproduct, selectedproduct.Quantity + 1);
        }

        private void BtnDeleteProd_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var selectedproduct = btn.DataContext as ProductInCartViewModel;

            UpdateProductQuantity(selectedproduct, 0);
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            var wind = new WindowOrder(this);
            wind.Show();
            NavigationService.GoBack();
        }
    }
}

//namespace Пр12.Pages
//{
    
    
//        public void LoadData()
//        {

//            var productsInCart = Core.Context.ProductInCart.Where(p => p.CartID == usercart.ID).ToList();

//            for (int i = 0; i < productsInCart.Count; i++)
//            {
//                var product = new ProductInCartViewModel
//                {
//                    ProductID = productsInCart[i].ProductID,
//                    ProductInCartID = productsInCart[i].ID,
//                    Name = productsInCart[i].Product.Name,
//                    Price = productsInCart[i].Product.Cost - (productsInCart[i].Product.Cost * (decimal)(productsInCart[i].Product.Discount / 100)),
//                    Quantity = productsInCart[i].Quantity,
//                    Image = productsInCart[i].Product.Image

//                };
//                products.Add(product);
//            }

//            ListBoxProductsInCart.ItemsSource = products;
//            UpdateTotalQuantity();

//        }

//        private void UpdateProductQuantity(ProductInCartViewModel productInCart, int newQuantity)
//        {
//            var prod = Core.Context.ProductInCart.FirstOrDefault(p => p.ProductID == productInCart.ProductID);
//            if (newQuantity >= 1)
//            {
//                if (prod != null)
//                {
//                    prod.Quantity = newQuantity;
//                    Core.Context.SaveChanges();
//                }
//                productInCart.Quantity = newQuantity;
//            }
//            else
//            {

//                if (prod != null)
//                {
//                    Core.Context.ProductInCart.Remove(prod);
//                    Core.Context.SaveChanges();
//                }
//                products.Remove(productInCart);

//            }
//            ListBoxProductsInCart.ItemsSource = null;
//            ListBoxProductsInCart.ItemsSource = products;
//            UpdateTotalQuantity();
//        }

//        private void UpdateTotalQuantity()
//        {

//            int totalQuantity = products.Sum(u => u.Quantity);
//            decimal totalPrice = products.Sum(u => u.Price * u.Quantity);
//            usercart.TotalQuantity = totalQuantity;
//            usercart.TotalAmount = totalPrice;
//            DataOfUser.UserCart.TotalQuantity = totalQuantity;
//            DataOfUser.UserCart.TotalAmount = totalPrice;
//            TxtBlockCartQuantity.Text = totalQuantity.ToString();
//            TxtBlcTotalPrice.Text = $"{totalPrice} Р";

//            var cartBD = Core.Context.Cart.FirstOrDefault(c => c.ID == usercart.ID);
//            if (cartBD != null)
//            {
//                cartBD.TotalQuantity = totalQuantity;
//                cartBD.TotalAmount = totalPrice;
//                Core.Context.SaveChanges();
//            }

//            if (totalQuantity == 0)
//            {

//                TxtBlockEmptyCart.Visibility = Visibility.Visible;
//                ListBoxProductsInCart.Visibility = Visibility.Collapsed;
//                StackTotalPrice.Visibility = Visibility.Collapsed;
//            }
//        }
//        private void BtnMinusProd_Click(object sender, RoutedEventArgs e)
//        {
//            var btn = (Button)sender;
//            var selectedproduct = btn.DataContext as ProductInCartViewModel;

//            UpdateProductQuantity(selectedproduct, selectedproduct.Quantity - 1);
//        }

//        private void BtnPlusProd_Click(object sender, RoutedEventArgs e)
//        {
//            var btn = (Button)sender;
//            var selectedproduct = btn.DataContext as ProductInCartViewModel;

//            UpdateProductQuantity(selectedproduct, selectedproduct.Quantity + 1);
//        }

//        private void BtnDeleteProd_Click(object sender, RoutedEventArgs e)
//        {
//            var btn = (Button)sender;
//            var selectedproduct = btn.DataContext as ProductInCartViewModel;

//            UpdateProductQuantity(selectedproduct, 0);
//        }

//        private void BtnOrder_Click(object sender, RoutedEventArgs e)
//        {
//            var wind = new WindowOrder(this);
//            wind.Show();
//            NavigationService.GoBack();
//        }
//    }
//}