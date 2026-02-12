using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Пр12.Pages
{
    public class AgeRatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id)
            {
                if (id == 1) return "0+";
                else if (id == 2) return "6+";
                else if (id == 3) return "12+";
                else if (id == 4) return "16+";
                else if (id == 5) return "18+";
                else return id.ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
