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
    /// Логика взаимодействия для Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        private int mc, vznos, sc;
        private double fpp, i;
        public Page4()
        {
            InitializeComponent();
        }
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PlusProgress();
            NavigationService.Navigate(new Page5());

            if (NavigationService?.CanGoForward == true)
            {
                NavigationService.GoForward();
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                MainWindow.MinusProgress();
                NavigationService?.GoBack();
            }
        }

        private void FPText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox fpt = (TextBox)sender;
            string text = fpt.Text;
            fpp = int.Parse(text);

        }

        private void FPText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text[0]);
        }

        private void CreditText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox sum = (TextBox)sender;
            sc = int.Parse(sum.Text);
        }
        private void MPS_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            i = int.Parse(tb.Text);

        }

        private void raschet_Click(object sender, RoutedEventArgs e)
        {
            double fvznos = (User.TOTAL * fpp) / 100;
            VznosText.Text = fvznos.ToString();

            double S = User.TOTAL - fvznos;
            Credit.Text = S.ToString();

            double A = (S * (i * Math.Pow(1 + i, mc)) / Math.Pow(1 + i, mc) - 1);
            ItogZnach.Text = A.ToString();
        }


        private void VznosText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox vt = (TextBox)sender;
            string text = vt.Text;
            vznos = int.Parse(text);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TextCounts == null)
                return;
            int newValue = (int)e.NewValue;
            int oldValue = (int)e.OldValue;
            int difValue = Math.Abs(newValue - oldValue);

            TextCounts.Text = (newValue).ToString();
            mc = newValue;

        }
    }
}
