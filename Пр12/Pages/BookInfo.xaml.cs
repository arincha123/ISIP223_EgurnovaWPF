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
        public bool IsAdmin => UserData.curUser != null && UserData.curUser.RoleID == 3;
        public Visibility FreezeButtonVisibility => (UserData.curUser != null && UserData.curUser.RoleID == 3) ? Visibility.Visible : Visibility.Collapsed;

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

            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Напишите текст отзыва!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ComboBoxItem selectedItem = RatingComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Выберите оценку!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CurrentBook.AuthorID == UserData.curUser.ID)
            {
                MessageBox.Show("Вы не можете оставлять отзывы на свои собственные книги!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedRating = int.Parse(selectedItem.Content.ToString().Split(' ')[0]);
            int rating = selectedRating * 2;

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
            Book selectedBook = CurrentBook;

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
                string statusName = GetStatusName(existingBook.ListStatusID);
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

                    string statusName = GetStatusName(addWindow.SelectedListStatusID);
                    MessageBox.Show($"Книга \"{selectedBook.Title}\" добавлена в список \"{statusName}\"!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string GetStatusName(int statusId)
        {
            switch (statusId)
            {
                case 1: return "Заброшено";
                case 2: return "В планах";
                case 3: return "Читаю";
                case 4: return "Прочитано";
                default: return "Неизвестно";
            }
        }

        private void FreezeBook_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Вы уверены, что хотите заморозить книгу \"{CurrentBook.Title}\"?\n\nПосле заморозки книга будет скрыта от пользователей.",
                "Подтверждение заморозки", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    CurrentBook.IsFrozen = true;
                    Core.Context.SaveChanges();

                    MessageBox.Show("Книга успешно заморожена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FreezeReview_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.curUser == null || UserData.curUser.RoleID != 3)
            {
                MessageBox.Show("У вас нет прав для заморозки отзыва!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Button button = sender as Button;
            Review selectedReview = button?.Tag as Review;

            if (selectedReview == null)
            {
                MessageBox.Show("Отзыв не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Заморозить отзыв пользователя {selectedReview.User?.Name}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    selectedReview.IsFrozen = true;
                    Core.Context.SaveChanges();

                    LoadReviews();
                    UpdateBookRating();

                    MessageBox.Show("Отзыв заморожен!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }




        private void ComplainBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string complaintType = button?.Tag?.ToString();

            if (string.IsNullOrEmpty(complaintType))
            {
                MessageBox.Show("Ошибка определения типа жалобы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (UserData.curUser == null)
            {
                MessageBox.Show("Чтобы отправить жалобу, необходимо авторизоваться!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (UserData.curUser.IsFrozen)
            {
                MessageBox.Show("Ваш аккаунт заморожен! Вы не можете отправлять жалобы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? authorId = null;
            int? bookId = null;
            int? reviewId = null;
            string targetName = "";
            string targetType = "";

            switch (complaintType)
            {
                case "Book":
                    bookId = CurrentBook.ID;
                    targetName = CurrentBook.Title;
                    targetType = "Книгу";
                    break;

                case "Author":
                    authorId = CurrentBook.AuthorID;
                    targetName = CurrentBook.User?.Name ?? "Автора";
                    targetType = "Автора";
                    break;

                case "Review":
                    Review selectedReview = button?.DataContext as Review;
                    if (selectedReview == null)
                    {
                        MessageBox.Show("Отзыв не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    reviewId = selectedReview.ID;
                    targetName = $"Отзыв пользователя {selectedReview.User?.Name}";
                    targetType = "Отзыв";
                    break;

                default:
                    MessageBox.Show("Неизвестный тип жалобы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            ComplaintWindow complaintWindow = new ComplaintWindow(targetType, targetName, authorId, bookId, reviewId);
            complaintWindow.Owner = Window.GetWindow(this);
            complaintWindow.ShowDialog();
        }

    }
}
