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
                new Weapon("Стальной меч", 10, 0, WeaponType.Melee, "/Images/Weap/стальной меч.png"),
                new Weapon("Секира воина", 15, 0, WeaponType.Melee, "/Images/Weap/секира воина.png"),
                new Weapon("Кинжал убийцы", 8, 0, WeaponType.Melee, "/Images/Weap/кинжал убийцы.png"),
                new Weapon("Легендарный клинок", 20, 5, WeaponType.Melee, "/Images/Weap/клинок.png"),

                new Weapon("Длинный лук", 12, 0, WeaponType.Ranged, "/Images/Weap/лук.png"),
                new Weapon("Арбалет снайпера", 18, 0, WeaponType.Ranged, "/Images/Weap/снайп арб.png"),
                new Weapon("Боевой посох", 6, 0, WeaponType.Ranged, "/Images/Weap/боев пос.png"),

                new Weapon("Магический посох", 12, 0, WeaponType.Magic, "/Images/Weap/маг посох.png"),
                new Weapon("Книга заклинаний", 14, 0, WeaponType.Magic, "/Images/Weap/книга закл.png"),
                new Weapon("Кристальный скипетр", 16, 0, WeaponType.Magic, "/Images/Weap/кристал.png"),

                new Armor("Кожаный доспех", 0, 5, ArmorType.Light, "/Images/Armor/кож дос.png"),
                new Armor("Одежды мага", 0, 4, ArmorType.Light, "/Images/Armor/од мага.png"),
                new Armor("Плащ разведчика", 0, 3, ArmorType.Light, "/Images/Armor/развед.png"),

                new Armor("Кольчуга", 0, 8, ArmorType.Medium, "/Images/Armor/кольчуг.png"),
                new Armor("Чешуйчатая броня", 0, 10, ArmorType.Medium, "/Images/Armor/чеш бр.png"),
                new Armor("Бригантина", 0, 12, ArmorType.Medium, "/Images/Armor/бригант.png"),

                new Armor("Латные доспехи", 0, 15, ArmorType.Heavy, "/Images/Armor/латы.png"),
                new Armor("Драконья броня", 0, 18, ArmorType.Heavy, "/Images/Armor/драк брон.png"),
                new Armor("Доспехи чемпиона", 0, 20, ArmorType.Heavy, "/Images/Armor/чемп.png"),
                new Armor("Доспехи чемпиона", 0, 20, ArmorType.Heavy, "/Images/Armor/чемп.png"),
                new Armor("Доспехи чемпиона", 0, 20, ArmorType.Heavy, "/Images/Armor/чемп.png"),
                new Armor("Доспехи чемпиона", 0, 20, ArmorType.Heavy, "/Images/Armor/чемп.png"),
                new Armor("Доспехи чемпиона", 0, 20, ArmorType.Heavy, "/Images/Armor/чемп.png"),
                new Armor("Доспехи чемпиона", 0, 20, ArmorType.Heavy, "/Images/Armor/чемп.png")
        };

        public static Item RandomItem()
        {
            int itemindex = Random.Next(0, items.Count);
            return items[itemindex];
        }

        public static Weapon startWeapon()
        {
            return new Weapon("Ржавый меч", 5, 0, WeaponType.Melee, "/Images/Weap/ржав.png");
        }

        public static Armor startArmor()
        {
            return new Armor("Потрёпанная кожаная броня", 0, 3, ArmorType.Light, "/Images/Armor/кож дос.png");
        }

        public static HealthPotion CreatePotion()
        {
            return new HealthPotion();
        }

    }
}
