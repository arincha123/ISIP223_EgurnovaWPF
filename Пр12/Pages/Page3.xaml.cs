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
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            User.TOTAL = User.ModelCost + User.TypeCost + User.ColorCost + User.OptionsCost;

            Model.Text = $"Модель вашей машины: {User.Model}";
            ModelCost.Text = $"Стоимость модели: {User.ModelCost} Р";
            Type.Text = $"Тип двигателя вашей машины: {User.Type}";
            TypeCost.Text = $"Стоимость двигателя: {User.TypeCost} Р";
            Color.Text = $"Цвет кузова вашей машины: {User.Color}";
            ColorCost.Text = $"Цена за цвет: {User.ColorCost} Р";
            Options.Text = $"Выбранные доп опции:\n{User.StrOptions}";
            OptionsCost.Text = $"Стоимость за все опции: {User.OptionsCost} Р";
            Itog.Text = $"Итог: {User.TOTAL}";
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Page4());
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}