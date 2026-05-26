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

namespace Пр12.Windows
{
    /// <summary>
    /// Логика взаимодействия для TextOfBook.xaml
    /// </summary>
    public partial class TextOfBook : Window
    {
        /// <summary>
        /// Загрузка текста, выбранной книги
        /// </summary>
        /// <param name="bookTitle"></param>
        /// <param name="bookText"></param>
        public TextOfBook(string bookTitle, string bookText)
        {
            InitializeComponent();
            Title = $"Фрагмент книги: {bookTitle}";
            TextContent.Text = bookText;
        }

        /// <summary>
        /// Кнопка назад / отмены
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
