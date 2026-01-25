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
    /// Логика взаимодействия для AllTovari.xaml
    /// </summary>
    public partial class AllTovari : Page
    {
        public AllTovari()
        {
            InitializeComponent();

            List<TOVAR> tovari = Core.Context.TOVAR.ToList();
        }
    }
}
