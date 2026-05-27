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
    /// Станица добавления новой книги
    /// </summary>
    public partial class AddBook : Page
    {
        private List<Genre> selectedgenres = new List<Genre>(); //выбранные жанры
        public AddBook()
        {
            InitializeComponent();
            LoadGenres();
        }

        /// <summary>
        /// Загрузка (обновление) доступных жанров
        /// </summary>
        private void LoadGenres()
        {
            var allGenres = Core.Context.Genre.ToList();
            var availableGenres = allGenres.Where(g => !selectedgenres.Any(sg => sg.ID == g.ID)).ToList();
            GenresComboBox.ItemsSource = availableGenres;
            GenresComboBox.SelectedIndex = availableGenres.Any() ? 0 : -1;
            ListBoxGenres.ItemsSource = selectedgenres.ToList();
        }

        /// <summary>
        /// Добвление жанров книге
        /// </summary>
        private void BtnAddGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresComboBox.SelectedItem is Genre selectedGenre)
            {
                if (selectedgenres == null)
                    selectedgenres = new List<Genre>();

                if (selectedgenres.Any(g => g.ID == selectedGenre.ID))
                {
                    MessageBox.Show("Этот жанр уже выбран!", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedgenres.Add(selectedGenre);
                LoadGenres();
            }
            else
            {
                MessageBox.Show("Выберите жанр из списка!", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Удаление жанра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var genre = btn?.DataContext as Genre;
            if (genre != null && selectedgenres != null)
            {
                selectedgenres.Remove(genre);
                LoadGenres();
            }
        }

        /// <summary>
        /// Сохранение добавленной книги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введите название книги!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtBoxImagePath.Text))
            {
                MessageBox.Show("Введите путь обложки вашей книги", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedgenres == null || selectedgenres.Count == 0) 
            {
                MessageBox.Show("Выберите хотя бы один жанр!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Введите описание книги!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextTextBox.Text))
            {
                MessageBox.Show("Введите текст книги!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newBook = new Book
                {
                    Title = TitleTextBox.Text.Trim(),
                    Description = DescriptionTextBox.Text.Trim(),
                    Text = TextTextBox.Text.Trim(),
                    PathToCover = TxtBoxImagePath.Text,
                    AuthorID = UserData.curUser.ID,
                    IsFrozen = false,
                    AvgRating = 0
                };

                Core.Context.Book.Add(newBook);
                Core.Context.SaveChanges();

                foreach (var genre in selectedgenres)
                {
                    var bookInGenre = new BookInGenre
                    {
                        BookID = newBook.ID,
                        GenreID = genre.ID
                    };
                    Core.Context.BookInGenre.Add(bookInGenre);
                }

                Core.Context.SaveChanges();

                MessageBox.Show("Книга успешно добавлена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        /// <summary>
        /// Отмена / выход из окна добавления новой книги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
