using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class Slime : Enemy
    {
        public Slime()
        {
            Name = "Слизень";
            MaxHP = 5;
            CurrentHP = 5;
            Attack = 5;
            Defense = 2;

        }

        public override void TakeDamage(int damage)
        {
            int nuwdamage = damage - 2;
            CurrentHP -= nuwdamage;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public override int GetDamage(Player player, bool usedDefense)
        {
            return Attack;
        }

        public override string Effect(Player player) => "";
    }
}
