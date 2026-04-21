using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Пр12.Pages;
using Пр12.Pages.Windows;

namespace Пр12
{
    public partial class MainWindow : Window
    {
        public static MainWindow Current { get; private set; }

        public MainWindow()
        {
            InitializeComponent(); 
            Current = this;
            this.Btn_Back.Visibility = Visibility.Hidden;
        }


        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }
    }
}
