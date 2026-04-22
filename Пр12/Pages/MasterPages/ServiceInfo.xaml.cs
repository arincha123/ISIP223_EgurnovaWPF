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

namespace Пр12.Pages.MasterPages
{
    /// <summary>
    /// Логика взаимодействия для ServiceInfo.xaml
    /// </summary>
    public partial class ServiceInfo : Page
    {
        public UserService CurrentServ { get; set; }

        public ServiceInfo(UserService service)
        {
            InitializeComponent();
            CurrentServ = service;
            DataContext = this;
            LoadServData();
        }

        private void LoadServData()
        {
            if (CurrentServ != null)
            {
                LoadServ(CurrentServ.ID);
            }
        }

        private void LoadServ (int servId)
        {
            ServName.Text = CurrentServ.Service.Name;
            ServCat.Text = CurrentServ.Service.ServCategory.Name;
            ServDate.Text = CurrentServ.Date.ToString("dd.MM.yyyy");
            ServTime.Text = CurrentServ.Schedule.Time.ToString("HH: mm");
            ServFIO.Text = CurrentServ.User.ToString();
            ServPhone.Text = CurrentServ.User.PhoneNumber;
        }

        private void CloseZapis_Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show( "Вы уверены, что хотите закрыть запись?", "Закрытие записи", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {

            }
        }
    }
}
