using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пр12
{
    internal class UsersAssemblies
    {
        static public List<basepart> UsAssembl = new List<basepart>();
        static public decimal TotalAmount = 0;
        static public string username;
        static public void Sort()
        {
            UsAssembl = UsAssembl.OrderBy(p => p.parttype.id).ToList();
        }

    }
}
