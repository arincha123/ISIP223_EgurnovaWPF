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
    /// Логика взаимодействия для EditBook.xaml
    /// </summary>
    public partial class EditBook : Page
    {
        private Book editingBook;
        private List<Genre> selectedgenres = new List<Genre>();

        public EditBook(Book book)
        {
            InitializeComponent();
            editingBook = book;
            LoadGenres();
            LoadBookData();

        }

        private void LoadGenres()
        {
            var allGenres = Core.Context.Genre.ToList();
            var availableGenres = allGenres.Where(g => !selectedgenres.Any(sg => sg.ID == g.ID)).ToList();
            GenresComboBox.ItemsSource = availableGenres;

            ListBoxGenres.ItemsSource = selectedgenres.ToList();
        }

        private void LoadBookData()
        {
            TitleTextBox.Text = editingBook.Title;
            DescriptionTextBox.Text = editingBook.Description;
            TextTextBox.Text = editingBook.Text;
            TxtBoxImagePath.Text = editingBook.PathToCover;

            selectedgenres = editingBook.BookInGenre?.Select(bg => bg.Genre).ToList() ?? new List<Genre>();
            LoadGenres();
        }
        private void BtnAddGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresComboBox.SelectedItem is Genre selectedGenre)
            {
                if (selectedgenres.Any(g => g.ID == selectedGenre.ID))
                {
                    MessageBox.Show("Этот жанр уже выбран!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                selectedgenres.Add(selectedGenre);
                LoadGenres();
            }
        }

        private void LoadEditingBook(Book book)
        {
            var bookGenreIds = book.BookInGenre.Select(bg => bg.GenreID).ToList();

            foreach (var item in ListBoxGenres.Items)
            {
                var genre = item as Genre;
                if (genre != null && bookGenreIds.Contains(genre.ID))
                {
                    ListBoxGenres.SelectedItems.Add(item); // Выделяем
                }
            }
        }
        private void BtnDeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var genre = btn?.DataContext as Genre;
            if (genre != null)
            {
                selectedgenres.Remove(genre);
                LoadGenres();
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            editingBook.Title = TitleTextBox.Text.Trim();
            editingBook.Description = DescriptionTextBox.Text.Trim();
            editingBook.Text = TextTextBox.Text.Trim();
            editingBook.PathToCover = TxtBoxImagePath.Text.Trim();

            var existingGenres = Core.Context.BookInGenre.Where(bg => bg.BookID == editingBook.ID).ToList();
            Core.Context.BookInGenre.RemoveRange(existingGenres);

            foreach (var genre in selectedgenres)
            {
                Core.Context.BookInGenre.Add(new BookInGenre
                {
                    BookID = editingBook.ID,
                    GenreID = genre.ID
                });
            }

            Core.Context.SaveChanges();
            MessageBox.Show("Книга обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите отменить добавление книги? Все данные будут потеряны.",
             "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    NavigationService.Navigate(new AuthorPage());
                }
            }
        }
    }
}
