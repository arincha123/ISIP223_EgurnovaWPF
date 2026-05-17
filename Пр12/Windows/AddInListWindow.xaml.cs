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
using System.Windows.Shapes;

namespace Пр12.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddInListWindow.xaml
    /// </summary>
    public partial class AddInListWindow : Window
    {
        public int SelectedListStatusID { get; private set; }

        public AddInListWindow()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ListComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null && selectedItem.Tag != null)
            {
                SelectedListStatusID = int.Parse(selectedItem.Tag.ToString());
            }

            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
