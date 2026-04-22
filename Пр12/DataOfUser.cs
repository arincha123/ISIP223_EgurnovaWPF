using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пр12
{
    static internal class DataOfUser
    {
        public static User curuser;
        public static bool isLoged => curuser != null;
    }
}
