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
            MaxHP = 20;
            CurrentHP = 20;
            Attack = 8;
            Defense = 3;
        }

        public override int GetDamage(Player player, bool usedDefense)
        {
            return Attack;
        }

        public override string Effect(Player player) => "";
    }
}
