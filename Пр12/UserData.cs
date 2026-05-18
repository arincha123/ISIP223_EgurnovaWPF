using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Пр12
{
    public static class UserData
    {
        public static User curUser {  get; set; }
        public static bool IsFrozen => curUser?.IsFrozen ?? false;
        public static bool IsLoginSuccess => curUser != null;
    }
}
