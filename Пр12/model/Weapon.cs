using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    public enum WeaponType
    {
        Melee,
        Ranged,
        Magic
    }
    internal class Weapon : Item
    {
        public WeaponType TypeWeap { get; set; }
        public bool IsWeapon => true;

        public Weapon(string name, int attack, int defense, WeaponType type)
            : base(name, attack, defense)
        {
            TypeWeap = type;
        }

        public override string ToString()
        {
            string typeStr;

            if (TypeWeap == WeaponType.Melee)
                typeStr = "Холодное";
            else if (TypeWeap == WeaponType.Ranged)
                typeStr = "Дальнострельное";
            else if (TypeWeap == WeaponType.Magic)
                typeStr = "Магия";
            else
                typeStr = "Неизвестно";

            return $"{Name} [Тип: {typeStr}, АТК: {Attack}, ЗАЩ: {Defense}]";
        }
    }
}
