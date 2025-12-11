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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Page2());
            MainWindow.PlusProgress();

            if (NavigationService?.CanGoForward == true)
            {
                NavigationService.GoForward();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MinusProgress();
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService?.GoBack();
            }
        }

        private void Model_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.Content.ToString())
            {
                case "Седан":
                    User.Model = "Седан";
                    User.ModelCost = 1500000;
                    break;
                case "Хэтчбек":
                    User.Model = "Хэтчбек";
                    User.ModelCost = 1700000;
                    break;
                case "Внедорожник":
                    User.Model = "Внедорожник";
                    User.ModelCost = 2500000;
                    break;

            }
        }

        private void Type_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.Content.ToString())
            {
                case "Бензиновый 1.5 л (120 л. с.)":
                    User.Type = "Бензиновый 1.5 л (120 л. с.)";
                    User.TypeCost = 100000;
                    break;
                case "Бензиновый 2.0 л (150 л. с.)":
                    User.Type = "Бензиновый 2.0 л (150 л. с.)";
                    User.TypeCost = 150000;
                    break;
                case "Гибрид 2.5 л (200 л. с.)":
                    User.Type = "Гибрид 2.5 л (200 л. с.)";
                    User.TypeCost = 300000;
                    break;

            }
        }
    }
}
