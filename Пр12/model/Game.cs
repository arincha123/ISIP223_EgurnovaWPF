using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP223_Egurnova.Model
{
    internal class Game
    {
        private Player player;
        private Random random;
        private int turnCount;
        private List<Item> possibleItems;

        public Game()
        {
            player = new Player();
            random = new Random();
            turnCount = 0;
        }

        public void Start()
        {
            Console.Title = "Текстовый Рогалик";
            Console.Clear();

            ConsoleHelper.WriteLineColor("Добро пожаловать в текстовый рогалик!", ConsoleColors.SystemColor);
            ConsoleHelper.WriteLineColor("Каждый ход вы будете встречать либо сундук, либо врага.", ConsoleColors.MenuColor);
            ConsoleHelper.WriteLineColor("Каждые 10 ходов вас ждёт встреча с боссом!\n", ConsoleColors.WarningColor);

            while (player.IsAlive())
            {
                turnCount++;
                ConsoleHelper.PrintSeparator();
                ConsoleHelper.WriteLineColor($"=== Ход {turnCount} ===", ConsoleColors.SystemColor);
                ConsoleHelper.WriteLineColor(player.ToString(), ConsoleColors.PlayerColor);

                if (player.IsFrozen)
                {
                    ConsoleHelper.WriteLineColor("Вы заморожены и пропускаете ход!", ConsoleColors.WarningColor);
                    player.IsFrozen = false;
                    ContinueGame();
                    continue;
                }

                int eventType = random.Next(0, 2);
                switch (eventType)
                {
                    case 0:
                        EncounterEnemy();
                        break;
                    case 1:
                        OpenChest();
                        break;
                }

                if (!player.IsAlive())
                {
                    ConsoleHelper.PrintSeparator();
                    ConsoleHelper.WriteLineColor("=== ИГРА ОКОНЧЕНА ===", ConsoleColors.DamageColor);
                    ConsoleHelper.WriteLineColor($"Вы продержались {turnCount} ходов.", ConsoleColors.SystemColor);
                    break;
                }

                ContinueGame();
            }
        }

        private void EncounterEnemy()
        {
            Enemy enemy;

            if (turnCount % 10 == 0)
            {
                int bossType = random.Next(0, 4);
                switch (bossType)
                {
                    case 0: enemy = new BossVVG(); break;
                    case 1: enemy = new BossKovalsky(); break;
                    case 2: enemy = new BossArchmage(); break;
                    case 3: enemy = new BossPestov(); break;
                    default: enemy = new BossVVG(); break;
                }
                ConsoleHelper.WriteLineColor($"\n!!! Появляется БОСС - {enemy.Name} !!!", ConsoleColors.BossColor);
            }
            else
            {
                int enemyType = random.Next(0, 3);
                switch (enemyType)
                {
                    case 0: enemy = new Goblin(); break;
                    case 1: enemy = new Skeleton(); break;
                    case 2: enemy = new Mage(); break;
                    default: enemy = new Goblin(); break;
                }
                ConsoleHelper.WriteLineColor($"\nПоявляется враг - {enemy.Name}!", ConsoleColors.EnemyColor);
            }

            ConsoleHelper.WriteLineColor(enemy.ToString(), ConsoleColors.EnemyColor);
            Battle(enemy);
        }

        private void Battle(Enemy enemy)
        {
            while (player.IsAlive() && enemy.IsAlive())
            {
                ConsoleHelper.WriteLineColor("\nВаш ход:", ConsoleColors.MenuColor);
                ConsoleHelper.WriteLineColor("1 - Атаковать", ConsoleColors.MenuColor);
                ConsoleHelper.WriteLineColor("2 - Защищаться", ConsoleColors.MenuColor);
                ConsoleHelper.WriteColor("Выберите действие: ", ConsoleColors.InputColor);

                string input = Console.ReadLine();
                bool usedDefense = false;

                if (input == "1")
                {
                    int playerDamage = player.GetAttack();
                    enemy.TakeDamage(playerDamage);
                    ConsoleHelper.WriteLineColor($"Вы наносите {playerDamage} урона!", ConsoleColors.DamageColor);
                }
                else if (input == "2")
                {
                    usedDefense = true;
                    ConsoleHelper.WriteLineColor("Вы готовитесь к защите...", ConsoleColors.SystemColor);
                }
                else
                {
                    ConsoleHelper.WriteLineColor("Неверный ввод, вы пропускаете ход!", ConsoleColors.WarningColor);
                }

                if (!enemy.IsAlive())
                {
                    ConsoleHelper.WriteLineColor($"\n{enemy.Name} повержен!", ConsoleColors.SystemColor);
                    return;
                }

                ConsoleHelper.WriteLineColor($"\nХод {enemy.Name}:", ConsoleColors.EnemyColor);

                int enemyDamage = enemy.GetDamage(player, usedDefense);
                int finalDamage = enemyDamage;

                if (usedDefense)
                {
                    int dodgeRoll = random.Next(0, 10);
                    if (dodgeRoll < 4)
                    {
                        ConsoleHelper.WriteLineColor("Вы успешно уклонились от атаки!", ConsoleColors.HealColor);
                        finalDamage = 0;
                    }
                    else
                    {
                        int blockPower = random.Next(0, 4);
                        double blockPercent = 0;
                        switch (blockPower)
                        {
                            case 0: blockPercent = 0.7; break;
                            case 1: blockPercent = 0.8; break;
                            case 2: blockPercent = 0.9; break;
                            case 3: blockPercent = 1.0; break;
                            default: blockPercent = 0.8; break;
                        }
                        int blockedDamage = (int)(player.GetDefense() * blockPercent);
                        finalDamage = Math.Max(0, enemyDamage - blockedDamage);
                        ConsoleHelper.WriteLineColor($"Вы блокируете {blockedDamage} урона!", ConsoleColors.SystemColor);
                    }
                }

                if (finalDamage > 0)
                {
                    player.TakeDamage(finalDamage);
                    ConsoleHelper.WriteLineColor($"{enemy.Name} наносит вам {finalDamage} урона!", ConsoleColors.DamageColor);
                }

                string specialEffect = enemy.Effect(player);
                if (!string.IsNullOrEmpty(specialEffect))
                {
                    ConsoleHelper.WriteLineColor(specialEffect, ConsoleColors.WarningColor);
                }

                ConsoleHelper.WriteLineColor($"\nСостояние после раунда:", ConsoleColors.SystemColor);
                ConsoleHelper.WriteLineColor(player.ToString(), ConsoleColors.PlayerColor);
                ConsoleHelper.WriteLineColor(enemy.ToString(), ConsoleColors.EnemyColor);

                if (!player.IsAlive())
                {
                    ConsoleHelper.WriteLineColor("\nВы пали в бою...", ConsoleColors.DamageColor);
                    return;
                }

                ContinueGame();
            }
        }

        private void OpenChest()
        {
            ConsoleHelper.WriteLineColor("\nВы нашли сундук!", ConsoleColors.ItemColor);

            int chestContent = random.Next(0, 10);
            switch (chestContent)
            {
                case 0:
                case 1:
                case 2:
                    ConsoleHelper.WriteLineColor("В сундуке лечебное зелье!", ConsoleColors.HealColor);
                    player.Heal();
                    ConsoleHelper.WriteLineColor("Ваше здоровье полностью восстановлено!", ConsoleColors.HealColor);
                    break;
                default:
                    int itemIndex = random.Next(0, possibleItems.Count);
                    Item foundItem = possibleItems[itemIndex];
                    ConsoleHelper.WriteLineColor($"В сундуке: {foundItem}", ConsoleColors.ItemColor);

                    ConsoleHelper.WriteLineColor("\nВаша текущая экипировка:", ConsoleColors.MenuColor);

                    if (foundItem is Weapon)
                    {
                        Weapon foundWeapon = (Weapon)foundItem;
                        ConsoleHelper.WriteLineColor($"Оружие: {player.Weapon}", ConsoleColors.ItemColor);
                        ConsoleHelper.WriteColor("\nХотите взять новое оружие? (y/n): ", ConsoleColors.InputColor);

                        string input = Console.ReadLine().ToLower();
                        if (input == "y" || input == "д")
                        {
                            player.Weapon = foundWeapon;
                            ConsoleHelper.WriteLineColor($"Вы экипировали: {foundWeapon.Name}", ConsoleColors.SystemColor);
                        }
                        else
                        {
                            ConsoleHelper.WriteLineColor("Вы оставили оружие в сундуке.", ConsoleColors.WarningColor);
                        }
                    }
                    else if (foundItem is Armor)
                    {
                        Armor foundArmor = (Armor)foundItem;
                        ConsoleHelper.WriteLineColor($"Броня: {player.Armor}", ConsoleColors.ItemColor);
                        ConsoleHelper.WriteColor("\nХотите взять новую броню? (y/n): ", ConsoleColors.InputColor);

                        string input = Console.ReadLine().ToLower();
                        if (input == "y" || input == "д")
                        {
                            player.Armor = foundArmor;
                            ConsoleHelper.WriteLineColor($"Вы экипировали: {foundArmor.Name}", ConsoleColors.SystemColor);
                        }
                        else
                        {
                            ConsoleHelper.WriteLineColor("Вы оставили броню в сундуке.", ConsoleColors.WarningColor);
                        }
                    }
                    break;
            }
        }

        private void ContinueGame()
        {
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
}
