using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пр12
{
    static public class DataOfUser
    {
        public static User curuser;
        public static Cart UserCart { get; set; }
        public static bool isLoged => curuser != null;
    }
}
