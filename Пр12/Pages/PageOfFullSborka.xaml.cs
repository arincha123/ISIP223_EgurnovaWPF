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
            List<parttype> parttypes = Core.Context.parttype.ToList();
            PartTypesList.ItemsSource = parttypes;
        }

        private void AllDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectedComponentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SelectEl_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            parttype pt = (parttype)bt.DataContext;
            NavigationService.Navigate(new TypeInfo(pt));
        }

        private void DelFromSborka_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
