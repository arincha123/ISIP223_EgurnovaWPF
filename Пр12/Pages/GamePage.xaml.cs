using ISIP223_Egurnova.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Пр12.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private Player player;
        private Random random;
        private int turnCount;
        private List<Enemy> currentEnemies;
        private bool isDefending;
        private bool isChoosingItem;
        private Item pendingItem;
        private Enemy selectedEnemy;

        public GamePage()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            player = new Player();
            random = new Random();
            turnCount = 0;
            currentEnemies = new List<Enemy>();
            isDefending = false;
            isChoosingItem = false;
            selectedEnemy = null;

            UpdateUI();
            AddLog("Добро пожаловать в Roguelike игру!", ConsoleColors.SystemColor);
            AddLog("Каждый ход вы будете встречать либо сундук, либо врага.", ConsoleColors.SystemColor);
            AddLog("Каждые 10 ходов вас ждёт встреча с боссом!", ConsoleColors.WarningColor);

            StartTurn();
        }

        private void UpdateUI()
        {
            HpBar.Value = player.CurrentHP;
            AttackTB.Text = $"ATK: {player.GetAttack()}";
            DefenceTB.Text = $"DEF: {player.GetDefense()}";
            FloorText.Text = $"Этаж: {turnCount}";
        }

        private void AddLog(string message, ConsoleColor color = ConsoleColor.White)
        {
            string coloredMessage = message;
            Logs.Text += coloredMessage + "\n";
            LogScrollViewer.ScrollToEnd();
        }

        private void ClearEnemiesPanel()
        {
            EnemiesPanel.Children.Clear();
        }

        private void DisplayEnemies()
        {
            ClearEnemiesPanel();

            foreach (var enemy in currentEnemies)
            {
                var enemyBorder = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.Green),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(10),
                    Padding = new Thickness(10),
                    Cursor = Cursors.Hand,
                    Tag = enemy
                };

                var stackPanel = new StackPanel();

                var image = new Image
                {
                    Source = GetEnemyImage(enemy),
                    Width = 100,
                    Height = 100,
                    Stretch = Stretch.Uniform
                };
                stackPanel.Children.Add(image);

                var nameText = new TextBlock
                {
                    Text = enemy.Name,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                stackPanel.Children.Add(nameText);

                var hpText = new TextBlock
                {
                    Text = $"HP: {enemy.CurrentHP}/{enemy.MaxHP}",
                    Foreground = new SolidColorBrush(Colors.Red),
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                stackPanel.Children.Add(hpText);

                var statsText = new TextBlock
                {
                    Text = $"ATK: {enemy.Attack} DEF: {enemy.Defense}",
                    Foreground = new SolidColorBrush(Colors.LightGray),
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                stackPanel.Children.Add(statsText);

                enemyBorder.Child = stackPanel;

                enemyBorder.MouseLeftButtonDown += Enemy_Click;

                if (selectedEnemy == enemy)
                {
                    enemyBorder.Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
                }

                EnemiesPanel.Children.Add(enemyBorder);
            }
        }

        private BitmapImage GetEnemyImage(Enemy enemy)
        {
            string imagePath = "";

            switch (enemy.Name)
            {
                case "Гоблин":
                    imagePath = "/Images/Enemies/goblin.png";
                    break;
                case "Скелет":
                    imagePath = "/Images/Enemies/skeleton.png";
                    break;
                case "Маг":
                    imagePath = "/Images/Enemies/mage.png";
                    break;
                case "Архимаг C++":
                    imagePath = "/Images/Enemies/archmage.png";
                    break;
                case "Босс гоблинов ВВГ":
                    imagePath = "/Images/Enemies/boss_goblin.png";
                    break;
                case "Босс скелет Ковальский":
                    imagePath = "/Images/Enemies/boss_skeleton.png";
                    break;
                case "Пестов С--":
                    imagePath = "/Images/Enemies/boss_pestov.png";
                    break;
                default:
                    imagePath = "/Images/Enemies/default.png";
                    break;
            }

            return new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        private void Enemy_Click(object sender, MouseButtonEventArgs e)
        {
            if (isChoosingItem) return;

            var border = sender as Border;
            if (border?.Tag is Enemy enemy)
            {
                selectedEnemy = enemy;
                DisplayEnemies();
                AddLog($"Вы выбрали {enemy.Name} для атаки!", ConsoleColors.SystemColor);
            }
        }

        private void StartTurn()
        {
            if (player.IsFrozen)
            {
                AddLog("Вы заморожены и пропускаете ход!", ConsoleColors.WarningColor);
                player.IsFrozen = false;
                ContinueGame();
                return;
            }

            turnCount++;
            AddLog($"=== Ход {turnCount} ===", ConsoleColors.SystemColor);
            UpdateUI();

            int eventType = random.Next(2);

            if (turnCount % 10 == 0)
            {
                SpawnBoss();
            }
            else if (eventType == 0)
            {
                SpawnEnemies();
            }
            else
            {
                OpenChest();
            }
        }

        private void SpawnEnemies()
        {
            int enemyCount = random.Next(1, 4);
            currentEnemies.Clear();
            selectedEnemy = null;

            AddLog($"Появляется {enemyCount} враг(ов)!", ConsoleColors.WarningColor);

            for (int i = 0; i < enemyCount; i++)
            {
                int enemyType = random.Next(0, 3);
                Enemy enemy;

                switch (enemyType)
                {
                    case 0:
                        enemy = new Goblin();
                        break;
                    case 1:
                        enemy = new Skeleton();
                        break;
                    case 2:
                        enemy = new Mage();
                        break;
                    default:
                        enemy = new Goblin();
                        break;
                }

                currentEnemies.Add(enemy);
                AddLog($"- {enemy.Name} (HP: {enemy.CurrentHP}, ATK: {enemy.Attack}, DEF: {enemy.Defense})", ConsoleColors.EnemyColor);
            }

            DisplayEnemies();
        }

        private void SpawnBoss()
        {
            currentEnemies.Clear();
            selectedEnemy = null;

            int bossType = random.Next(0, 4);
            Enemy boss;

            switch (bossType)
            {
                case 0:
                    boss = new BossVVG();
                    break;
                case 1:
                    boss = new BossKovalsky();
                    break;
                case 2:
                    boss = new BossArchmage();
                    break;
                case 3:
                    boss = new BossPestov();
                    break;
                default:
                    boss = new BossVVG();
                    break;
            }

            currentEnemies.Add(boss);
            AddLog($"!!! Появляется БОСС - {boss.Name} !!!", ConsoleColors.BossColor);
            AddLog($"HP: {boss.CurrentHP}, ATK: {boss.Attack}, DEF: {boss.Defense}", ConsoleColors.BossColor);

            DisplayEnemies();
        }

        private void OpenChest()
        {
            AddLog("Вы нашли сундук!", ConsoleColors.ItemColor);

            int chestContent = random.Next(10);

            if (chestContent < 3)
            {
                player.Heal();
                AddLog("В сундуке лечебное зелье! Ваше здоровье полностью восстановлено!", ConsoleColors.HealColor);
                UpdateUI();
                ContinueGame();
            }
            else
            {
                pendingItem = ItemFactory.RandomItem();
                AddLog($"В сундуке: {pendingItem.Name}!", ConsoleColors.ItemColor);

                if (pendingItem is Weapon weapon)
                {
                    AddLog($"Ваше текущее оружие: {player.Weapon}", ConsoleColors.ItemColor);
                    AddLog($"Новое оружие: {weapon}", ConsoleColors.ItemColor);
                }
                else if (pendingItem is Armor armor)
                {
                    AddLog($"Ваша текущая броня: {player.Armor}", ConsoleColors.ItemColor);
                    AddLog($"Новая броня: {armor}", ConsoleColors.ItemColor);
                }

                isChoosingItem = true;
                SelectBtn.Visibility = Visibility.Visible;
                DiscardBtn.Visibility = Visibility.Visible;
                AttackBtn.Visibility = Visibility.Collapsed;
                DefenceBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pendingItem is Weapon weapon)
            {
                player.Weapon = weapon;
                AddLog($"Вы экипировали: {weapon.Name}", ConsoleColors.SystemColor);
            }
            else if (pendingItem is Armor armor)
            {
                player.Armor = armor;
                AddLog($"Вы экипировали: {armor.Name}", ConsoleColors.SystemColor);
            }

            isChoosingItem = false;
            pendingItem = null;
            SelectBtn.Visibility = Visibility.Collapsed;
            DiscardBtn.Visibility = Visibility.Collapsed;
            AttackBtn.Visibility = Visibility.Visible;
            DefenceBtn.Visibility = Visibility.Visible;

            UpdateUI();
            ContinueGame();
        }

        private void DiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            AddLog($"Вы отказались от {pendingItem.Name} и оставили его в сундуке.", ConsoleColors.WarningColor);

            isChoosingItem = false;
            pendingItem = null;

            SelectBtn.Visibility = Visibility.Collapsed;
            DiscardBtn.Visibility = Visibility.Collapsed;
            AttackBtn.Visibility = Visibility.Visible;
            DefenceBtn.Visibility = Visibility.Visible;

            ContinueGame();
        }

        private void AttackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isChoosingItem) return;

            if (selectedEnemy == null)
            {
                AddLog("Сначала выберите врага для атаки!", ConsoleColors.WarningColor);
                return;
            }

            if (!currentEnemies.Contains(selectedEnemy))
            {
                AddLog("Выбранный враг уже побежден!", ConsoleColors.WarningColor);
                selectedEnemy = null;
                DisplayEnemies();
                return;
            }

            int playerDamage = player.GetAttack();
            selectedEnemy.TakeDamage(playerDamage);
            AddLog($"Вы наносите {playerDamage} урона {selectedEnemy.Name}!", ConsoleColors.DamageColor);

            if (!selectedEnemy.IsAlive())
            {
                AddLog($"{selectedEnemy.Name} повержен!", ConsoleColors.SystemColor);
                currentEnemies.Remove(selectedEnemy);
                selectedEnemy = null;
                DisplayEnemies();

                if (currentEnemies.Count == 0)
                {
                    AddLog("Все враги побеждены!", ConsoleColors.HealColor);
                    ContinueGame();
                    return;
                }
            }
            else
            {
                DisplayEnemies();
            }

            foreach (var enemy in currentEnemies.ToList())
            {
                if (!enemy.IsAlive()) continue;

                AddLog($"\nХод {enemy.Name}:", ConsoleColors.EnemyColor);

                int enemyDamage = enemy.GetDamage(player, isDefending);
                int finalDamage = enemyDamage;

                if (isDefending)
                {
                    int dodgeRoll = random.Next(10);
                    if (dodgeRoll < 4)
                    {
                        AddLog("Вы успешно уклонились от атаки!", ConsoleColors.HealColor);
                        finalDamage = 0;
                    }
                    else
                    {
                        int blockPower = random.Next(4);
                        double blockPercent;

                        switch (blockPower)
                        {
                            case 0:
                                blockPercent = 0.7;
                                break;
                            case 1:
                                blockPercent = 0.8;
                                break;
                            case 2:
                                blockPercent = 0.9;
                                break;
                            default:
                                blockPercent = 1.0;
                                break;
                        }

                        int blockedDamage = (int)(player.GetDefense() * blockPercent);
                        finalDamage = Math.Max(0, enemyDamage - blockedDamage);
                        AddLog($"Вы блокируете {blockedDamage} урона!", ConsoleColors.SystemColor);
                    }
                    isDefending = false;
                }

                if (finalDamage > 0)
                {
                    player.TakeDamage(finalDamage);
                    AddLog($"{enemy.Name} наносит вам {finalDamage} урона!", ConsoleColors.DamageColor);
                    UpdateUI();
                }

                string specialEffect = enemy.Effect(player);
                if (!string.IsNullOrEmpty(specialEffect))
                {
                    AddLog(specialEffect, ConsoleColors.WarningColor);
                }

                AddLog($"Ваше здоровье: {player.CurrentHP}/{player.MaxHP}", ConsoleColors.PlayerColor);

                if (!player.IsAlive())
                {
                    AddLog("Вы пали в бою... Игра окончена!", ConsoleColors.DamageColor);
                    ShowGameOver();
                    return;
                }
            }

            DisplayEnemies();

            bool allEnemiesDead = true;
            foreach (var enemy in currentEnemies)
            {
                if (enemy.IsAlive())
                {
                    allEnemiesDead = false;
                    break;
                }
            }
        }

        private void DefenceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isChoosingItem) return;

            isDefending = true;
            AddLog("Вы готовитесь к защите...", ConsoleColors.SystemColor);

            foreach (var enemy in currentEnemies.ToList())
            {
                if (!enemy.IsAlive()) continue;

                AddLog($"\nХод {enemy.Name}:", ConsoleColors.EnemyColor);

                int enemyDamage = enemy.GetDamage(player, true);
                int finalDamage = enemyDamage;

                int dodgeRoll = random.Next(10);
                if (dodgeRoll < 4)
                {
                    AddLog("Вы успешно уклонились от атаки!", ConsoleColors.HealColor);
                    finalDamage = 0;
                }
                else
                {
                    int blockPower = random.Next(4);
                    double blockPercent;

                    switch (blockPower)
                    {
                        case 0:
                            blockPercent = 0.7;
                            break;
                        case 1:
                            blockPercent = 0.8;
                            break;
                        case 2:
                            blockPercent = 0.9;
                            break;
                        default:
                            blockPercent = 1.0;
                            break;
                    }

                    int blockedDamage = (int)(player.GetDefense() * blockPercent);
                    finalDamage = Math.Max(0, enemyDamage - blockedDamage);
                    AddLog($"Вы блокируете {blockedDamage} урона!", ConsoleColors.SystemColor);
                }

                if (finalDamage > 0)
                {
                    player.TakeDamage(finalDamage);
                    AddLog($"{enemy.Name} наносит вам {finalDamage} урона!", ConsoleColors.DamageColor);
                    UpdateUI();
                }

                string specialEffect = enemy.Effect(player);
                if (!string.IsNullOrEmpty(specialEffect))
                {
                    AddLog(specialEffect, ConsoleColors.WarningColor);
                }

                AddLog($"Ваше здоровье: {player.CurrentHP}/{player.MaxHP}", ConsoleColors.PlayerColor);

                if (!player.IsAlive())
                {
                    AddLog("Вы пали в бою... Игра окончена!", ConsoleColors.DamageColor);
                    ShowGameOver();
                    return;
                }
            }

            isDefending = false;
            DisplayEnemies();

            bool allEnemiesDead = true;
            foreach (var enemy in currentEnemies)
            {
                if (enemy.IsAlive())
                {
                    allEnemiesDead = false;
                    break;
                }
            }

            if (allEnemiesDead)
            {
                AddLog("Все враги побеждены!", ConsoleColors.HealColor);
                ContinueGame();
            }
        }

        private void ContinueGame()
        {
            currentEnemies.Clear();
            selectedEnemy = null;
            ClearEnemiesPanel();
            StartTurn();
        }

        private void ClearLog()
        {
            Logs.Text = "";
        }
        private void ShowGameOver()

        {
            ClearLog();
            ClearEnemiesPanel();
            var result = MessageBox.Show("Игра окончена!\nХотите начать заново?", "Game Over", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                InitializeGame();
            }
            else
            {
                NavigationService?.GoBack();
            }
        }
    }
}