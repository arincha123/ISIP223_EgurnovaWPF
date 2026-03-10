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
    /// Логика взаимодействия для PageOfFullSborka.xaml
    /// </summary>
    public partial class PageOfFullSborka : Page
    {
        public PageOfFullSborka()
        {
            InitializeComponent();
            Loaded += PageOfFullSborka_Loaded;
            List<parttype> parttypes = Core.Context.parttype.ToList();
            PartTypesList.ItemsSource = parttypes;
        }

        private void PageOfFullSborka_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSelectedComponentsList();
            UpdateTotalPrice();
        }

        private void UpdateSelectedComponentsList()
        {
            SelectedComponentsList.ItemsSource = null;
            SelectedComponentsList.ItemsSource = UsersAssemblies.UsAssembl;
            UpdateTotalPrice();
        }

        private void UpdateTotalPrice()
        {
            UsersAssemblies.TotalAmount = UsersAssemblies.UsAssembl.Sum(p => p.price);

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (UsersAssemblies.UsAssembl.Count == 0)
                return;

            MessageBoxResult result = MessageBox.Show(
                "Очистить всю сборку?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                UsersAssemblies.UsAssembl.Clear();
                UsersAssemblies.TotalAmount = 0;
                UsersAssemblies.Sort();
                UpdateSelectedComponentsList();
            }
        }

        private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new YourAssembly());
        }

        private void SelectEl_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            parttype pt = (parttype)bt.DataContext;
            NavigationService.Navigate(new TypeInfo(pt));
        }

        private void DelFromSborka_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            basepart partToRemove = (basepart)button.DataContext;

            MessageBoxResult result = MessageBox.Show(
                $"Удалить {partToRemove.name} из сборки?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                UsersAssemblies.UsAssembl.Remove(partToRemove);
                UsersAssemblies.Sort();
                UpdateSelectedComponentsList();
            }
        }
    }
}
