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
    /// Логика взаимодействия для ListOfBooks.xaml
    /// </summary>
    public partial class ListOfBooks : Page
    {
        private List<ReadingList> allUserBooks;
        private List<Book> allBooks;

        public ListOfBooks()
        {
            InitializeComponent();
            Loaded += Page_Loaded;

            allBooks = Core.Context.Book.ToList();

            List<string> genres = Core.Context.Genre.Select(g => g.Name).ToList();
            genres.Insert(0, "Все жанры");
            ComboFiltr.ItemsSource = genres;
            ComboFiltr.SelectedIndex = 0;

            LoadUserLists();


        }


        private void LoadUserLists()
        {
            if (UserData.curUser == null)
            {
                return;
            }

            if (UserData.curUser.IsFrozen)
            {
                HideComboboxes();
                
            }

            allUserBooks = Core.Context.ReadingList.Where(rl => rl.UserID == UserData.curUser.ID).ToList();
            allUserBooks = allUserBooks.Where(rl => rl.Book.IsFrozen == false).ToList();

            ApplyFiltersAndSearch();
        }

        private void ApplyFiltersAndSearch()
        {
            if (allUserBooks == null) return;

            var result = allUserBooks.ToList();

            if (!string.IsNullOrEmpty(SearchBox.Text))
            {
                var searchtext = SearchBox.Text.ToLower();

                result = result.Where(b =>(b.Book.Title != null && b.Book.Title.ToLower().Contains(searchtext)) ||
                    (b.Book.User != null && b.Book.User.Name != null && b.Book.User.Name.ToLower().Contains(searchtext))
                ).ToList();
            }

            if (ComboSort.SelectedItem is ComboBoxItem selecteditem && selecteditem.Tag != null)
            {
                string TypeOfSort = selecteditem.Tag?.ToString();
                switch (TypeOfSort)
                {
                    case "По названию":
                        result = result.OrderBy(b => b.Book.Title).ToList();
                        break;
                    case "По рейтингу":
                        result = result.OrderBy(b => b.Book.AvgRating).ToList();
                        break;
                    default:
                        result = result.OrderBy(b => b.Book.Title).ToList();
                        break;
                }
            }
            else
            {
                result = result.OrderBy(b => b.Book.Title).ToList();
            }

            if (ComboFiltr.SelectedItem != null && ComboFiltr.SelectedItem.ToString() != "Все жанры")
            {
                string selectedGenre = ComboFiltr.SelectedItem.ToString();
                result = result.Where(s =>
                    s.Book.BookInGenre != null &&
                    s.Book.BookInGenre.Any(bg => bg.Genre != null && bg.Genre.Name == selectedGenre)
                ).ToList();
            }

            DisplayBooksByStatus(result);
        }

        private void DisplayBooksByStatus(List<ReadingList> books)
        {
            var abandoned = books.Where(b => b.ListStatusID == 1).ToList();
            AbandonedListBox.ItemsSource = abandoned;

            var plans = books.Where(b => b.ListStatusID == 2).ToList();
            PlansListBox.ItemsSource = plans;

            var reading = books.Where(b => b.ListStatusID == 3).ToList();
            ReadingListBox.ItemsSource = reading;

            var read = books.Where(b => b.ListStatusID == 4).ToList();
            ReadListBox.ItemsSource = read;

            UpdateTabHeaders(abandoned.Count, plans.Count, reading.Count, read.Count);
        }

        private void UpdateTabHeaders(int abandonedCount, int plansCount, int readingCount, int readCount)
        {
            AbandonedTab.Header = $"Заброшено ({abandonedCount})";
            PlansTab.Header = $"В планах ({plansCount})";
            ReadingTab.Header = $"Читаю ({readingCount})";
            ReadTab.Header = $"Прочитано ({readCount})";
        }

        private void MoveToList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var readingItem = comboBox?.Tag as ReadingList;

            if (readingItem == null || comboBox.SelectedItem == null) return;

            var selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem?.Tag == null) return;

            int newStatusId = int.Parse(selectedItem.Tag.ToString());

            if (readingItem.ListStatusID == newStatusId) return;

            var result = MessageBox.Show("Переместить книгу в другой список?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    readingItem.ListStatusID = newStatusId;
                    Core.Context.SaveChanges();

                    LoadUserLists();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при перемещении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                LoadUserLists();
            }
        }

        public void HideComboboxes()
        {
            AbandonedListBox.IsHitTestVisible = false;
            PlansListBox.IsHitTestVisible = false;
            ReadingListBox.IsHitTestVisible = false;
            ReadListBox.IsHitTestVisible = false;
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


        //private void Btn_Back_Click(object sender, RoutedEventArgs e)
        //{
        //    if (NavigationService.CanGoBack)
        //    {
        //        NavigationService.GoBack();
        //    }
        //}

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            ReadingList selectedItem = listBox?.SelectedItem as ReadingList;

            if (selectedItem?.Book != null)
            {
                BookInfo bookInfo = new BookInfo(selectedItem.Book);
                NavigationService.Navigate(bookInfo);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserLists();
        }

    }
}