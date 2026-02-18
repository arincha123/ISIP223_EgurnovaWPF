using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Пр12.Pages
{
    public class ColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SEAT_IN_SESSION ss = Core.Context.SEAT_IN_SESSION.FirstOrDefault(s => s.id_seat == (int)value);
            bool status = ss.STATUS;

            if (status == true)
                return "#4CAF50";
            else if (status == false)
                return "#F44336";
            else 
                return "#9E9E9E";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
