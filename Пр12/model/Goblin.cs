using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class Goblin : Enemy
    {
        public Goblin()
        {
            Name = "Гоблин";
            MaxHP = 15;
            CurrentHP = 15;
            Attack = 4;
            Defense = 2;
        }

        public override int GetDamage(Player player, bool usedDefense)
        {
            Random random = new Random();
            int critRoll = random.Next(0, 5);
            bool isCrit = (critRoll == 0);
            int damage = Attack;

            if (isCrit)
            {
                damage = (int)(damage * 1.5);
                Console.WriteLine("Критический удар!");
            }

            return damage;
        }

        public override string Effect(Player player) => "";
    }
}
