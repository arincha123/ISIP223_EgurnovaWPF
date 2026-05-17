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
            LoadReviews();
        }

        private void LoadReviews()
        {
            // Загружаем только незамороженные отзывы для текущей книги
            Reviews = Core.Context.Review
                .Include("User")
                .Where(r => r.BookID == CurrentBook.ID && !r.IsFrozen)
                .OrderByDescending(r => r.Date)
                .ToList();

            // Обновляем ListBox
            ReviewsListBox.ItemsSource = null;
            ReviewsListBox.ItemsSource = Reviews;

            // Обновляем счетчик отзывов (если есть TextBlock для счетчика)
            UpdateReviewsCount();

            // Показываем сообщение, если отзывов нет
            if (Reviews.Count == 0)
            {
                NoReviewsText.Visibility = Visibility.Visible;
            }
            else
            {
                NoReviewsText.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateReviewsCount()
        {
            // Если у вас есть TextBlock для отображения количества отзывов
            // Например, ReviewsCountTextBlock.Text = $"Отзывы ({Reviews.Count})";

            // Или если у вас есть биндинг, то обновляем вручную
            var reviewsCountText = FindName("ReviewsCountTextBlock") as TextBlock;
            if (reviewsCountText != null)
            {
                reviewsCountText.Text = $"Отзывы ({Reviews.Count})";
            }
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void CommentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SubmitReviewButton_Click(object sender, RoutedEventArgs e)
        {

        }





    }
}
