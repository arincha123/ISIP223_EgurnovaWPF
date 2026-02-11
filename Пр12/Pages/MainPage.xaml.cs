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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private List <FILM> allFs;
        private List<FILM> sorted;
        public MainPage()
        {
            InitializeComponent();
            List<FILM> films = Core.Context.FILM.ToList();
            Films.ItemsSource = films;
            PageLoad();

        }
        


        private void PageLoad()
        {

            allFs = Core.Context.FILM.ToList();
            Films.ItemsSource = allFs;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<FILM> fILMs = Core.Context.FILM.ToList();
            var search = Search.Text.ToLower();
            fILMs = fILMs.Where(f => f.NAME_FILM.ToLower().Contains(Search.Text.ToLower())).ToList();
            Films.ItemsSource = fILMs;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Sort.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortType = selectedItem.Tag?.ToString();
                switch (sortType)
                {
                    case "Названию":
                        sorted = allFs.OrderBy(f => f.NAME_FILM).ToList();
                        Films.ItemsSource = sorted;
                        break;
                    case "Рейтингу":
                        sorted = allFs.OrderByDescending(f => f.RATING_FILM).ToList();
                        Films.ItemsSource = sorted;
                        break;
                }
            }
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountPage());
        }


        private void Films_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FILM selectedFilm = Films.SelectedItem as FILM;

            if (selectedFilm != null)
            {
                FilmInfo detailsPage = new FilmInfo(selectedFilm);

                NavigationService.Navigate(detailsPage);
            }
        }
    }
}
