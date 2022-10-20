using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Console;

/*
-----TODO-----
a) *FINISHED* Implement a first monster that awards experience
b) *FINISHED* Implement player skills
c) *FINISHED* With a working combat, test the xp and level up methodology
d) *FINISHED* Implement file writing to save the data from a session.
e) *FINISHED* Begin to implement items: the drop chance, drop table, properties...
... and how itemization works along with display on the player info screen

-----TODO-----
0) *FINISHED* Revamp, improve, update, and simplify variables in the player and monster class.
1) *FINISHED* Itemization
2) Ability unlocks and exploreProgress
3) *FINISHED* Money and shop
4) *FINISHED* Many new powers
5) *FINISHED* Fully update the dynamic combat method to include all new powers

9/11/2020 2:05PM
-----TOALSODO-----
Ok while implementing the above list, code has become substantially less organized and elegant.
There needs to be substantial housekeeping, yet also I want to continue making new ground and
reach the above features...
1) overall inventory menu, input operations, invalid input
2) bad use of variables, consider player, monster, and item class
3) player 'temporary' variables which now display for playermenu and set player for dynamiccombatmethod

09/09/2020
For a new save file in NumeralData.txt, have the file
1 - level
0 - xp
30 - life
5 - dmg
0 - life regen
The only stream writers are in totnp.cs when loading player and in Player.cs writing methods...
... reading is also in the PlayerInfoMenu() method in totnp.cs.

----- DEFAULT NumeralData.txt ----- View LoadPlayerData() method for positions.
1
0
1
0
30
0
0
5
0
0
0
0
0
0
0
0
0
0
0

----- DEFAULT ItemData.txt -----
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0

----- DEFAULT StoreData.txt -----
1
2
8
*/

namespace totnb
{
    class totnb
    {
        public static int CombatTurn;
        static char input = ' ';
        static int inputInt;
        static Player player;

        // Main
        static void Main(string[] args)
        {
            LoadPlayerData();
            player.LoadPlayerItems();
            MainMenu();
            Write("\nExiting...\n");
        }


        public static void MainMenu()
        {
            // Menu loop
            do
            {
                // Main menu
                WriteLine("1) Explore");
                WriteLine("2) Player Info"); // Previously: 2) Player Info
                WriteLine("3) Inventory");
                WriteLine("4) Shop");
                WriteLine("0) Exit");

                // Accept input from user
                input = (char)ReadKey().KeyChar; // Simply change to a ReadLine() to support beyond 10 options
                Clear();

                // Validate input
                if (char.IsDigit(input) == false) inputInt = -1;
                else inputInt = int.Parse(input.ToString());
            } while (inputInt < 0 || inputInt > 4);

            // Using the user input to choose a menu
            if (inputInt == 1) ExploreMenu();
            else if (inputInt == 2) PlayerInfoMenu();
            else if (inputInt == 3) InventoryMenu();
            else if (inputInt == 4) ShopMenu();
            else if (inputInt == 0) { Environment.Exit(1); } // Exits program
        }


        public static void ExploreMenu()
        {
            do
            {
                // Explore menu
                for (int i = 0; i < Monster.GetNumerOfZonesUnlocked(player); i++)
                {
                    WriteLine("{0}) {1}", (i + 1), Monster.GetZoneName(i + 1));
                }
                WriteLine("9) Vault");
                WriteLine("0) Main Menu");

                input = (char)ReadKey().KeyChar; // Simply change to a ReadLine() to support beyond 10 options
                Clear();

                // Forces loop to continue if input isn't of the correct format
                if (char.IsDigit(input) == false) inputInt = -1;
                else inputInt = int.Parse(input.ToString());

            } while (inputInt < 0 || (inputInt > Monster.GetNumerOfZonesUnlocked(player) && inputInt != 9));

            // Using the user input to choose a menu
            if (inputInt == 0) MainMenu();

            // EnterAnArea(Monster[] monsters) accepts all the monsters in the zone the player has unlocked
            else if (inputInt > 0 && inputInt < 9) EnterAnArea(Monster.GetUnlockedMonstersForArea(inputInt, player));
            else if (inputInt == 9) EnterAnArea(Monster.GetVaultMonsters());
        }

        public static void EnterAnArea(Monster[] monsters)
        {
            do
            {
                // Displaying each monster in the monsters array
                for (int i = 0; i < monsters.Length; i++)
                {
                    WriteLine("{0}) {1}", i + 1, monsters[i].Display);
                }
                WriteLine("0) Return to exploring");

                // Reading input
                input = (char)ReadKey().KeyChar;
                Clear();

                if (char.IsDigit(input) == false) inputInt = -1;
                else inputInt = int.Parse(input.ToString());

            } while (inputInt < 0 || inputInt > monsters.Length);

            // Using the user input to choose what to do
            if (inputInt == 0) ExploreMenu();
            CombatMenu(monsters[inputInt - 1]);
        }


        // Dynamic combat method
        public static void CombatMenu(Monster enemy)
        {
            // Setting up the foe... copying information from the enemy...
            enemy.TemporaryLife = enemy.Life;
            enemy.TemporaryDamage = enemy.Damage;

            // Setting up the player
            player.SetPlayerValues();
            //Ability[] abilities = Ability.GetAllAbilities();

            int BasePlayerLife = player.TemporaryLife;
            player.Abilities = Ability.GetAllAbilities();
            //int BasePlayerDamage = player.TemporaryDamage;

            // SET VARIABLES PHASE A
            // ... carefully check PHASE B below.
            // ... this is in scope of combat, so things here can impact combat
            CombatTurn = 1; // Not used presently, but may be in the future
            Random r = new Random();
            bool currentlyCritical = false; // 0 is no crit, 1 is crit, 2 is super crit.
            int bonusDamageFromCritical = 0;

            // Account for ability 4: TriBuff
            BuffTwo TriBuff = (BuffTwo)player.Abilities[3];

            // Account for ability 5: Healing Touch
            HealTwo HealingTouch = (HealTwo)player.Abilities[4];
            HealingTouch.IsActive = false;
            int MonsterDamageTaken;

            bool enemyCanMove = true;
            enemy.WillMiss = false;
            player.WillDodge = false;

            while (player.TemporaryLife > 0 && enemy.TemporaryLife > 0)
            {
                do
                {
                    // Displaying all abilities in a menu that the player has unlocked
                    for (int i = 0; i < player.Abilities.Length; i++)
                    {
                        if (player.Abilities[i].RequiredLevel <= player.Level) WriteLine(player.Abilities[i].Display);
                    }
                    WriteLine("0) Leave the encounter");

                    // Displaying player and monster life
                    ForegroundColor = ConsoleColor.Green;
                    WriteLine("{0}{0}{0}{3}{1} <------------------------------> {2}{0}", "\n", player.TemporaryLife, enemy.TemporaryLife, "\t");
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("{0}{1}", "\t", player.TemporaryDamage);
                    ForegroundColor = ConsoleColor.White;

                    // Taking input
                    input = (char)ReadKey().KeyChar;
                    Write("{0}", "\n");
                    Clear();
                    if (char.IsDigit(input) == false) inputInt = -1;
                    else inputInt = int.Parse(input.ToString());

                    // SET VARIABLES PHASE A
                    // ... carefully check PHASE B below.
                    if (TriBuff.IsActive) TriBuff.UsedLast = true;
                    TriBuff.IsActive = false;
                    MonsterDamageTaken = enemy.TemporaryLife; // Subtract post-combat monster life to get result

                } while (inputInt < 0 || inputInt > player.Abilities.Length); // Checks if input is valid

                // Using the user input to decide a move
                if (inputInt == 0) ExploreMenu();
                foreach (Ability ability in player.Abilities)
                {
                    if (inputInt == ability.Number && ability.RequiredLevel <= player.Level)
                    {



                        // --------------------------------------------------COMBAT_AREA--------------------------------------------------
                        // Executing the player's move, then the foe's move (if it survives)
                        // --------------------------------------------------COMBAT_AREA--------------------------------------------------

                        // Player move
                        enemy.TemporaryLife -= ability.UseAbility(player);

                        // Check healing touch
                        if (HealingTouch.IsActive)
                        {
                            MonsterDamageTaken -= enemy.TemporaryLife;
                            player.TemporaryLife += MonsterDamageTaken;
                        }

                        // Foe move
                        if (enemy.TemporaryLife > 0)
                        {
                            // Check prevention
                            if (enemyCanMove)
                            {
                                player.TemporaryLife -= enemy.UseAbility();
                            }

                            // --------------------------------------------------COMBAT_AFTER--------------------------------------------------
                            // SET VARIABLES PHASE B

                            // Life Regen
                            if (player.TemporaryLife > 0) // this if() prevents the life-regen-revive-and-win bug
                            {
                                // If statement for the 3rd ability: Regen
                                int TimeBasedLifeGain = 0;
                                Heal h = (Heal)player.Abilities[2];
                                if (h.TotalRegen.Count > 0)
                                {

                                    // Removing any time based instances of regen if their time has expired
                                    for (int i = 0; i < h.AllRegenTurns.Count; i++)
                                    {
                                        if (h.AllRegenTurns[i] <= 0)
                                        {
                                            h.TotalRegen.RemoveAt(i);
                                            h.AllRegenTurns.RemoveAt(i);
                                        }
                                    }
                                    for (int i = 0; i < h.AllRegenTurns.Count; i++)
                                    {
                                        h.AllRegenTurns[i]--;
                                    }

                                    // Adding all time based instances of regen to lifeGain
                                    foreach (int reg in h.TotalRegen)
                                    {
                                        TimeBasedLifeGain += reg;
                                    }
                                }

                                // Applying life regen from all sources to the player, then displaying a message
                                player.TemporaryLife += (player.TemporaryLifeRegen + TimeBasedLifeGain);
                                if (player.TemporaryLifeRegen + TimeBasedLifeGain > 0) WriteLine("You regen {1} life.{0}", "\n", (player.TemporaryLifeRegen + TimeBasedLifeGain));
                                CombatTurn++;
                            }

                            // Miracle
                            if (r.Next(1, 101) <= player.TemporaryMiracle)
                            {
                                if (r.Next(1, 101) <= player.TemporaryMiracleSuper)
                                {
                                    int lifeGain = (BasePlayerLife * 2);

                                    player.TemporaryLife += lifeGain;
                                    WriteLine("By a super miracle, you gain {1} life.{0}", "\n", lifeGain);
                                }
                                else
                                {
                                    int lifeGain = (BasePlayerLife / 2);
                                    player.TemporaryLife += lifeGain;
                                    WriteLine("By a miracle, you gain {1} life.{0}", "\n", lifeGain);
                                }
                            }

                            // Prevention
                            enemyCanMove = true;
                            if (r.Next(1, 101) <= player.TemporaryPrevention)
                            {
                                enemyCanMove = false;
                                WriteLine("{1} is prevented. It will do nothing this turn.{0}", "\n", enemy.Name);
                            }

                            // Dodge
                            enemy.WillMiss = false;
                            player.WillDodge = false;
                            if (r.Next(1, 101) <= player.TemporaryDodge)
                            {
                                enemy.WillMiss = true;
                                player.WillDodge = true;
                                // No message is displayed, as the user will view this decision as being made in real time...
                                // ... at which point it may display a message.
                            }

                            // Removing bonus critical damage
                            if (currentlyCritical)
                            {
                                player.TemporaryDamage -= bonusDamageFromCritical;
                                currentlyCritical = false;
                            }

                            // Removing bonus rant force damage
                            if (TriBuff.UsedLast) player.TemporaryDamage -= TriBuff.Buff;

                            // Rant Force ability
                            int tBuff = 0;
                            TriBuff.Buff = tBuff;
                            if (TriBuff.IsActive)
                            {
                                tBuff = (int)(player.TemporaryDamage * BuffTwo.DEFAULT_BUFFAMOUNT);
                                player.TemporaryDamage += tBuff;
                                TriBuff.Buff = tBuff;
                                //TriBuff.UsedLast = true;
                            }
                            else
                            {
                                TriBuff.UsedLast = false;
                            }
                            // End of the exchange (all of the reactions to the player pressing a button have finished)
                            // SET VARIABLES PHASE C
                            //if (player.TemporaryLife > BasePlayerLife) player.TemporaryLife = BasePlayerLife;

                            // Healing Touch ability
                            if (HealingTouch.IsActive)
                            {
                                if (HealingTouch.HealingTouchTurns == HealTwo.DEFAULT_TURNS) player.TemporaryDamage /= 2;
                                HealingTouch.HealingTouchTurns--;
                                if (HealingTouch.HealingTouchTurns < 0)
                                {
                                    HealingTouch.IsActive = false;
                                    WriteLine("Your healing touch wears off.\n");
                                }
                            }

                            // Critical
                            bonusDamageFromCritical = 0;
                            if (r.Next(1, 101) <= player.TemporaryCritical)
                            {
                                if (r.Next(1, 101) <= player.TemporaryCriticalSuper)
                                {
                                    bonusDamageFromCritical = (player.TemporaryDamage * 4);
                                    //player.TemporaryDamage += bonusDamageFromCritical;
                                    //WriteLine("Super critical: you have {1} extra damage for this turn.{0}", "\n", bonusDamageFromCritical);
                                    WriteLine("Super critical: you have 5x damage for 1 turn!");
                                    currentlyCritical = true;
                                }
                                else
                                {
                                    bonusDamageFromCritical = player.TemporaryDamage;
                                    //player.TemporaryDamage += bonusDamageFromCritical;
                                    //WriteLine("Critical: you have {1} extra damage for this turn.{0}", "\n", bonusDamageFromCritical);
                                    WriteLine("Critical: you have 2x damage for 1 turn!");
                                    currentlyCritical = true;
                                }
                            }

                            // Adding any bonus damage to the player
                            //player.TemporaryDamage += tBuff;
                            player.TemporaryDamage += bonusDamageFromCritical;
                        }
                    }
                }
            }

            // When either the player or foe is defeated
            if (player.TemporaryLife <= 0)
            {
                // Death message and setting experience to 0.
                player.WriteNewExperience(0); // Call WriteNewExperience when xp is decreasing
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Your life has reached {1} <= 0.{0}" +
                    "You have been defeated by {2} and have been returned to the main menu.{0}" +
                    "Your experience for this level has been reset to 0.{0}", "\n", player.TemporaryLife, enemy.Name);
                ForegroundColor = ConsoleColor.White;
                MainMenu();
            }
            else
            {
                // Experience reward
                player.IncreaseXP(enemy.RewardXP); // Call IncreaseXP when xp is increasing
                ForegroundColor = ConsoleColor.Green;
                string message;
                if (enemy.RewardXP == 1)
                {
                    message = String.Format("Congratulations! You have defeated {1}!{0}" +
                    "It has granted you {2} experience point!{0}", "\n", enemy.Name, enemy.RewardXP);
                }
                else message = String.Format("Congratulations! You have defeated {1}!{0}" +
                    "It has granted you {2} experience points!{0}", "\n", enemy.Name, enemy.RewardXP);
                WriteLine(message);
                ForegroundColor = ConsoleColor.White;

                // Potential power reward
                // r is a 'Random' object created at the top of the dynamic combat method
                float odds = (enemy.ItemChance * 100);
                if (odds >= r.Next(1, 101)) RewardTables.PickAndApplyReward(enemy.LootTable, player); // This is a number from 1-100.

                // Checking if this is the furthest monster defeated in progression. If so, it unlocks the next monster or zone!
                if (enemy.ID == player.ExploreProgress && player.ExploreProgress < Monster.totalMonsters) // RHS prevents out-of-bounds arrays for the end of the game
                {
                    player.WriteNewExploreProgress(player.ExploreProgress + 1);
                    ForegroundColor = ConsoleColor.Green;
                    if (enemy.IsBoss) WriteLine("New area unlocked: {0}\n", Monster.GetZoneName(enemy.AreaNumber + 1));
                    else WriteLine("A new foe has been discovered.\n");
                    ForegroundColor = ConsoleColor.White;
                }

                // Return to exploring
                ExploreMenu();
            }
        }


        public static void PlayerInfoMenu()
        {
            player.SetPlayerValues();
            do
            {
                WriteLine("1) Rename {0}", player.Name);
                WriteLine("0) Main Menu");
                Write("{0}{0}{0}", "\n"); // Indent downwards
                WriteLine("Name: " + player.Name);
                WriteLine("Level: " + player.Level);
                WriteLine("Experiece: " + player.Experience + "/" + Player.RequiredExperience(player.Level));
                WriteLine("Money: " + player.Money);
                Write("{0}---------- Basic Player Stats ----------{0}{0}", "\n");
                WriteLine("Life: " + player.TemporaryLife);
                WriteLine("Damage: " + player.TemporaryDamage);
                WriteLine("Life regen: " + player.TemporaryLifeRegen);
                Write("{0}---------- Advanced Player Stats ----------{0}{0}", "\n");
                WriteLine("Resistance(-): {1}{0}{0}{0}Critical: {2:P}", "\t", player.TemporaryResistanceFlat, ((float)player.TemporaryCritical / 100));
                WriteLine("Resistance(%): {1:P}{0}{0}{0}Super Critical: {2:P}", "\t", ((float)player.TemporaryResistancePercent / 100), ((float)player.TemporaryCriticalSuper / 100));
                WriteLine("Prevention: {1:P}{0}{0}{0}Miracle: {2:P}", "\t", ((float)player.TemporaryPrevention / 100), ((float)player.TemporaryMiracle / 100));
                WriteLine("Dodge: {1:P}{0}{0}{0}{0}Super Miracle: {2:P}", "\t", ((float)player.TemporaryDodge / 100), ((float)player.TemporaryMiracleSuper / 100));

                input = (char)ReadKey().KeyChar;
                Clear();
            } while (input != '0' && input != '1');

            // Using the user input to choose a menu
            if (input == '0') MainMenu();
            if (input == '1')
            {
                Clear();
                Write("Enter new name: ");
                string newName = ReadLine();
                player.Rename(newName);
                Clear();
                PlayerInfoMenu();
            }
        }


        public static void InventoryMenu()
        {
            do
            {
                Clear();
                for (int i = 0; i < player.InventoryItems.Length; i++)
                {
                    WriteLine("---------------Inventory Slot {0}---------------\n{1}", (i + 1), player.InventoryItems[i].ToStringWithSellPrice());
                }
                Write("{0}{0}", "\n"); // Indent downwards
                for (int i = 0; i < player.EquippedItems.Length; i++)
                {
                    WriteLine("---------------Item Slot {0}---------------\n{1}", (i + 1), player.EquippedItems[i].ToStringWithSellPrice());
                }
                Write("\n");
                WriteLine("1) Swap Item Slot And Inventory Slot");
                WriteLine("2) Sell Inventory Item");
                WriteLine("3) Sell All Inventory Items");
                WriteLine("0) Main Menu");
                Write("\n\n");

                input = (char)ReadKey().KeyChar;
            } while (input != '0' && input != '1' && input != '2' && input != '3');

            // Using the user input to choose a menu
            if (input == '0')
            {
                Clear();
                MainMenu();
            }
            if (input == '1')
            {
                // Accepting user input
                string firstInput, secondInput;
                Write("\bItem slot: ");
                firstInput = ReadLine();
                Write("Inventory slot: ");
                secondInput = ReadLine();

                // Validating input
                if (HasOnlyDigits(firstInput) == false || firstInput.Length == 0) InventoryMenu();
                if (HasOnlyDigits(secondInput) == false || secondInput.Length == 0) InventoryMenu();
                int firstSlot = int.Parse(firstInput);
                int secondSlot = int.Parse(secondInput);
                if (firstSlot > player.EquippedItems.Length) InventoryMenu();
                if (secondSlot > player.InventoryItems.Length) InventoryMenu();

                // Swapping the items
                Item temp;
                temp = player.EquippedItems[firstSlot - 1];
                player.EquippedItems[firstSlot - 1] = player.InventoryItems[secondSlot - 1];
                player.InventoryItems[secondSlot - 1] = temp;

                player.WriteItemData();
                InventoryMenu();
            }
            if (input == '2')
            {
                Write("\bInventory slot to sell: ");
                string inputSlot = ReadLine();

                // Validating input
                if (HasOnlyDigits(inputSlot) == false || inputSlot.Length == 0) InventoryMenu();
                int inputSlotNum = int.Parse(inputSlot);
                if (inputSlotNum > player.InventoryItems.Length) InventoryMenu();

                // Selling the item
                player.Money += player.InventoryItems[inputSlotNum - 1].Money / 5;
                Item emptyItem = new ItemZero();
                player.InventoryItems[inputSlotNum - 1] = emptyItem;

                player.WriteNumeralData(); // Saving
                player.WriteItemData(); // Saving
                InventoryMenu();
            }
            if (input == '3')
            {
                // Selling all items

                Item emptyItem = new ItemZero();
                for (int i = 0; i < player.InventoryItems.Length; i++)
                {
                    player.Money += player.InventoryItems[i].Money / 5;
                    player.InventoryItems[i] = emptyItem;
                }
                player.WriteNumeralData(); // Saving
                player.WriteItemData(); // Saving
                InventoryMenu();
            }
        }


        public static void ShopMenu() // Should make a more advanced Shop.cs class with much more features
        {
            while (true)
            {
                do
                {

                    // Shop menu

                    Store.LoadWares();
                    for (int i = 0; i < Store.Wares.Length; i++)
                    {
                        WriteLine("{0}) Press {0} to buy {1} for {2} money.\n{3}\n", i + 1, Store.Wares[i].Name, Store.Wares[i].Money, Store.Wares[i].ToString());
                        //WriteLine(Store.Wares[i].ToString() + "\n");
                    }
                    WriteLine("{0}) Spend 25 money to refresh the store's items.\n", Store.Wares.Length + 1);
                    WriteLine("{0}) Spend 100 money to gain +1 permanent life.\n", Store.Wares.Length + 2);
                    WriteLine("{0}) Spend 100 money to gain +1 permanent damage.\n", Store.Wares.Length + 3);
                    WriteLine("0) Main Menu\n");
                    WriteLine("Your money: {0}", player.Money);

                    // Accept input from user
                    input = (char)ReadKey().KeyChar;
                    Clear();

                    if (char.IsDigit(input) == false) inputInt = -1;
                    else inputInt = int.Parse(input.ToString());
                } while (inputInt < 0 || inputInt > 6); // 6 items in the shop
                if (inputInt == 0) MainMenu();
                if (inputInt == 1 && player.Money >= Store.Wares[0].Money) Store.BuyItem(1, player);
                if (inputInt == 2 && player.Money >= Store.Wares[1].Money) Store.BuyItem(2, player);
                if (inputInt == 3 && player.Money >= Store.Wares[2].Money) Store.BuyItem(3, player);
                if (inputInt == 4 && player.Money >= 25)
                {
                    player.DecreaseMoney(25);
                    Store.RefreshWares();
                }
                if (inputInt == 5 && player.Money >= 100)
                {
                    player.DecreaseMoney(100);
                    player.AwardLife(1);
                }
                if (inputInt == 6 && player.Money >= 100)
                {
                    player.DecreaseMoney(100);
                    player.AwardDamage(1);
                }
            }
            // end of permanent loop
        }


        // Reads info from .txt files
        public static void LoadPlayerData()
        {
            // This code is declared in order with respect to the NumeralData.txt file it's collected from.
            string tempName;

            int tempLevel; // NumeralData.txt line 1
            int tempExperience; // NumeralData.txt line 2
            int tempExploreProgress; // NumeralData.txt line 3
            int tempMoney; // NumeralData.txt line 4
            int tempLife; // NumeralData.txt line 5
            int tempLifeBuffFlat; // NumeralData.txt line 6
            int tempLifeBuffPercent; // NumeralData.txt line 7
            int tempDamage; // NumeralData.txt line 8
            int tempDamageBuffFlat; // NumeralData.txt line 9
            int tempDamageBuffPercent; // NumeralData.txt line 10
            int tempLifeRegen; // NumeralData.txt line 11
            int tempMiracle; // NumeralData.txt line 12
            int tempMiracleSuper; // NumeralData.txt line 13
            int tempCritical; // NumeralData.txt line 14
            int tempCriticalSuper; // NumeralData.txt line 15
            int tempResistanceFlat; // NumeralData.txt line 16
            int tempResistancePercent; // NumeralData.txt line 17
            int tempPrevention; // NumeralData.txt line 18
            int tempDodge; // NumeralData.txt line 19

            // Reading string data
            StreamReader nameReader = new StreamReader("PlayerName.txt");
            using (nameReader)
            {
                tempName = nameReader.ReadLine(); // PlayerName.txt line 1
            }
            nameReader.Close();

            // Reading numeral data
            StreamReader numReader = new StreamReader("NumeralData.txt");
            using (numReader)
            {
                tempLevel = int.Parse(numReader.ReadLine()); // NumeralData.txt line 1
                tempExperience = int.Parse(numReader.ReadLine()); // NumeralData.txt line 2
                tempExploreProgress = int.Parse(numReader.ReadLine()); // NumeralData.txt line 3
                tempMoney = int.Parse(numReader.ReadLine()); // NumeralData.txt line 4
                tempLife = int.Parse(numReader.ReadLine()); // NumeralData.txt line 5
                tempLifeBuffFlat = int.Parse(numReader.ReadLine()); // NumeralData.txt line 6
                tempLifeBuffPercent = int.Parse(numReader.ReadLine()); // NumeralData.txt line 7
                tempDamage = int.Parse(numReader.ReadLine()); // NumeralData.txt line 8
                tempDamageBuffFlat = int.Parse(numReader.ReadLine()); // NumeralData.txt line 9
                tempDamageBuffPercent = int.Parse(numReader.ReadLine()); // NumeralData.txt line 10
                tempLifeRegen = int.Parse(numReader.ReadLine()); // NumeralData.txt line 11
                tempMiracle = int.Parse(numReader.ReadLine()); // NumeralData.txt line 12
                tempMiracleSuper = int.Parse(numReader.ReadLine()); // NumeralData.txt line 13
                tempCritical = int.Parse(numReader.ReadLine()); // NumeralData.txt line 14
                tempCriticalSuper = int.Parse(numReader.ReadLine()); // NumeralData.txt line 15
                tempResistanceFlat = int.Parse(numReader.ReadLine()); // NumeralData.txt line 16
                tempResistancePercent = int.Parse(numReader.ReadLine()); // NumeralData.txt line 17
                tempPrevention = int.Parse(numReader.ReadLine()); // NumeralData.txt line 18
                tempDodge = int.Parse(numReader.ReadLine()); // NumeralData.txt line 19
            }
            numReader.Close();

            // Loading the read data info a Player object
            player = new Player(tempName, tempLevel, tempExperience, tempExploreProgress, tempMoney, tempLife, tempLifeBuffFlat, tempLifeBuffPercent, tempDamage, tempDamageBuffFlat,
                tempDamageBuffPercent, tempLifeRegen, tempMiracle, tempMiracleSuper, tempCritical, tempCriticalSuper, tempResistanceFlat, tempResistancePercent, tempPrevention, tempDodge);
        }

        // Used by children of the Monster class during combat
        public static int APM(int enemyDamage) // APM == ApplyPlayerMitigation
        {
            // Formula applies percentage resistance first, then flat resistance.
            //Ex: enemyDamage = 100, resistancePercent = 75, resistanceFlat = 20. Result of damage = 5. Reverse the order and it would be 23.
            float mitigatedEnemyDamage = ((float)enemyDamage * (1 - ((float)player.TemporaryResistancePercent / 100)) - (float)player.TemporaryResistanceFlat);
            int damageResult = (int)mitigatedEnemyDamage;
            if (damageResult < 0) damageResult = 0;
            return damageResult;
        }

        public static bool HasOnlyDigits(string str)
        {
            char[] charArray = str.ToCharArray();
            foreach (char c in charArray)
            {
                if (char.IsDigit(c) == false) return false;
            }
            return true;
        }
    }
}
