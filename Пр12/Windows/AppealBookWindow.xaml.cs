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
    /// Логика взаимодействия для AppealBookWindow.xaml
    /// </summary>
    public partial class AppealBookWindow : Window
    {
        public string AppealReason { get; private set; }
        public string BookTitle { get; private set; }

        public AppealBookWindow(string bookTitle)
        {
            InitializeComponent();
            BookTitle = bookTitle;
            Title = $"Оспаривание заморозки - {bookTitle}";
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReasonTextBox.Text))
            {
                MessageBox.Show("Укажите причину для разморозки книги!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ReasonTextBox.Text.Length < 10)
            {
                MessageBox.Show("Пожалуйста, укажите более подробную причину (минимум 10 символов)!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AppealReason = ReasonTextBox.Text.Trim();
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