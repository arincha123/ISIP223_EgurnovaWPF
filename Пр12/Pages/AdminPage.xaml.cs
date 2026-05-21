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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private List<Complaint> allComplaints;
        private List<RequestForDefrosting> allDefrostRequests;
        private List<RequestForAuthor> allAuthorRequests;
        private List<User> allUsers;
        private List<Book> allBooks;
        private List<Review> allReviews;

        public AdminPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComplaints();
            LoadDefrostRequests();
            LoadAuthorRequests();
            LoadFrozenLists();
            LoadUsers();
        }

        //загрузка списков
        private void LoadComplaints()
        {
            allComplaints = Core.Context.Complaint.OrderByDescending(c => c.Date).ToList();
            ComplaintsListBox.ItemsSource = allComplaints;
        }

        private void LoadFrozenLists()
        {
            allBooks = Core.Context.Book.Where(b => b.IsFrozen == true).ToList();
            FrozenBooksListBox.ItemsSource = allBooks;

            allUsers = Core.Context.User.Where(u => u.IsFrozen == true).ToList();
            FrozenUsersListBox.ItemsSource = allUsers;

            allReviews = Core.Context.Review.Include("User").Where(r => r.IsFrozen == true).ToList();
            FrozenReviewsListBox.ItemsSource = allReviews;
        }

        private void LoadUsers()
        {
            allUsers = Core.Context.User.ToList();
            UsersListBox.ItemsSource = allUsers;
        }

        private void LoadDefrostRequests()
        {
            allDefrostRequests = Core.Context.RequestForDefrosting.Where(r => r.RequestStatusID == 2).OrderByDescending(r => r.Date)
                .ToList();
            DefrostRequestsListBox.ItemsSource = allDefrostRequests;
        }

        private void LoadAuthorRequests()
        {
            allAuthorRequests = Core.Context.RequestForAuthor.Where(r => r.StatusID == 2).OrderByDescending(r => r.Date)
                .ToList();
            AuthorRequestsListBox.ItemsSource = allAuthorRequests;
        }

        //жалобы
        private void AcceptComplaint_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var complaint = btn.DataContext as Complaint;

            if (complaint != null)
            {
                var result = MessageBox.Show("Принять жалобу? Это приведет к заморозке объекта.",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (complaint.BookID != null)
                    {
                        var book = Core.Context.Book.FirstOrDefault(b => b.ID == complaint.BookID);
                        if (book != null) book.IsFrozen = true;
                    }

                    if (complaint.AuthorID != null)
                    {
                        var author = Core.Context.User.Find(complaint.AuthorID);
                        if (author != null) author.IsFrozen = true;
                    }

                    if (complaint.ReviewID != null)
                    {
                        var review = Core.Context.Review.Find(complaint.ReviewID);
                        if (review != null) review.IsFrozen = true;
                    }

                    Core.Context.Complaint.Remove(complaint);
                    Core.Context.SaveChanges();

                    LoadComplaints();
                    LoadFrozenLists();
                    LoadUsers();
                    MessageBox.Show("Жалоба принята!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void RejectComplaint_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var complaint = btn.DataContext as Complaint;

            if (complaint != null)
            {
                Core.Context.Complaint.Remove(complaint);
                Core.Context.SaveChanges();
                LoadComplaints();
                MessageBox.Show("Жалоба отклонена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //заявки на разморозку
        private void AcceptDefrostRequest_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var request = btn.DataContext as RequestForDefrosting;

            if (request != null)
            {
                request.RequestStatusID = 1;

                if (request.TypeID == 1 && request.BookID == null)
                {
                    var user = Core.Context.User.Find(request.UserID);
                    if (user != null) user.IsFrozen = false;
                }
                else if (request.TypeID == 2 && request.BookID != null)
                {
                    var book = Core.Context.Book.Find(request.BookID);
                    if (book != null) book.IsFrozen = false;
                }

                Core.Context.SaveChanges();
                LoadDefrostRequests();
                LoadFrozenLists();
                LoadUsers();
                MessageBox.Show("Заявка принята!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RejectDefrostRequest_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var request = btn.DataContext as RequestForDefrosting;

            if (request != null)
            {
                request.RequestStatusID = 3;
                Core.Context.SaveChanges();
                LoadDefrostRequests();
                MessageBox.Show("Заявка отклонена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //заявки на получение роли автора
        private void AcceptAuthorRequest_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var request = btn.DataContext as RequestForAuthor;

            if (request != null)
            {
                request.StatusID = 1;
                var user = Core.Context.User.Find(request.UserID);
                if (user != null) user.RoleID = 2;

                Core.Context.SaveChanges();
                LoadAuthorRequests();
                LoadUsers();
                MessageBox.Show("Заявка принята! Пользователь теперь автор.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RejectAuthorRequest_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var request = btn.DataContext as RequestForAuthor;

            if (request != null)
            {
                request.StatusID = 3;
                Core.Context.SaveChanges();
                LoadAuthorRequests();
                MessageBox.Show("Заявка отклонена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //замороженные книги, пользователи, отзывы
        private void UnfreezeBook_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var book = btn.DataContext as Book;
            if (book != null)
            {
                book.IsFrozen = false;
                Core.Context.SaveChanges();
                LoadFrozenLists();
                LoadUsers();
                MessageBox.Show("Книга разморожена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UnfreezeUser_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var user = btn.DataContext as User;
            if (user != null)
            {
                user.IsFrozen = false;
                Core.Context.SaveChanges();
                LoadFrozenLists();
                LoadUsers();
                MessageBox.Show("Пользователь разморожен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UnfreezeReview_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var review = btn.DataContext as Review;
            if (review != null)
            {
                review.IsFrozen = false;
                Core.Context.SaveChanges();
                LoadFrozenLists();
                MessageBox.Show("Отзыв разморожен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null || comboBox.SelectedItem == null)
                return;

            User user = comboBox.DataContext as User;
            if (user == null)
                return;

            if (user.ID == UserData.curUser.ID)
            {
                MessageBox.Show("Вы не можете изменить свою собственную роль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboBox.SelectedValue = user.RoleID;
                return;
            }

            string selectedRole = comboBox.SelectedItem.ToString();
            int roleId = 1;
            if (selectedRole == "Автор")
                roleId = 2;
            if (selectedRole == "Администратор")
                roleId = 3;

            if (user.RoleID == roleId)
                return;

            user.RoleID = roleId;
            Core.Context.SaveChanges();
            LoadUsers();
            MessageBox.Show($"Роль пользователя {user.Name} изменена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            User user = (sender as Button)?.DataContext as User;
            if (user == null)
                return;

            if (user.ID == UserData.curUser.ID)
            {
                MessageBox.Show("Вы не можете изменить свой собственный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ChangePassword window = new ChangePassword(user);
            window.Owner = Window.GetWindow(this);

            if (window.ShowDialog() == true)
            {
                user.Password = window.NewPassword;
                Core.Context.SaveChanges();
                LoadUsers();
                MessageBox.Show("Пароль успешно изменен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
