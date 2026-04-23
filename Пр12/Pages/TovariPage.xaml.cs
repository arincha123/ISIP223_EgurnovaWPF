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
    public partial class TovariPage : Page
    {
        List<Product> products;
        List<string> categors;
        List<string> manufs;
        bool IsFiltr = false;

        public TovariPage()
        {
            InitializeComponent();
            if (MainWindow.Current != null)
                MainWindow.Current.Btn_Back.Visibility = Visibility.Visible;

            LoadData();
        }

        private void LoadData()
        {
            products = Core.Context.Product.ToList();
            ListBoxProducts.ItemsSource = products;
            categors = Core.Context.ProdCategory.Select(p => p.Name).ToList();
            categors.Insert(0, "Все");
            ComboBoxFiltrProdCat.ItemsSource = categors;
            manufs = Core.Context.Manufacturer.Select(p => p.Name).ToList();
            manufs.Insert(0, "Все");
            ComboBoxFiltrProdMan.ItemsSource = manufs;
        }

        private void TxtBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxProducts.ItemsSource = products.Where(p => p.Name.ToLower().Contains(TxtBoxSearch.Text.ToLower())).ToList();

        }

        private void ComboBoxFiltrProdCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectCategory = (string)ComboBoxFiltrProdCat.SelectedItem;
            string selectManufacturer = (string)ComboBoxFiltrProdMan.SelectedItem;
            if (selectManufacturer == null || selectCategory == null) return;
            Filrt(selectCategory, selectManufacturer);


        }

        private void Filrt(string selectCategory, string selectManufacturer)
        {
            List<Product> prod = products;

            if (selectCategory == "Все" && selectManufacturer == "Все")
            {
                prod = prod;
            }
            else if (selectCategory == "Все" && selectManufacturer != "Все")
            {
                prod = prod.Where(p => p.Manufacturer.Name == selectManufacturer).ToList();
            }
            else if (selectCategory != "Все" && selectManufacturer == "Все")
            {
                prod = prod.Where(p => p.ProdCategory.Name == selectCategory).ToList();
            }
            else
            {
                prod = prod.Where(p => p.ProdCategory.Name == selectCategory && p.Manufacturer.Name == selectManufacturer).ToList();
            }
            if (IsFiltr)
                prod = prod.OrderByDescending(p => p.Rating).ToList();

            ListBoxProducts.ItemsSource = prod;



        }



        private void BtnSortRating_Click(object sender, RoutedEventArgs e)
        {
            IsFiltr = !IsFiltr;


            Filrt((string)ComboBoxFiltrProdCat.SelectedItem, (string)ComboBoxFiltrProdMan.SelectedItem);


        }

        private void CreateCart()
        {

            Cart cart = new Cart()
            {
                UserID = DataOfUser.curuser.ID,
                TotalAmount = 0,

            };
            Core.Context.Cart.Add(cart);
            Core.Context.SaveChanges();
            DataOfUser.UserCart = cart;
        }
        private void BtnAddCart_Click(object sender, RoutedEventArgs e)
        {
            if (!DataOfUser.isLoged)
            {
                MessageBox.Show("Нужна авторизация");
                return;
            }

            if (DataOfUser.UserCart == null)
            {
                CreateCart();
            }



            Button btn = (Button)sender;
            Product selectedProduct = (Product)btn.DataContext;

            var existprod = Core.Context.ProductInCart.FirstOrDefault(a => a.ProductID == selectedProduct.ID);

            if (existprod == null)
            {
                var pridcart = new ProductInCart
                {
                    CartID = DataOfUser.UserCart.ID,
                    ProductID = selectedProduct.ID,
                    Quantity = 1

                };
                Core.Context.ProductInCart.Add(pridcart);

            }
            else
            {
                existprod.Quantity += 1;
            }
            Core.Context.SaveChanges();



        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            if (!DataOfUser.isLoged)
            {
                MessageBox.Show("Нужна авторизация");

                return;
            }
            if (DataOfUser.UserCart == null)
            {
                CreateCart();
            }
            NavigationService.Navigate(new CartPage(DataOfUser.UserCart));

        }

        private void ListBoxProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedprod = ListBoxProducts.SelectedItem as Product;
            var wind = new WindowProduct(selectedprod);
            wind.ShowDialog();
        }
    }
}