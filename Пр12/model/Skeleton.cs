using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class Skeleton : Enemy
    {
        public Skeleton()
        {
            Name = "Скелет";
            MaxHP = 40;
            CurrentHP = 40;
            Attack = 10;
            Defense = 5;
        }

        public override int GetDamage(Player player, bool usedDefense)
        {
            return Attack;
        }

        public override string Effect(Player player) => "";
    }
}
