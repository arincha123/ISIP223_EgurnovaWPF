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
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public List <Review> reviews { get; set; }
        public Profile()
        {
            InitializeComponent();
            LoadUser();
            
        }
        public void LoadUser()
        {
            if (UserData.curUser != null)
            {
                UserName.Text = UserData.curUser.Name;
                UserLogin.Text = UserData.curUser.Login;
                UserEamil.Text = UserData.curUser.Email;

                string role = "";
                switch (UserData.curUser.RoleID)
                {
                    case 1: role = "Читатель"; break;
                    case 2: role = "Автор"; break;
                    case 3: role = "Администратор"; break;
                }

                UserRole.Text = role;

                if (UserData.curUser.RoleID == 2 || UserData.curUser.RoleID == 3)
                {
                    ReqForAuth.Visibility = Visibility.Collapsed;
                }

            }
            else
            {
                return;
            }
        }

        public void LoadReview()
        {
            if (UserData.curUser == null) return;

            var rev = Core.Context.Review.Where(r => r.UserID == UserData.curUser.ID && r.IsFrozen == false).ToList();
            reviews= rev.OrderByDescending(r => r.Date).ToList();

            ReviewsListBox.ItemsSource = reviews;

            if (reviews.Count == 0)
            {
                NoReviewsText.Visibility = Visibility.Visible;
                ReviewsListBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoReviewsText.Visibility = Visibility.Collapsed;
                ReviewsListBox.Visibility = Visibility.Visible;
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReview();
        }

        private void ReqForAuth_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.curUser == null)
            {
                MessageBox.Show("Пользователь не авторизован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (UserData.curUser.IsFrozen)
            {
                MessageBox.Show("Ваш аккаунт заморожен! Вы не можете подать заявку на роль автора.",
                    "Аккаунт заморожен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (UserData.curUser.RoleID == 2)
            {
                MessageBox.Show("Вы уже являетесь автором!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (UserData.curUser.RoleID == 3)
            {
                MessageBox.Show("Вы уже являетесь администратором!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var existingRequest = Core.Context.RequestForAuthor.FirstOrDefault(r => r.UserID == UserData.curUser.ID && r.StatusID == 2);

            if (existingRequest != null)
            {
                MessageBox.Show("Вы уже подавали заявку на роль автора! Ожидайте рассмотрения.",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите подать заявку на роль автора?\n\nПосле рассмотрения заявки администратором вы сможете публиковать свои книги.",
                "Подача заявки", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var request = new RequestForAuthor
                    {
                        UserID = UserData.curUser.ID,
                        StatusID = 2,
                        Date = DateTime.Now
                    };

                    Core.Context.RequestForAuthor.Add(request);
                    Core.Context.SaveChanges();

                    MessageBox.Show("Заявка успешно отправлена! Администратор рассмотрит её в ближайшее время.",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    ReqForAuth.IsEnabled = false;
                    ReqForAuth.Content = "Заявка отправлена";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отправке заявки: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ReqForUnfroz_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.curUser == null)
            {
                MessageBox.Show("Пользователь не авторизован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!UserData.curUser.IsFrozen)
            {
                MessageBox.Show("Ваш аккаунт не заморожен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var existingRequest = Core.Context.RequestForDefrosting
                .FirstOrDefault(r => r.UserID == UserData.curUser.ID && r.RequestStatusID == 2 && r.TypeID == 1);

            if (existingRequest != null)
            {
                MessageBox.Show("Вы уже подавали заявку на разморозку! Ожидайте рассмотрения.",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Отправить заявку на разморозку аккаунта?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var request = new RequestForDefrosting
                    {
                        UserID = UserData.curUser.ID,
                        BookID = null,
                        Reason = "Пользователь просит разморозить аккаунт",
                        RequestStatusID = 2,
                        TypeID = 1,
                        Date = DateTime.Now
                    };

                    Core.Context.RequestForDefrosting.Add(request);
                    Core.Context.SaveChanges();

                    MessageBox.Show("Заявка на разморозку успешно отправлена! Администратор рассмотрит её в ближайшее время.",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    ReqForUnfroz.IsEnabled = false;
                    ReqForUnfroz.Content = "Заявка отправлена";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отправке заявки: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
