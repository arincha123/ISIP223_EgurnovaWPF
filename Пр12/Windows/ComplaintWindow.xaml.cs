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
        private int ComplaintType;
        private string TargetName;
        private int BookId;
        private int? ReviewId;

        public ComplaintWindow(int complaintType, string targetName, int bookId, int? reviewId = null)
        {
            InitializeComponent();


            ComplaintType = complaintType;
            TargetName = targetName;
            BookId = bookId;
            ReviewId = reviewId;

            List<string> reasons = Core.Context.TypeOfComplaint.Select(r => r.Reason).ToList();
            reasons.Insert(0, "Выберите причину");
            CmbReasons.ItemsSource = reasons;
            CmbReasons.SelectedIndex = 0;

            string typeText = "";
            switch (complaintType)
            {
                case 1: typeText = "книгу"; break;
                case 2: typeText = "отзыв"; break;
                case 3: typeText = "автора"; break;
            }

            TxtTargetInfo.Text = $"Жалоба на {typeText}";
            TxtTargetName.Text = targetName;
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            string selectedReason = CmbReasons.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedReason) || selectedReason == "Выберите причину")
            {
                MessageBox.Show("Выберите причину жалобы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Отправить жалобу на {TargetName}?\nПричина: {selectedReason}", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                Complaint newComplaint = new Complaint();
                newComplaint.UserID = UserData.curUser.ID;
                newComplaint.ReasonID = CmbReasons.SelectedIndex;
                newComplaint.Date = DateTime.Now;

                switch (ComplaintType)
                {
                    case 1:
                        newComplaint.BookID = BookId;
                        newComplaint.ReviewID = null;
                        break;

                    case 2:
                        newComplaint.BookID = BookId;
                        newComplaint.ReviewID = ReviewId;
                        break;

                    case 3:
                        newComplaint.BookID = BookId;
                        newComplaint.ReviewID = null;
                        break;
                }

                Core.Context.Complaint.Add(newComplaint);
                Core.Context.SaveChanges();

                MessageBox.Show("Жалоба отправлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}