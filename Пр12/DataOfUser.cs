using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пр12
{
    static internal class DataOfUser
    {
        public static User user;
        public static bool isLoged => user != null;
    }
}
