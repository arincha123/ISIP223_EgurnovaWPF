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
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Page
    {
        public Order()
        {
            InitializeComponent();
            Carta.ItemsSource = Cart.korzinka;
        }

        private void form_Click(object sender, RoutedEventArgs e)
        {
            CUSTOMER customer = new CUSTOMER()
            {
                NAME_CUSTOMER = FIO.Text.ToString(),
                EMAIL_CUSTOMER = EMAIL.Text.ToString(),
                ADRESS_CUSTOMER = ADRESS.Text.ToString()
            };
            int id_customer = Core.Context.CUSTOMER.Last().ID_CUSTOMER;
            MessageBox.Show($"Заказ оформлен на имя: {customer.NAME_CUSTOMER}\n На адрес: {customer.ADRESS_CUSTOMER}\n Чек придёт на почту: {customer.EMAIL_CUSTOMER}\n Сумма заказа: {id_customer}");

            Core.Context.CUSTOMER.Add(customer);
            Core.Context.SaveChanges();


            ORDER oRDER = new ORDER()
            {
                
                PRICE_ORDER = Cart.korzinka.Sum(a => a.PRICE_TOVAR),
                id_customer = id_customer
            };

            Core.Context.ORDER.Add(oRDER);
            Core.Context.SaveChanges();
            int id_or = Core.Context.ORDER.Last().ID_ORDER;

            foreach (var tovar in Cart.korzinka)
            {
                TOVAR
            }











        }
    }
}
