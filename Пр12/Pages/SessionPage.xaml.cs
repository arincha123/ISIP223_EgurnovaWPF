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


        public SessionPage( SESSIONS selectedseans )
        {
            DataContext = this;
            sEATs = Core.Context.SEAT.Where(s => s.id_hall == selectedseans.id_hall).ToList();

            FreeSeat = Core.Context.SEAT_IN_SESSION.Where(s => s.is_session == selectedseans.ID_SESSION && s.STATUS == true).ToList();


            InitializeComponent();
            
            SeatGrid.Width = sEATs.Max(s => s.NUMBER) * 30 + 70;

        }

        private void SeatGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var seat = SeatGrid.SelectedItem as SEAT;
            if (FreeSeat.FirstOrDefault(s => s.id_seat == s.id_seat) == null  )
            {
                return;
            }

        }

        private void OrderTicket_Click(object sender, RoutedEventArgs e)
        {
            MessageBox ord = MessageBox.Show("Вы выбрали следующее место:\nЗал: \nРяд: \nНомер: ", "Покупка билета", MessageBoxButton.YesNo, MessageBoxResult.Yes);
            if (ord == MessageBoxResult.Yes)
            {
                TICKET tICKET = new TICKET(
                    
                    


                    
                );
            }
            else if (ord == MessageBoxResult.No)
            {
                MessageBox.Show("Зачем ты нажал нет?(", "Не круто(", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;
            }
        }
    }
}
