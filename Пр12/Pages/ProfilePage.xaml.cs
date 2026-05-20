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
                switch (UserData.curUser.Role.ID)
                {
                    case 1: role = "Читатель"; break;
                    case 2: role = "Автор"; break;
                    case 3: role = "Администратор"; break;
                }

                UserRole.Text = role;

            }else
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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReview();
        }

        private void ReqForAuth_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы точно уверены, что хотите подать заявку на роль?", "Подача заявки", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                UserData.curUser.RoleID = 2;
            }
        }

        private void ReqForUnfroz_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
