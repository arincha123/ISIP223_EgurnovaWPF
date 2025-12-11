using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пр12
{
    public static class User
    {
        static public string Model { get; set; } = "";
        static public int ModelCost { get; set; }
        static public string Type { get; set; } = "";
        static public int TypeCost { get; set; }
        static public string Color { get; set; } = "";
        static public int ColorCost { get; set; }
        static public List<string> Options { get; set; } = new List<string>();
        static public string StrOptions { get; set; } = "";
        static public int OptionsCost { get; set; }
        static public int TOTAL { get; set; }
        static public int BASE { get; set; }
        static public int ADD { get; set; }
        static public string Name { get; set; } = "";
        static public string Phone { get; set; } = "";
        static public string Email { get; set; } = "";

    }
}
