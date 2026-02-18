using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для SessionPage.xaml
    /// </summary>
    public partial class SessionPage : Page
    {
        public List<SEAT> sEATs {  get; set; }
        public List<SEAT_IN_SESSION> FreeSeat {  get; set; }
        public SEAT selectedseat { get; set; }
        public SESSIONS selses { get; set; }




        public SessionPage( SESSIONS selectedseans )
        {
            selses = selectedseans;
            DataContext = this;
            sEATs = Core.Context.SEAT.Where(s => s.id_hall == selectedseans.id_hall).ToList();

            FreeSeat = Core.Context.SEAT_IN_SESSION.Where(s => s.id_session == selectedseans.ID_SESSION && s.STATUS == true).ToList();


            InitializeComponent();
            
            SeatGrid.Width = sEATs.Max(s => s.NUMBER) * 30 + 70;

        }

        private void SeatGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedseat = SeatGrid.SelectedItem as SEAT;
            if (FreeSeat.FirstOrDefault(s => s.id_seat == s.id_seat) == null  )
            {
                return;
            }

        }

        private void OrderTicket_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult ord = MessageBox.Show($"Вы выбрали следующее место:\nЗал: {selectedseat.HALL.HALL_NAME}\nРяд: {selectedseat.ROW}\nНомер: {selectedseat.NUMBER}", "Покупка билета", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
            if (ord == MessageBoxResult.Yes)
            {
                TICKET tICKET = new TICKET()
                {
                    id_session = selses.ID_SESSION,
                    id_customer = User.curuser.ID_CUSTOMER,
                    id_seat = selectedseat.ID_SEAT,
                    TOTAL_PRICE = selectedseat.HALL.HALL_RATING.TICKET_PRICE
                };


                MessageBox.Show("Вы успешно приобрели билет");

                SEAT_IN_SESSION setinsession = Core.Context.SEAT_IN_SESSION.FirstOrDefault(s => s.id_seat == selectedseat.ID_SEAT);

                setinsession.STATUS = false;

                Core.Context.TICKET.Add(tICKET);
                Core.Context.SaveChanges();


                if (NavigationService?.CanGoBack == true)
                {
                    NavigationService?.GoBack();
                }
            }
            else if (ord == MessageBoxResult.No)
            {
                MessageBox.Show("выключи с позором", "Не круто(", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
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
