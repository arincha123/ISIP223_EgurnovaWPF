using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class Mage : Enemy
    {

        public Mage()
        {
            Name = "Маг";
            MaxHP = 20;
            CurrentHP = 20;
            Attack = 10;
            Defense = 1;
        }

        public override int GetDamage(Player player, bool usedDefense)
        {
            return Attack;
        }

        public override string Effect(Player player)
        {
            Random random = new Random();
            int freezeRoll = random.Next(0, 4);
            bool isFrozen = (freezeRoll == 0);

            if (isFrozen)
            {
                player.IsFrozen = true;
                return "Маг замораживает вас! Вы пропустите следующий ход.";
            }
            return "";
        }
    }
}
