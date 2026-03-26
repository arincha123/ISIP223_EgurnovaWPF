using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class BossArchmage : Mage
    {
        public BossArchmage()
        {
            Name = "Архимаг C++";
            MaxHP = (int)(20 * 1.8);
            CurrentHP = MaxHP;
            Attack = (int)(12 * 1.6);
            Defense = (int)(1 * 1.1);
        }

        public override string Effect(Player player)
        {
            Random rand = new Random();
            if (rand.NextDouble() < 0.35)
            {
                player.IsFrozen = true;
                return "Архимаг замораживает вас магией C++! Вы пропустите следующий ход.";
            }
            return "";
        }
    }
}
