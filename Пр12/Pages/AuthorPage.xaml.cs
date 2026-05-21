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

            BooksItemsControl.ItemsSource = authorsbooks;

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

        private void FreezeBookBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AppealFreezeBookBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
