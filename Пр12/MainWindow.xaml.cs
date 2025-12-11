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

namespace Пр12
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public int Progresses = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        static public void MinusProgress()
        {
            Progresses--;
        }

        static public void PlusProgress()
        {
            Progresses++;
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            Progress.Value = Progresses;
        }
    }
}
