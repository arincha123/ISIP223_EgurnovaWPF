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
    /// Логика взаимодействия для YourTickets.xaml
    /// </summary>
    public partial class YourTickets : Page
    {
        private List<TICKET> tickets {  get; set; }



        public YourTickets()
        {
            InitializeComponent();
            CUSTOMER us = User.curuser;

            tickets = Core.Context.TICKET.Where(t => t.id_customer == us.ID_CUSTOMER).ToList();
            listoftickets.ItemsSource = tickets;
            UserInfo.DataContext = us;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService?.GoBack();
            }
        }
    }
}
