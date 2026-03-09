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
using System.Xml.Linq;

namespace Пр12.Pages
{
    /// <summary>
    /// Логика взаимодействия для TypeInfo.xaml
    /// </summary>
    public partial class TypeInfo : Page
    {
        private int selectedTypeId;

        private List<basepart> allPartsOfCurrentType;

        public TypeInfo(parttype Types)
        {
            InitializeComponent();
            selectedTypeId = Types.id;

            allPartsOfCurrentType = Core.Context.basepart.Where(p => p.parttypeid == selectedTypeId).ToList();
            List<string> manufacturers = allPartsOfCurrentType.Select(p => p.manufacturer.name).Distinct().ToList();

            manufacturers.Insert(0, "Все");

            ComboFiltr.ItemsSource = manufacturers;
            ComboFiltr.SelectedIndex = 0;

            NameOfPart.DataContext = Types;
            Selectedparts.ItemsSource = allPartsOfCurrentType;
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = Search.Text.ToLower();
            List<basepart> filteredParts = new List<basepart>(allPartsOfCurrentType);

            if (ComboFiltr.SelectedItem.ToString() != "Все")
            {
                string selectedManufacturer = ComboFiltr.SelectedItem.ToString();
                filteredParts = filteredParts.Where(p => p.manufacturer.name == selectedManufacturer).ToList();
            }

            if (searchText != "")
            {
                filteredParts = filteredParts.Where(p => p.name.ToLower().Contains(searchText)).ToList();
            }

            Selectedparts.ItemsSource = filteredParts;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search_TextChanged(null, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void AddInAssemb_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
