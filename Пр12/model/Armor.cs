using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Пр12;

namespace ISIP223_Egurnova.Model
{
    public enum ArmorType
    {
        Light,
        Medium,
        Heavy
    }
    internal class Armor : Item
    {
        public ArmorType TypeArmor { get; set; }
        public bool IsWeapon => false;

        public Armor(string name, int attack, int defense, ArmorType type)
            : base(name, attack, defense)
        {
            TypeArmor = type;
        }

        public override string ToString()
        {
            string typeStr;

            if (TypeArmor == ArmorType.Light)
                typeStr = "Лёгкая";
            else if (TypeArmor == ArmorType.Medium)
                typeStr = "Средняя";
            else if (TypeArmor == ArmorType.Heavy)
                typeStr = "Тяжёлая";
            else
                typeStr = "Неизвестно";

            return $"{Name} [Тип: {typeStr}, АТК: {Attack}, ЗАЩ: {Defense}]";
        }
    }
}
