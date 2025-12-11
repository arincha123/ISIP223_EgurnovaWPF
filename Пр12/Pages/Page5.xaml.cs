using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Page5.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        public Page5()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {

                var result = MessageBox.Show("Есть несохранённые изменения. Покинуть страницу?", "Подтверждение",
                 MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    //e.Cancel = true;
                }
                NavigationService?.GoBack();
                MainWindow.MinusProgress();
            }
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            {
                if (sender is TextBox textBox)
                {
                    User.Name = textBox.Text;
                }
            }
        }
        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string digitsOnly = new string(textBox.Text.Where(char.IsDigit).ToArray());
                textBox.Text = digitsOnly;
                User.Phone = digitsOnly;
            }
        }

        private void Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-Я\s]+$");
        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                User.Email = textBox.Text;
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (User.Name.Length > 0 && User.Phone.Length > 0 && User.Email.Length > 0)
            {
                var res = MessageBox.Show($"Сохранить данную заявку?\nВаше имя: {User.Name} \nВаш телефон: {User.Phone}\nВаша почта: {User.Email}\nМодель машины: {User.Model}\nТип двигателя: {User.Type}\nЦвет машины: {User.Color}\nИтоговая сумма: {User.TOTAL}", "Подтверждение", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.No)
                {
                    return;
                }
                else
                {
                    MessageBox.Show("Ваша заявка успешно сохранена");
                }
            }
        }
    }
}
