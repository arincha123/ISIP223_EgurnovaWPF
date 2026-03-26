using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class Player
    {
        public int MaxHP { get; set; } = 100;
        public int CurrentHP { get; set; } = 100;
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public bool IsFrozen { get; set; } = false;

        public Player()
        {
            Weapon = ItemFactory.startWeapon();
            Armor = ItemFactory.startArmor();
        }

        public int GetAttack()
        {
            if (Weapon != null)
            {
                return Weapon.Attack;
            }
            else
            {
                return 0;
            }
        }

        public int GetDefense()
        {
            if (Armor != null)
            {
                return Armor.Defense;
            }
            else
            {
                return 0;
            }
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public void Heal()
        {
            CurrentHP = MaxHP;
        }

        public bool IsAlive() => CurrentHP > 0;

        public override string ToString()
        {
            return $"Игрок: HP {CurrentHP}/{MaxHP}, Атака: {GetAttack()}, Защита: {GetDefense()}";
        }
    }
}
