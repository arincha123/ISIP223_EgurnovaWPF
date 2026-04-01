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
using System.Windows.Threading;

namespace Пр12.Pages
{
    /// <summary>
    /// Логика взаимодействия для WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public WelcomePage()
        {
            InitializeComponent();
            
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
            
            
        }

        private void StartBtm_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            NavigationService.Navigate(new GamePage());
        }
        

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            StartBtm.Margin = new Thickness(r.Next((int)MainWindow.GetWindow(this).ActualWidth-173), r.Next((int)Window.GetWindow(this).ActualHeight - 33), 0, 0);
        }
    }
}
