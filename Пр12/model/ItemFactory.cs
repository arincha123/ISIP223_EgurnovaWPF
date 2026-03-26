using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal static class ItemFactory
    {
        private static Random Random = new Random();
        private static List<Item> items = new List<Item>
        {
                new Weapon("Стальной меч", 10, 0, WeaponType.Melee),
                new Weapon("Секира воина", 15, 2, WeaponType.Melee),
                new Weapon("Кинжал убийцы", 8, 0, WeaponType.Melee),
                new Weapon("Легендарный клинок", 20, 5, WeaponType.Melee),

                new Weapon("Длинный лук", 12, 0, WeaponType.Ranged),
                new Weapon("Арбалет снайпера", 18, 1, WeaponType.Ranged),
                new Weapon("Боевой посох", 6, 3, WeaponType.Ranged),

                new Weapon("Магический посох", 12, 3, WeaponType.Magic),
                new Weapon("Книга заклинаний", 14, 2, WeaponType.Magic),
                new Weapon("Кристальный скипетр", 16, 4, WeaponType.Magic),

                new Armor("Кожаный доспех", 0, 5, ArmorType.Light),
                new Armor("Одежды мага", 2, 4, ArmorType.Light),
                new Armor("Плащ разведчика", 1, 3, ArmorType.Light),

                new Armor("Кольчуга", 0, 8, ArmorType.Medium),
                new Armor("Чешуйчатая броня", 1, 10, ArmorType.Medium),
                new Armor("Бригантина", 0, 12, ArmorType.Medium),

                new Armor("Латные доспехи", 0, 15, ArmorType.Heavy),
                new Armor("Драконья броня", 5, 20, ArmorType.Heavy),
                new Armor("Доспехи чемпиона", 3, 18, ArmorType.Heavy)
        };

        public static Item RandomItem()
        {
            int itemindex = Random.Next(0, items.Count);
            return items[itemindex];
        }

        public static Weapon startWeapon()
        {
            return new Weapon("Ржавый меч", 5, 0, WeaponType.Melee);
        }

        public static Armor startArmor()
        {
            return new Armor("Потрёпанная кожаная броня", 0, 3, ArmorType.Light);
        }

        public static HealthPotion CreatePotion()
        {
            return new HealthPotion();
        }

    }
}
