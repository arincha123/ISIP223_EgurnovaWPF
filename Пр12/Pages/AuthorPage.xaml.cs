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
    /// Логика взаимодействия для AuthorPage.xaml
    /// </summary>
    public partial class AuthorPage : Page
    {
        public List<Book> authorsbooks;
        public List<Book> frozenBooks;

        public AuthorPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }

        private void LoadBooks()
        {
            if (UserData.curUser == null) return;

            authorsbooks = Core.Context.Book.Where(b => b.AuthorID == UserData.curUser.ID && !b.IsFrozen).OrderByDescending(b => b.ID).ToList();

            MyOwnBooks.ItemsSource = authorsbooks;

            frozenBooks = Core.Context.Book.Where(b => b.AuthorID == UserData.curUser.ID && b.IsFrozen).OrderByDescending(b => b.ID).ToList();

            FrozenBooksItemsControl.ItemsSource = frozenBooks;

        }

        private void AddBookBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddBook());
        }

        private void EditBookBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Book book = (Book)btn.DataContext;

            if (book != null)
            {
                NavigationService.Navigate(new EditBook(book));
            }

        }

        private void AppealFreezeBookBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Book selectedBook = button?.Tag as Book;

            if (selectedBook == null) return;

            if (!selectedBook.IsFrozen)
            {
                MessageBox.Show("Эта книга не заморожена!", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var existingRequest = Core.Context.RequestForDefrosting
                .FirstOrDefault(r => r.BookID == selectedBook.ID && r.RequestStatusID == 2);

            if (existingRequest != null)
            {
                MessageBox.Show("Вы уже подавали заявку на разморозку этой книги!",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var appealWindow = new AppealBookWindow(selectedBook.Title);
            appealWindow.Owner = Window.GetWindow(this);

            if (appealWindow.ShowDialog() == true)
            {
                try
                {
                    var request = new RequestForDefrosting
                    {
                        UserID = UserData.curUser.ID,
                        BookID = selectedBook.ID,
                        Reason = appealWindow.AppealReason,
                        RequestStatusID = 2,
                        TypeID = 2,
                        Date = DateTime.Now
                    };

                    Core.Context.RequestForDefrosting.Add(request);
                    Core.Context.SaveChanges();

                    MessageBox.Show("Заявка на разморозку книги успешно отправлена!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    button.IsEnabled = false;
                    button.Content = "✓ Заявка отправлена";
                    button.Background = new SolidColorBrush(Colors.Gray);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DelBookBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Book selectedBook = button?.Tag as Book;

            if (selectedBook == null) return;

            var result = MessageBox.Show($"Удалить книгу \"{selectedBook.Title}\"?\nЭто действие нельзя отменить.",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var bookGenres = Core.Context.BookInGenre.Where(bg => bg.BookID == selectedBook.ID).ToList();
                    Core.Context.BookInGenre.RemoveRange(bookGenres);

                    var reviews = Core.Context.Review.Where(r => r.BookID == selectedBook.ID).ToList();
                    Core.Context.Review.RemoveRange(reviews);

                    var readingLists = Core.Context.ReadingList.Where(rl => rl.BookID == selectedBook.ID).ToList();
                    Core.Context.ReadingList.RemoveRange(readingLists);

                    Core.Context.Book.Remove(selectedBook);
                    Core.Context.SaveChanges();

                    LoadBooks();
                    MessageBox.Show("Книга удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
