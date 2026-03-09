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
    /// Логика взаимодействия для YourAssembly.xaml
    /// </summary>
    public partial class YourAssembly : Page
    {
        public List <assembly> Assemblies { get; set; }
        public YourAssembly()
        {
            InitializeComponent();
            Assemblies = Core.Context.assembly.ToList();

            foreach (var assembly in Assemblies)
            {
                var parts = assembly.partassembly.ToList();
            }
            Assemly.ItemsSource = Assemblies;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

    }
}
