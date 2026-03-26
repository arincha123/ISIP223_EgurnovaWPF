using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class EnemyFactory
    {
        static Random Random = new Random();

        public static Enemy CreateUnit()
        {
            int a = Random.Next(0, 4);
            switch (a)
            {
                case 0: return new Goblin();
                case 1: return new Skeleton();
                case 2: return new Mage();
                case 3: return new Slime();
                default: return new Goblin();
            }
        }

        public static Enemy CreateBoss()
        {
            int a = Random.Next(0, 4);
            switch (a)
            {
                case 0: return new BossVVG();
                case 1: return new BossKovalsky();
                case 2: return new BossPestov();
                case 3: return new BossArchmage();
                default: return new BossVVG();
            }
        }

        public static Enemy CreateEnemy(bool isBoss)
        {
            if (isBoss == true) return CreateBoss();
            else return CreateUnit();
        }
    }
}
