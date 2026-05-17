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
    /// Логика взаимодействия для BookInfo.xaml
    /// </summary>
    public partial class BookInfo : Page
    {
        public Book CurrentBook { get; set; }
        public List<Review> Reviews { get; set; }

        public BookInfo()
        {
            InitializeComponent();
        }

        public BookInfo(Book currentBook) : this()
        {
            CurrentBook = currentBook;
            DataContext = this;

            UpdateBookRating();
            LoadFreezeBtn();
            LoadReviews();
        }

        private void LoadFreezeBtn()
        {
            if (UserData.curUser != null && UserData.curUser.RoleID == 3)
            {
                FreezeBook.Visibility = Visibility.Visible;
            }
            else
            {
                FreezeBook.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadReviews()
        {
            Reviews = Core.Context.Review.Include("User").Where(r => r.BookID == CurrentBook.ID && !r.IsFrozen).OrderByDescending(r => r.Date).ToList();

            ReviewsListBox.ItemsSource = null;
            ReviewsListBox.ItemsSource = Reviews;


            if (Reviews.Count == 0)
            {
                NoReviewsText.Visibility = Visibility.Visible;
            }
            else
            {
                NoReviewsText.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateBookRating()
        {
            double avgRating = 0;
            if (CurrentBook.Review.Count > 0)
            {
                double sum = 0;
                foreach (var review in CurrentBook.Review)
                {
                    sum += review.Rating;
                }
                avgRating = sum / CurrentBook.Review.Count;
            }
            RatingText.Text = Math.Round(avgRating).ToString();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void SubmitReviewButton_Click(object sender, RoutedEventArgs e)
        {
            string text = CommentText.Text.Trim();
            if (text == null)
            {
                MessageBox.Show("Напишите свой отзыв...");
                return;
            }

            ComboBoxItem selectedItem = RatingComboBox.SelectedItem as ComboBoxItem;
            int rating = int.Parse(selectedItem.Content.ToString());

            Review newReview = new Review
            {
                BookID = CurrentBook.ID,
                UserID = UserData.curUser.ID,
                Text = text,
                Rating = rating,
                Date = DateTime.Now,
                IsFrozen = false
            };

            Core.Context.Review.Add(newReview);
            Core.Context.SaveChanges();

            CommentText.Clear();
            RatingComboBox.SelectedIndex = 0;

            LoadReviews();

            UpdateBookRating();
        }

        private void Readpart_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CurrentBook.Text))
            {
                TextOfBook textWindow = new TextOfBook(CurrentBook.Title, CurrentBook.Text);
                textWindow.Owner = Window.GetWindow(this);
                textWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Фрагмент книги недоступен!", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddInList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FreezeBook_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComplainAuthorBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComplainBookBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
