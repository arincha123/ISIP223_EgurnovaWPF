using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class BossPestov : Skeleton
    {
        private double freezeChance = 0.4;

        public BossPestov()
        {
            Name = "Пестов С-- ";
            MaxHP = (int)(25 * 1.3);
            CurrentHP = MaxHP;
            Attack = (int)(10 * 1.8);
            Defense = (int)(3 * 0.6);
        }

        public override string Effect(Player player)
        {
            Random random = new Random();
            int freezeRoll = random.Next(0, 4);
            bool isFrozen = (freezeRoll == 0);

            if (isFrozen)
            {
                player.IsFrozen = true;
                return "Пестов замораживает вас магией С--! Вы пропустите следующий ход.";
            }
            return "";
        }
    }
}
