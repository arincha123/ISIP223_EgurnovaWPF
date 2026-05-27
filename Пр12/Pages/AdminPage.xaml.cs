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

        /// <summary>
        /// Загрузка всех списков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComplaints();
            LoadDefrostRequests();
            LoadAuthorRequests();
            LoadFrozenLists();
            LoadUsers();
        }

        /// <summary>
        /// Загрузка списка жалоб
        /// </summary>
        private void LoadComplaints()
        {
            allComplaints = Core.Context.Complaint.OrderByDescending(c => c.Date).ToList();
            ComplaintsListBox.ItemsSource = allComplaints;
        }

        /// <summary>
        /// Загрузка списков замороженных объектов
        /// </summary>
        private void LoadFrozenLists()
        {
            allBooks = Core.Context.Book.Where(b => b.IsFrozen == true).ToList();
            FrozenBooksListBox.ItemsSource = allBooks;

            allUsers = Core.Context.User.Where(u => u.IsFrozen == true).ToList();
            FrozenUsersListBox.ItemsSource = allUsers;

            allReviews = Core.Context.Review.Include("User").Where(r => r.IsFrozen == true).ToList();
            FrozenReviewsListBox.ItemsSource = allReviews;
        }

        /// <summary>
        /// Загрузка списка пользователей
        /// </summary>
        private void LoadUsers()
        {
            allUsers = Core.Context.User.ToList();
            UsersListBox.ItemsSource = allUsers;
        }

        /// <summary>
        /// Загрузка списка тех, кто подал запрос на разморозку
        /// </summary>
        private void LoadDefrostRequests()
        {
            allDefrostRequests = Core.Context.RequestForDefrosting.Where(r => r.RequestStatusID == 2).OrderByDescending(r => r.Date)
                .ToList();
            DefrostRequestsListBox.ItemsSource = allDefrostRequests;
        }

        /// <summary>
        /// Загрузка списка тех, кто подал запрос на изменение статума на "Автор"
        /// </summary>
        private void LoadAuthorRequests()
        {
            allAuthorRequests = Core.Context.RequestForAuthor.Where(r => r.StatusID == 2).OrderByDescending(r => r.Date)
                .ToList();
            AuthorRequestsListBox.ItemsSource = allAuthorRequests;
        }

        /// <summary>
        /// Принятие жалобы (замораживает объект)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Отклонение жалобы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Принятие заявки на разморозку (размораживает книгу или автора)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Отклонение заявки на разморозку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Принятие заявки на роль Автора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Отклонение заявки на роль Автора 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Размораживание книги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                UpdateCatalogPage();

                MessageBox.Show("Книга разморожена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Обновление каталога
        /// </summary>
        private void UpdateCatalogPage()
        {
            var frame = MainPage.CatalogFrame;
            if (frame?.Content is CatalogPage catalogPage)
            {
                catalogPage.RefreshData();
            }
        }

        /// <summary>
        /// Размораживание пользователя (автора)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Размораживание отзыва
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Изменение роли
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null || comboBox.SelectedIndex == -1)
                return;

            User user = comboBox.DataContext as User;
            if (user == null)
                return;

            if (user.ID == UserData.curUser.ID)
            {
                MessageBox.Show("Вы не можете изменить свою собственную роль!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                comboBox.SelectedIndex = GetRoleIndex(user.RoleID);
                return;
            }

            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            int newRoleId = int.Parse(selectedItem.Tag.ToString());

            if (user.RoleID == newRoleId) return;

            var result = MessageBox.Show($"Изменить роль пользователя {user.Name}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                user.RoleID = newRoleId;
                Core.Context.SaveChanges();
                LoadUsers();
                MessageBox.Show("Роль изменена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                comboBox.SelectedIndex = GetRoleIndex(user.RoleID);
            }
        }

        /// <summary>
        /// Получение ID роли в зависимости от выбранного comboboxItem
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private int GetRoleIndex(int roleId)
        {
            switch (roleId)
            {
                case 1: return 0;
                case 2: return 1;
                case 3: return 2;
                default: return 0;
            }
        }

        /// <summary>
        /// Изменение пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
