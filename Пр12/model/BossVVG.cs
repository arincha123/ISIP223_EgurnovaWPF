using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class BossVVG : Goblin
    {
        public BossVVG()
        {
            Name = "Босс гоблинов ВВГ";
            MaxHP = (int)(30 * 2.0);
            CurrentHP = MaxHP;
            Attack = (int)(8 * 1.5);
            Defense = (int)(2 * 1.2);
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
    }
}
