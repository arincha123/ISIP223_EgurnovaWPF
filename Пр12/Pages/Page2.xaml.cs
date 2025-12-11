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
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            User.UpdateStrOptions();
            User.StrOptions = string.Join<string>("\n", User.Options);
            NavigationService.Navigate(new Page3());
            MainWindow.PlusProgress();

            if (NavigationService?.CanGoForward == true)
            {
                NavigationService.GoForward();
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            User.UpdateStrOptions();
            MainWindow.MinusProgress();
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService?.GoBack();
            }
        }

        private void Color_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.Content.ToString())
            {
                case "Чёрный цвет (+200)":
                    User.Color = "Чёрный цвет (+200)";
                    User.ColorCost = 200;
                    break;
                case "Красный цвет (+500)":
                    User.Color = "Красный цвет (+500)";
                    User.ColorCost = 500;
                    break;
                case "Оранжевый цвет (+500)":
                    User.Color = "Оранжевый цвет (+500)";
                    User.ColorCost = 500;
                    break;
                case "Жёлтый цвет (+500)":
                    User.Color = "Жёлтый цвет (+500)";
                    User.ColorCost = 500;
                    break;
                case "Зелёный цвет (+500)":
                    User.Color = "Зелёный цвет (+500)";
                    User.ColorCost = 500;
                    break;
                case "Голубой цвет (+500)":
                    User.Color = "Голубой цвет (+500)";
                    User.ColorCost = 500;
                    break;
                case "Синий цвет (+500)":
                    User.Color = "Синий цвет (+500)";
                    User.ColorCost = 500;
                    break;
                case "Фиолетовый цвет (+500)":
                    User.Color = "Фиолетовый цвет (+500)";
                    User.ColorCost = 500;
                    break;
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SyncCheckBoxesWithUserData();
        }

        private void SyncCheckBoxesWithUserData()
        {
            // Восстанавливаем состояние галочек из User.Options
            Krisha.IsChecked = User.Options.Contains("панорамная крыша");
            Salon.IsChecked = User.Options.Contains("кожаный салон");
            Climat.IsChecked = User.Options.Contains("климат-контроль");
            Cruise.IsChecked = User.Options.Contains("круиз-контроль");
        }

        private void Krisha_Click(object sender, RoutedEventArgs e)
        {
            User.UpdateOption("панорамная крыша", 8000, Krisha.IsChecked == true);
        }

        private void Climat_Click(object sender, RoutedEventArgs e)
        {
            User.UpdateOption("климат-контроль", 25000, Climat.IsChecked == true);
        }

        private void Cruise_Click(object sender, RoutedEventArgs e)
        {
            User.UpdateOption("круиз-контроль", 45000, Cruise.IsChecked == true);
        }

        private void Salon_Click(object sender, RoutedEventArgs e)
        {
            User.UpdateOption("кожаный салон", 120000, Salon.IsChecked == true);
        }

    }
}
