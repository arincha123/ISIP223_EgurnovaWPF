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
        public CatalogPage()
        {
            InitializeComponent();
            PageLoad();
        }

        private void PageLoad()
        {
            List<Book> books = Core.Context.Book.ToList();
            ListBooks.ItemsSource = books;


            List<string> genres = Core.Context.Genre.Select(g => g.Name).ToList();
            ComboFiltr.ItemsSource = genres;


        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Book> sortedBooks = Core.Context.Book.ToList();
            sortedBooks = sortedBooks.Where(b => b.Title.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
            ListBooks.ItemsSource = sortedBooks;
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }



}
