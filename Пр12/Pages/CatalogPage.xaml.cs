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
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        List<Book> allBooks = Core.Context.Book.ToList();
        List<Book> filteredBooks = Core.Context.Book.ToList();
        public CatalogPage()
        {
            InitializeComponent();
            PageLoad();
        }

        private void PageLoad()
        {
            ListBooks.ItemsSource = allBooks;

            List<string> genres = Core.Context.Genre.Select(g => g.Name).ToList();
            ComboFiltr.Items.Clear();
            ComboFiltr.ItemsSource = genres;
            genres.Insert(0, "Все жанры");
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFiltersAndSearch();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndSearch();
        }

        private void ComboFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndSearch();
        }

        void ApplyFiltersAndSearch()
        {
            var result = allBooks.ToList();

            //поиск
            if (!string.IsNullOrEmpty(SearchBox.Text))
            {
                var searchtext = SearchBox.Text.ToLower();

                result = result.Where(b => (b.Title != null && b.Title.ToLower().Contains(searchtext)) ||
                    (b.User != null && b.User.Name != null && b.User.Name.ToLower().Contains(searchtext))).ToList();
            }

            //работа сортировки по названию и рейтингу
            if (ComboSort.SelectedItem is ComboBoxItem selecteditem && selecteditem.Tag != null)
            {
                string TypeOfSort = selecteditem.Tag?.ToString();
                switch (TypeOfSort)
                {
                    case "По названию":
                        result = allBooks.OrderBy(b => b.Title).ToList();
                        break;
                    case "По рейтингу":
                        result = allBooks.OrderBy(b => b.AvgRating).ToList();
                        break;
                }
            }

            //фильтрация по жанрам
            if (ComboFiltr.SelectedItem != null && ComboFiltr.SelectedItem.ToString() != "Все жанры")
            {
                string selectedGenre = ComboFiltr.SelectedItem.ToString();
                result = result.Where(s => s.BookInGenre != null && s.BookInGenre.Any(bg => bg.Genre != null && bg.Genre.Name == selectedGenre)).ToList();
            }

            filteredBooks = result;
            ListBooks.ItemsSource = filteredBooks;
        }

        private void ListBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Book selectedbook = ListBooks.SelectedItem as Book;
            if (selectedbook != null)
            {
                BookInfo infiOfBook = new BookInfo(selectedbook);
                NavigationService.Navigate(infiOfBook);
            }
        }

    }
}
