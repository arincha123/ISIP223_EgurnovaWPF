using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class BossKovalsky : Skeleton
    {
        public BossKovalsky()
        {
            Name = "Босс скелет Ковальский";
            MaxHP = (int)(25 * 2.5);
            CurrentHP = MaxHP;
            Attack = (int)(10 * 1.3);
            Defense = (int)(3 * 1.4);
        }
    }
}
