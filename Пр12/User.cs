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


        public static void UpdateStrOptions()
        {
            StrOptions = string.Join(", ", Options);
        }

        public static void UpdateOption(string optionName, int price, bool isSelected)
        {
            if (isSelected)
            {
                if (!Options.Contains(optionName))
                {
                    OptionsCost += price;
                    Options.Add(optionName);
                }
            }
            else
            {
                if (Options.Contains(optionName))
                {
                    OptionsCost -= price;
                    Options.Remove(optionName);
                }
            }
            UpdateStrOptions();
        }
        public static void RecalculateOptionsCost()
        {
            OptionsCost = 0;
            foreach (var option in Options)
            {
                OptionsCost += GetOptionPrice(option);
            }
            UpdateStrOptions();
        }

        private static int GetOptionPrice(string option)
        {
            switch (option)
            {
                case "панорамная крыша": return 8000;
                case "кожаный салон": return 120000;
                case "климат-контроль": return 25000;
                case "круиз-контроль": return 45000;
                default: return 0;
            }
        }
    }
}
