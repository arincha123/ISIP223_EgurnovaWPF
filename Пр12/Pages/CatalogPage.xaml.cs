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
using Пр12.Windows;

namespace Пр12.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        private List<Book> allBooks {  get; set; }
        List<Book> filteredBooks = Core.Context.Book.ToList();
        public CatalogPage()
        {
            InitializeComponent();
            PageLoad();
        }

        private void PageLoad()
        {
            allBooks = Core.Context.Book.Where(b => b.IsFrozen == false).ToList();

            foreach (var book in allBooks)
            {
                UpdateBookRating(book);
            }

            ListBooks.ItemsSource = allBooks;

            List<string> genres = Core.Context.Genre.Select(g => g.Name).ToList();
            ComboFiltr.ItemsSource = genres;
            genres.Insert(0, "Все жанры");
        }

        private void UpdateBookRating(Book book)
        {
            if (book.Review != null && book.Review.Count > 0)
            {
                double sum = 0;
                foreach (var review in book.Review)
                {
                    sum += review.Rating;
                }
                book.AvgRating = Math.Round(sum / book.Review.Count, 1);
            }
            else
            {
                book.AvgRating = 0;
            }
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

        private void ApplyFiltersAndSearch()
        {
            var result = allBooks.Where(b => !b.IsFrozen).ToList();

            foreach (var book in result)
            {
                    if (book.Review != null && book.Review.Any())
                    {
                        double sum = book.Review.Sum(r => r.Rating);
                        book.AvgRating = Math.Round(sum / book.Review.Count, 1);
                    }
                    else
                    {
                        book.AvgRating = 0;
                    }
            }

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
                        result = result.OrderBy(b => b.Title).ToList();
                        break;
                    case "По рейтингу":
                        result = result.OrderByDescending(b => b.AvgRating).ToList();
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
                if (UserData.curUser != null && UserData.curUser.IsFrozen)
                {
                    MessageBox.Show("Ваш аккаунт заморожен! Для просмотра информации о книге обратитесь к администратору.",
                                   "Аккаунт заморожен", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                BookInfo infiOfBook = new BookInfo(selectedbook);
                NavigationService.Navigate(infiOfBook);
            }
        }

        public void RefreshData()
        {
            allBooks = Core.Context.Book.Where(b => b.IsFrozen == false).ToList();

            foreach (var book in allBooks)
            {
                UpdateBookRating(book);
            }

            ApplyFiltersAndSearch();
        }

        private void AddToListBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.curUser.IsFrozen == true)
            {
                MessageBox.Show("Ваш аккаунт заморожен! Обратитесь к администратору. Вы не можете добавлять книги в списки.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Button button = sender as Button;
            Book selectedBook = button?.DataContext as Book;

            if (selectedBook == null)
            {
                MessageBox.Show("Книга не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (UserData.curUser == null)
            {
                MessageBox.Show("Чтобы добавить книгу в список, необходимо авторизоваться!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var existingBook = Core.Context.ReadingList.FirstOrDefault(rl => rl.UserID == UserData.curUser.ID && rl.BookID == selectedBook.ID);

            if (existingBook != null)
            {
                string statusName = "";
                switch (existingBook.ListStatusID)
                {
                    case 1: statusName = "Заброшено"; break;
                    case 2: statusName = "В планах"; break;
                    case 3: statusName = "Читаю"; break;
                    case 4: statusName = "Прочитано"; break;
                }

                MessageBox.Show($"Эта книга уже находится в списке \"{statusName}\"!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AddInListWindow addWindow = new AddInListWindow();
            addWindow.Owner = Window.GetWindow(this);

            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    ReadingList newReadingItem = new ReadingList
                    {
                        UserID = UserData.curUser.ID,
                        BookID = selectedBook.ID,
                        ListStatusID = addWindow.SelectedListStatusID
                    };

                    Core.Context.ReadingList.Add(newReadingItem);
                    Core.Context.SaveChanges();

                    string statusName = "";
                    switch (addWindow.SelectedListStatusID)
                    {
                        case 2: statusName = "В планах"; break;
                        case 3: statusName = "Читаю"; break;
                        case 4: statusName = "Прочитано"; break;
                    }

                    MessageBox.Show($"Книга \"{selectedBook.Title}\" добавлена в список \"{statusName}\"!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
