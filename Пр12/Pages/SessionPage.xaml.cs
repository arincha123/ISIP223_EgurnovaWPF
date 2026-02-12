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
    /// Логика взаимодействия для SessionPage.xaml
    /// </summary>
    public partial class SessionPage : Page
    {
        public List<SEAT> sEATs {  get; set; }


        public SessionPage( SESSIONS selectedseans )
        {
            DataContext = this;
            sEATs = Core.Context.SEAT.Where(s => s.id_hall == selectedseans.id_hall).ToList();


            InitializeComponent();
            
            SeatGrid.Width = sEATs.Max(s => s.NUMBER) * 30 + 70;
            //SeatGrid.ItemsSource = sEATs;

        }
    }
}
