using System;
using System.Collections.Generic;
using System.IO;

// permanent buffs should be rare and reserved to difficult encounters
// every combat attribute needs a temporary variable so it may be easily manipulated by abilities. +t means it needs temporary var as well
/*
 * damage+t - base player damage. Only needs to be seperated if implementing buff % damage AND buff flat damage
 * life+t - base player life. Only needs to be seperated if implementing buff % life AND buff flat life
 * lifeRegen+t
 * resistanceFlat+t
 * resistancePercent+t
 * dodge+t
 * prevention+t
 * critical+t // dmg * 2
 * superCritical+t // convert crit to dmg * 5
 * miracle+t // 50% life heal
 * superMiracle+t // convert miracle to 200% life heal
 * 
 * damageBuffFlat - no temp needed as this will simply calculate damage (base player damage)
 * damageBuffPercentage - no temp needed as this will simply calculate damage (base player damage)
 * lifeBuffFlat - no temp needed as this will simply calculate life (base player life)
 * lifeBuffPercentage - no temp needed as this will simply calculate life (base player life)
 * 
 * file order: 
 * 1: level
 * 2: experience
 * 3: exploreProgress
 * 
 * 4: gold
 * 5: life
 * 6: lifeBuffFlat
 * 7: lifeBuffPercentage
 * 8: damage
 * 9: damageBuffFlat
 * 10: damageBuffPercentage
 * 11: lifeRegen
 * 12: miracle
 * 13: miracleSuper
 * 14: critical
 * 15: criticalSuper
 * 16: resistanceFlat
 * 17: resistancePercentage
 * 18: prevention
 * 19: dodge
 * 
 */

public class Player
{
	private string name;

	private int level;
	private int experience;
	private int exploreProgress;
	private int money;
	//private List<Item> activeItems = new List<Item>(); // Intended capacity: 5. (equipped items)
	//private List<Item> inventory = new List<Item>(); // Intended capacity: 20. (stored items not currently in use)
	private Item[] equippedItems = new Item[5]; // Player can equip 5 items
	private Item[] inventoryItems = new Item[20]; // Inventory size is 20
	private Ability[] abilities;

	private int life;
	private int lifeBuffFlat; // Not useful?
	private int lifeBuffPercent; // 1 = 1%
	private int damage;
	private int damageBuffFlat; // Not useful?
	private int damageBuffPercent; // 1 = 1%
	private int lifeRegen;
	private int miracle;
	private int miracleSuper; // 1 = 1%
	private int critical;
	private int criticalSuper;
	private int resistanceFlat;
	private int resistancePercent; // 1 = 1%
	private int prevention; // 1 = 1%
	private int dodge; // 1 = 1%

	// Used to maintain permanent status of object's values. These have setters. Typically, abilities will alter these for the remainder of combat.
	private int temporaryLife; // Used by some abilities
	private int temporaryLifeBuffFlat; // Not useful?
	private int temporaryLifeBuffPercent;
	private int temporaryDamage; // Used by some abilities to set CurrentPlayerDamage = temporaryDamage;
	private int temporaryDamageBuffFlat; // Not useful?
	private int temporaryDamageBuffPercent;
	private int temporaryLifeRegen;
	private int temporaryMiracle;
	private int temporaryMiracleSuper;
	private int temporaryCritical;
	private int temporaryCriticalSuper;
	private int temporaryResistanceFlat;
	private int temporaryResistancePercent;
	private int temporaryPrevention;
	private int temporaryDodge;

	private bool willDodge; // Accessed and manipulated during combat

	// Accessor properties for core player variables. Cannot be changed without a file save...
	// ... These are used to calculate the real-time temporary variables...
	// ... The temporary variables are the ones displayed and manipulated in combat.
	public string Name { get { return this.name; } }
	public int Level { get { return this.level; } }
	public int Experience { get { return this.experience; } }
	public int ExploreProgress { get { return this.exploreProgress; } }
	public int Life { get { return this.life; } }
	public int LifeBuffPercent { get { return this.lifeBuffPercent; } }
	public int Damage { get { return this.damage; } }
	public int DamageBuffPercent { get { return this.damageBuffPercent; } }
	public int LifeRegen { get { return this.lifeRegen; } }
	public int Miracle { get { return this.miracle; } }
	public int MiracleSuper { get { return this.miracleSuper; } }
	public int Critical { get { return this.critical; } }
	public int CriticalSuper { get { return this.criticalSuper; } }
	public int ResistanceFlat { get { return this.resistanceFlat; } }
	public int ResistancePercent { get { return this.resistancePercent; } }
	public int Prevention { get { return this.prevention; } }
	public int Dodge { get { return this.dodge; } }
	public Item[] EquippedItems { get { return this.equippedItems; } }
	public Item[] InventoryItems { get { return this.inventoryItems; } }
	public Ability[] Abilities { get { return this.abilities; } set { this.abilities = value; } }

	// With setters, the temporary values can be changed. The core player data above cannot be changed without a file save.
	public int Money { get { return this.money; } set { this.money = value; } }
	public int TemporaryLife { get { return this.temporaryLife; } set { this.temporaryLife = value; } }
	public int TemporaryLifeBuffPercent { get { return this.temporaryLifeBuffPercent; } set { this.temporaryLifeBuffPercent = value; } }
	public int TemporaryDamage { get { return this.temporaryDamage; } set { this.temporaryDamage = value; } }
	public int TemporaryDamageBuffPercent { get { return this.temporaryDamageBuffPercent; } set { this.temporaryDamageBuffPercent = value; } }
	public int TemporaryLifeRegen { get { return this.temporaryLifeRegen; } set { this.temporaryLifeRegen = value; } }
	public int TemporaryMiracle { get { return this.temporaryMiracle; } set { this.temporaryMiracle = value; } }
	public int TemporaryMiracleSuper { get { return this.temporaryMiracleSuper; } set { this.temporaryMiracleSuper = value; } }
	public int TemporaryCritical { get { return this.temporaryCritical; } set { this.temporaryCritical = value; } }
	public int TemporaryCriticalSuper { get { return this.temporaryCriticalSuper; } set { this.temporaryCriticalSuper = value; } }
	public int TemporaryResistanceFlat { get { return this.temporaryResistanceFlat; } set { this.temporaryResistanceFlat = value; } }
	public int TemporaryResistancePercent { get { return this.temporaryResistancePercent; } set { this.temporaryResistancePercent = value; } }
	public int TemporaryPrevention { get { return this.temporaryPrevention; } set { this.temporaryPrevention = value; } }
	public int TemporaryDodge { get { return this.temporaryDodge; } set { this.temporaryDodge = value; } }
	public bool WillDodge { get { return this.willDodge; } set { this.willDodge = value; } }

	// Constants
	public const string DEFAULT_NAME = "Player";
	public const int DEFAULT_LVL = 1;
	public const int DEFAULT_XP = 0;
	public const int DEFAULT_EXPLORE_PROGRESS = 1;
	public const int DEFAULT_LIFE = 30;
	public const int DEFAULT_DAMAGE = 5;
	public const int DEFAULT_LIFEREGEN = 0;
	// Essentially every other variable would be a constant of 0.

	// Constructors
	public Player() { }
	public Player(string name, int level, int experience, int exploreProgress, int money, int life, int lifeBuffFlat, int lifeBuffPercent, int damage, int damageBuffFlat, int damageBuffPercent,
		int lifeRegen, int miracle, int miracleSuper, int critical, int criticalSuper, int resistanceFlat, int resistancePercent, int prevention, int dodge)
    {
		this.name = name;
		this.level = level;
		this.experience = experience;
		this.exploreProgress = exploreProgress;
		this.money = money;
		this.life = life;
		this.lifeBuffFlat = lifeBuffFlat;
		this.lifeBuffPercent = lifeBuffPercent;
		this.damage = damage;
		this.damageBuffFlat = damageBuffFlat;
		this.damageBuffPercent = damageBuffPercent;
		this.lifeRegen = lifeRegen;
		this.miracle = miracle;
		this.miracleSuper = miracleSuper;
		this.critical = critical;
		this.criticalSuper = criticalSuper;
		this.resistanceFlat = resistanceFlat;
		this.resistancePercent = resistancePercent;
		this.prevention = prevention;
		this.dodge = dodge;
    }

	public void IncreaseXP(int xp) // Level up should be here to account for remainder overflow
	{
		this.experience += xp;
		if (this.experience >= RequiredExperience(this.level))
        {
			this.experience -= RequiredExperience(this.level); // Consumes the required experience to level up
			this.level++; // Performs the level up
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("{1} is now level {2}!{0}", "\n", this.name, this.level);
			Console.ForegroundColor = ConsoleColor.White;
		}
		WriteNumeralData(); // Saves new data to NumeralData.txt
	}

	// Methods
	// Returns the required amount of experience required to level up for a given level
	public static int RequiredExperience(int lvl) // where 0 < lvl < 20, lvl an integer
    {
		if (lvl > 0 && lvl < 20) return ((lvl * lvl * 10) + 20); // ((lvl ^ 2) * 10).LHS = 1, 4, 9, ..., 361. =) 10, 40, 90, ..., 3,610. NEW: 30, 60, 110, ..., 3,630.
		// Alternate XP curve: ((2 ^ lvl) * 5). LHS = 2, 4, 8, ..., 524,288. =) 10, 20, 40, ..., 2,621,440.
		else return 1_000_000;
    }


	// Item methods retaining to the player object and variables
	public void AwardItem(Item newItem)
    {
		bool isRoom = false;
		int itemPosition = 0;
        for (int i = 0; i < inventoryItems.Length; i++)
        {
			if (inventoryItems[i].ID == 0)
            {
				isRoom = true;
				itemPosition = i;
				break;
			}
        }
		if (isRoom) // Adding item
        {
			inventoryItems[itemPosition] = newItem;
			WriteItemData();
			if (newItem.Tier >= 3) Console.ForegroundColor = ConsoleColor.Blue;
			else Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("A new item {0} has been added to your inventory in slot {1}.\n\n{2}\n", newItem.Name, itemPosition + 1, newItem.ToStringWithSellPrice());
			Console.ForegroundColor = ConsoleColor.White;
		}
		else // Full, so just giving you the money
        {
			this.Money += newItem.Money;
			WriteNumeralData(); // Saving the money
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("A new item {0} couldn't fit in your inventory.\nIt was sold automatically for {1} money.\n", newItem.Name, newItem.Money);
			Console.ForegroundColor = ConsoleColor.White;
		}
    }

	public void SetPlayerValues() // Setting up the player
    {
		// Copying base stats to temporary variables (here, item bonuses aren't included)
		TemporaryLife = Life;
		temporaryLifeBuffPercent = LifeBuffPercent;
		TemporaryDamage = Damage;
		temporaryDamageBuffPercent = DamageBuffPercent;
		TemporaryLifeRegen = LifeRegen; // Life regen is used in combat loop
		TemporaryMiracle = Miracle; // Miracle and super miracle stats are used in combat loop
		TemporaryMiracleSuper = MiracleSuper;
		TemporaryCritical = Critical; // Critical and super critical stats are used in combat loop
		TemporaryCriticalSuper = CriticalSuper;
		TemporaryResistanceFlat = ResistanceFlat; // The resistance stats are used automatically by the ApplyDamageMitigation method
		TemporaryResistancePercent = ResistancePercent; // The resistance stats are used automatically by the ApplyDamageMitigation method
		TemporaryPrevention = Prevention; // Prevention stat used in combat loop
		TemporaryDodge = Dodge; // Dodge stat used in combat loop

		// Increasing stats based on equipped items.
		for (int i = 0; i < EquippedItems.Length; i++)
		{
			TemporaryLife += EquippedItems[i].FlatLife;
			TemporaryDamage += EquippedItems[i].FlatDamage;
			TemporaryLifeRegen += EquippedItems[i].LifeRegen;
			TemporaryMiracle += EquippedItems[i].Miracle;
			TemporaryMiracleSuper += EquippedItems[i].SuperMiracle;
			TemporaryCritical += EquippedItems[i].Critical;
			TemporaryCriticalSuper += EquippedItems[i].SuperCritical;
			TemporaryResistanceFlat += EquippedItems[i].FlatResistance;
			TemporaryResistancePercent += EquippedItems[i].PercentResistance;
			TemporaryPrevention += EquippedItems[i].Prevention;
			TemporaryDodge += EquippedItems[i].Dodge;

			TemporaryLifeBuffPercent += EquippedItems[i].PercentLife;
			TemporaryDamageBuffPercent += EquippedItems[i].PercentDamage;
		}

		// Lastly, applying % increase modifiers (Because of 2 step computations: flat life * life%)
		TemporaryLife += (int)((float)TemporaryLife * (float)TemporaryLifeBuffPercent / 100);
		TemporaryDamage += (int)((float)TemporaryDamage * (float)TemporaryDamageBuffPercent / 100);
	}

	public void LoadPlayerItems() // Fills the player's equippedItems and inventoryItems arrays.
    {
		Item[] allItems = Item.GetAllItemsArray();
		StreamReader reader = new StreamReader("ItemData.txt");
		using (reader)
        {
            for (int i = 0; i < equippedItems.Length; i++)
            {
				equippedItems[i] = allItems[int.Parse(reader.ReadLine())]; // allItems[i] == item.ID;
            }
            for (int i = 0; i < inventoryItems.Length; i++)
            {
				inventoryItems[i] = allItems[int.Parse(reader.ReadLine())];

			}
        }
		reader.Close();
    }

	public void WriteItemData() // Stores all current elements in equippedItems[] and inventoryItems[] to the file
    {
		StreamWriter writer = new StreamWriter("ItemData.txt");
		using (writer)
        {
			for (int i = 0; i < equippedItems.Length; i++) // equippedItems[] occupies lines 1-5
			{
				writer.WriteLine(equippedItems[i].ID);
			}
			for (int i = 0; i < inventoryItems.Length; i++) // inventoryItems[] occupies lines 6-25
			{
				writer.WriteLine(inventoryItems[i].ID);

			}
		}
		writer.Close();
    }

	// File writing methods
	public void Rename(string newName)
	{
		this.name = newName;
		StreamWriter writer = new StreamWriter("PlayerName.txt");
		using (writer)
		{
			writer.WriteLine(newName);
		}
		writer.Close();
	}

	public void AwardMoney(int n)
	{
		Console.ForegroundColor = ConsoleColor.DarkYellow;
		Console.WriteLine("Congratulations! You have gained {0} money.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.money += n;
		WriteNumeralData();
	}

	public void DecreaseMoney(int n)
	{
		this.money -= n;
		WriteNumeralData();
	}

	public void WriteNewLife(int newLife)
	{
		this.life = newLife; // Saving to object, then to file
		WriteNumeralData();
	}

	public void AwardLife(int n)
    {
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0} life bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.life += n;
		WriteNumeralData();
    }

	public void WriteNewDamage(int newDamage)
	{
		this.damage = newDamage; // Saving to object, then to file
		WriteNumeralData();
	}

	public void AwardDamage(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0} damage bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.damage += n;
		WriteNumeralData();
	}

	public void WriteNewLifeRegen(int newLifeRegen)
	{
		this.lifeRegen = newLifeRegen;
		WriteNumeralData();
	}

	public void AwardLifeRegen(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0} life regen bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.lifeRegen += n;
		WriteNumeralData();
	}

	public void WriteNewCritical(int newCritical)
	{
		this.critical = newCritical;
		WriteNumeralData();
	}

	public void AwardCritical(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0}% critical bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.critical += n;
		WriteNumeralData();
	}

	public void WriteNewSuperCritical(int newSuperCritical)
	{
		this.criticalSuper = newSuperCritical;
		WriteNumeralData();
	}

	public void AwardSuperCritical(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0}% super critical bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.criticalSuper += n;
		WriteNumeralData();
	}

	public void WriteNewMiracle(int newMiracle)
	{
		this.miracle = newMiracle;
		WriteNumeralData();
	}

	public void AwardMiracle(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0}% miracle bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.miracle += n;
		WriteNumeralData();
	}

	public void WriteNewSuperMiracle(int newSuperMiracle)
	{
		this.miracleSuper = newSuperMiracle;
		WriteNumeralData();
	}

	public void AwardSuperMiracle(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0}% super miracle bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.miracleSuper += n;
		WriteNumeralData();
	}

	public void WriteNewFlatResistance(int newResistance)
	{
		this.resistanceFlat = newResistance;
		WriteNumeralData();
	}

	public void AwardFlatResistance(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0} resistance bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.resistanceFlat += n;
		WriteNumeralData();
	}

	public void WriteNewPercentResistance(int newPercentResistance)
	{
		this.resistancePercent = newPercentResistance;
		WriteNumeralData();
	}

	public void AwardPercentResistance(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0}% resistance bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.resistancePercent += n;
		WriteNumeralData();
	}

	public void WriteNewDodge(int newDodge)
	{
		this.dodge = newDodge;
		WriteNumeralData();
	}

	public void AwardDodge(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0}% dodge bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.dodge += n;
		WriteNumeralData();
	}

	public void WriteNewPrevention(int newPrevention)
	{
		this.prevention = newPrevention;
		WriteNumeralData();
	}

	public void AwardPrevention(int n)
	{
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("Congratulations! You have received a permanent +{0}% prevention bonus.\n", n);
		Console.ForegroundColor = ConsoleColor.White;
		this.prevention += n;
		WriteNumeralData();
	}

	public void WriteNewExperience(int newExperience)
	{
		this.experience = newExperience; // Saving to object, then to file
		WriteNumeralData();
	}

	public void WriteNewExploreProgress(int newExploreProgress)
	{
		this.exploreProgress = newExploreProgress; // Saving to object, then to file
		WriteNumeralData();
	}

	public void WriteNumeralData()
    {
		int[] numeralData = new int[19];
		numeralData[0] = this.level;
		numeralData[1] = this.experience;
		numeralData[2] = this.exploreProgress;
		numeralData[3] = this.money;
		numeralData[4] = this.life;
		numeralData[5] = this.lifeBuffFlat;
		numeralData[6] = this.lifeBuffPercent;
		numeralData[7] = this.damage;
		numeralData[8] = this.damageBuffFlat;
		numeralData[9] = this.damageBuffPercent;
		numeralData[10] = this.lifeRegen;
		numeralData[11] = this.miracle;
		numeralData[12] = this.miracleSuper;
		numeralData[13] = this.critical;
		numeralData[14] = this.criticalSuper;
		numeralData[15] = this.resistanceFlat;
		numeralData[16] = this.resistancePercent;
		numeralData[17] = this.prevention;
		numeralData[18] = this.dodge;

		StreamWriter numWriter = new StreamWriter("NumeralData.txt");
		using (numWriter)
		{
			for (int i = 0; i < numeralData.Length; i++)
			{
				numWriter.WriteLine(numeralData[i]);
			}
		}
		numWriter.Close();
	}
}
