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
    /// Логика взаимодействия для TypeInfo.xaml
    /// </summary>
    public partial class TypeInfo : Page
    {
        public List<basepart> baseparts { get; set; }
        public List<string> man {  get; set; }
        public TypeInfo(parttype Types)
        {
            InitializeComponent();
            baseparts = Core.Context.basepart.Where(p => p.parttypeid == Types.id).ToList();
            man = baseparts.Select(p => p.manufacturer.name).Distinct().ToList();
            man.Insert(0, "Все");
            ComboFiltr.ItemsSource = man;
            NameOfPart.DataContext = Types;
            
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboFiltr.SelectedItem.ToString() == "Все")
            {
                Selectedparts.ItemsSource = baseparts;
            }
            else Selectedparts.ItemsSource = baseparts.Where(p => p.manufacturer.name == ComboFiltr.SelectedItem.ToString());
        }
    }
}
