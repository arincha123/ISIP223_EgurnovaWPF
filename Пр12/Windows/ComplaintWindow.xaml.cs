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
    /// Логика взаимодействия для ComplaintWindow.xaml
    /// </summary>
    public partial class ComplaintWindow : Window
    {
        private int? _authorId;
        private int? _bookId;
        private int? _reviewId;
        private string _targetName;
        private string _targetType;
        private TypeOfComplaint _selectedReason;
        private bool isReasonSelected = false;

        /// <summary>
        /// Загрузка окна с уже заполненными параметрами для подачи жалобы
        /// </summary>
        /// <param name="targetType">объект, на который жалуются</param>
        /// <param name="targetName">имя объекта</param>
        /// <param name="authorId">ID автора (если есть)</param>
        /// <param name="bookId">ID книги (если есть)</param>
        /// <param name="reviewId">ID отзыва (если есть)</param>
        public ComplaintWindow(string targetType, string targetName, int? authorId, int? bookId, int? reviewId)
        {
            InitializeComponent();

            _targetType = targetType;
            _targetName = targetName;
            _authorId = authorId;
            _bookId = bookId;
            _reviewId = reviewId;

            TxtBlcComplaintTarget.Text = $"Жалоба на {targetType} «{targetName}»";

            LoadReasons();
        }

        /// <summary>
        /// Загрузка списка причин для жалобы
        /// </summary>
        private void LoadReasons()
        {
            var reasons = Core.Context.TypeOfComplaint.ToList();
            ListBoxReasons.ItemsSource = reasons;
        }

        /// <summary>
        /// Получение конкретной выбранной причины
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            isReasonSelected = true;
            RadioButton radio = (RadioButton)sender;
            _selectedReason = (TypeOfComplaint)radio.DataContext;
        }

        /// <summary>
        /// "Оформление" жалобы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnComplaint_Click(object sender, RoutedEventArgs e)
        {
            if (!isReasonSelected)
            {
                MessageBox.Show("Выберите причину жалобы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Отправить жалобу на {_targetType} «{_targetName}»?\nПричина: {_selectedReason.Reason}",
                "Подтверждение жалобы", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var complaint = new Complaint
                    {
                        UserID = UserData.curUser.ID,
                        AuthorID = _authorId,
                        BookID = _bookId,
                        ReviewID = _reviewId,
                        ReasonID = _selectedReason.ID,
                        Date = DateTime.Now
                    };

                    Core.Context.Complaint.Add(complaint);
                    Core.Context.SaveChanges();

                    MessageBox.Show("Ваша жалоба отправлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Кнопка назад / отмены
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}