using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal abstract class Item
    {
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        protected Item(string name, int attack, int defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }
    }
}
