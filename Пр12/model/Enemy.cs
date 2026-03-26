using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal abstract class Enemy
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public bool IsAlive() => CurrentHP > 0;

        public virtual void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public abstract int GetDamage(Player player, bool usedDefense);
        public abstract string Effect(Player player);

        public override string ToString()
        {
            return $"{Name}: HP {CurrentHP}/{MaxHP}, Атака: {Attack}, Защита: {Defense}";
        }
    }
}
