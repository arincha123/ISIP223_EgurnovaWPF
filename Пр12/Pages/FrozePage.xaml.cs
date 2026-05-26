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
    /// Логика взаимодействия для FrozePage.xaml. Иконка этой страницы появляется в боковом меню только тогда, когда пользователь заморожен, и оповещает о том, что пользователь
    /// не может ни с чем взаимодействовать и ему нужно перейти на страницу Профиля
    /// </summary>
    public partial class FrozePage : Page
    {
        public FrozePage()
        {
            InitializeComponent();
        }
    }
}
