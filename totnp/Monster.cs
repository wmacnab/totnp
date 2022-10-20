using Microsoft.VisualBasic;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using static totnb.totnb;

public class Monster
{
	protected string name;
	protected int id;
	protected int areaNumber;
	protected int life;
	protected int damage;
	protected int rewardXP;
	protected int lootTable;
	protected float itemChance; // Effective for 2 decimal places
	protected bool isBoss;
	protected string display;

	protected int temporaryLife;
	protected int temporaryDamage;
	protected bool willMiss;

	// Not including vault
	public static int totalMonsters = 9; // The number of monsters currently created and ready to appear in the game

	// Constructors
	public Monster() { }
	public Monster(string name, int id, int areaNumber, int life, int damage, int rewardXP, int lootTable, float itemChance, bool isBoss)
    {
		this.name = name;
		this.id = id;
		this.areaNumber = areaNumber;
		this.life = life;
		this.damage = damage;
		this.rewardXP = rewardXP;
		this.lootTable = lootTable;
		this.itemChance = itemChance;
		this.isBoss = isBoss;
		this.display = String.Format("Fight {0}", this.name);
    }


	// Accessor properties
	public string Name { get { return this.name; } }
	public int ID { get { return this.id; } }
	public int AreaNumber { get { return this.areaNumber; } }
	public int Life { get { return this.life; } }
	public int Damage { get { return this.damage; } }
	public int RewardXP { get { return this.rewardXP; } }
	public int LootTable { get { return this.lootTable; } }
	public float ItemChance { get { return this.itemChance; } }
	public bool IsBoss { get { return this.isBoss; } }
	public string Display { get { return this.display; } }
	public int TemporaryLife { get { return this.temporaryLife; } set { this.temporaryLife = value; } }
	public int TemporaryDamage { get { return this.temporaryDamage; } set { this.temporaryDamage = value; } }
	public bool WillMiss { get { return this.willMiss; } set { this.willMiss = value; } }

	// Methods
	public static Monster[] GetAllMonsters()
	{
		Monster[] monsters = new Monster[totalMonsters];

		Monster aaa = new Squeak();
		monsters[0] = aaa;
		Monster aab = new GrenYep();
		monsters[1] = aab;
		Monster aac = new Pelly();
		monsters[2] = aac;
		Monster aad = new Flewshoo();
		monsters[3] = aad;
		Monster aae = new Woodwalker();
		monsters[4] = aae;
		Monster aaf = new Bharbeast();
		monsters[5] = aaf;
		Monster aag = new Spidersaurus();
		monsters[6] = aag;
		Monster aah = new Mudge();
		monsters[7] = aah;
		Monster aai = new MotherNature();
		monsters[8] = aai;

		return monsters;
	}

	public static Monster[] GetVaultMonsters()
    {
		Monster[] monsters = new Monster[1];

		KingOfWealth a = new KingOfWealth();
		monsters[0] = a;

		return monsters;
    }

	public static Monster[] GetUnlockedMonstersForArea(int area, Player player)
	{
		Monster[] monsters = GetAllMonsters();
		int n = 0;

		for (int i = 0; i < monsters.Length; i++)
		{
			if (monsters[i].AreaNumber == area && monsters[i].ID <= player.ExploreProgress) ++n;
		}

		Monster[] unlockedMonsters = new Monster[n];

		for (int i = 0; i < n; i++)
		{
			if (monsters[i].AreaNumber == area && monsters[i].ID <= player.ExploreProgress) unlockedMonsters[i] = monsters[i];
		}

		return unlockedMonsters;
	}

	public static Monster[] GetMonstersForArea(int area)
	{
		Monster[] monsters = GetAllMonsters();
		int monstersInArea = GetNumberOfMonstersInArea(area);

		Monster[] areaMonsters = new Monster[monstersInArea];

		for (int i = 0; i < monstersInArea; i++)
		{
			if (monsters[i].AreaNumber == area) areaMonsters[i] = monsters[i];
		}
		return areaMonsters;
	}

	public static int GetNumberOfMonstersInArea(int area)
    {
		Monster[] monsters = GetAllMonsters();
		int n = 0;
		for (int i = 0; i < monsters.Length; i++)
		{
			if (monsters[i].AreaNumber == area) ++n;
		}
		return n;
    }

	public static int GetNumerOfZonesUnlocked(Player player)
	{
		int n;
		Monster[] monsters = GetAllMonsters();
		n = monsters[player.ExploreProgress - 1].AreaNumber;
		return n;
	}

	public static string GetZoneName(int zoneID)
	{
		if (zoneID == 1) return "Forest Area";
		if (zoneID == 2) return "Lergus Village";
		if (zoneID == 3) return "Chaos Outskirts";
		if (zoneID == 4) return "Castle of King Lewph";
		if (zoneID == 5) return "Dark Tunnel";
		else return "Unknown Area";
	}

	/*
	public static Monster[] GetAllAreaOneMonsters()
    {
		Monster[] forestMonsters = new Monster[5]; // Currently cannot exceed 8, same for other areas. (due to user input)

		Spidersaurus a = new Spidersaurus();
		forestMonsters[0] = a;
		Woodwalker b = new Woodwalker();
		forestMonsters[1] = b;
		Bharbeast c = new Bharbeast();
		forestMonsters[2] = c;
		Flewshoo d = new Flewshoo();
		forestMonsters[3] = d;
		MotherNature e = new MotherNature();
		forestMonsters[4] = e;

		return forestMonsters;
    }
	public static Monster[] GetAllAreaTwoMonsters()
    {
		Monster[] villageMonsters = new Monster[3];

		Spidersaurus a = new Spidersaurus();
		villageMonsters[0] = a;
		Woodwalker b = new Woodwalker();
		villageMonsters[1] = b;
		Bharbeast c = new Bharbeast();
		villageMonsters[2] = c;

		return villageMonsters;
	}
	public static Monster[] GetAllAreaThreeMonsters()
    {
		Monster[] outskirtsMonsters = new Monster[2];

		Spidersaurus a = new Spidersaurus();
		outskirtsMonsters[0] = a;
		Woodwalker b = new Woodwalker();
		outskirtsMonsters[1] = b;

		return outskirtsMonsters;
	}
	*/

	public virtual int UseAbility() { return 0; }

	// Attack ability methods
	protected int Attack()
    {
		return FinalizeAttack(this.TemporaryDamage);
    }

	protected int MidAttack() // +- 20%
	{
		int min = (int)((float)this.TemporaryDamage - ((float)this.TemporaryDamage / 5));
		int max = (int)((float)this.TemporaryDamage + ((float)this.TemporaryDamage / 5));
		Random random = new Random();
		return FinalizeAttack(random.Next(min, max + 1));
	}

	protected int WildAttack() // +- 50%
    {
		int min = (int)((float)this.TemporaryDamage - ((float)this.TemporaryDamage / 2));
		int max = (int)((float)this.TemporaryDamage + ((float)this.TemporaryDamage / 2));
		Random random = new Random();
		return FinalizeAttack(random.Next(min, max + 1));
    }

	protected int FlatAttack(int dmg)
    {
		return FinalizeAttack(dmg);
    }

	protected int FlatRangeAttack(int minDmg, int maxDmg)
    {
		Random random = new Random();
		int dmg = random.Next(minDmg, maxDmg + 1);
		return FinalizeAttack(dmg);
    }

	// For custom attack abilities in a child class, simply send the final damage result into this method.
	protected int FinalizeAttack(int damage) // Compute and display damage from a child's attack method
	{
		if (WillMiss)
        {
			Console.WriteLine("You dodged {0} damage! {1} inflicted 0 damage upon you!\n", damage, this.Name);
			return 0;
        }
		Console.WriteLine("{0} inflicted {1} damage upon you!\n", this.Name, APM(damage));
		return APM(damage);
    }

	// Buff ability methods
	protected int Buff() // This buff is a constant 50% of the permanent base damage of this monster
	{
		int buff = this.Damage / 2;
		return FinalizeBuff(buff);
    }

	protected int FlatBuff(int buff)
    {
		return FinalizeBuff(buff);
    }

	protected int BasePercentBuff(double buffPercent) // This buff is a constant percentage of the permanent base damage of this monster
	{
		int buff = (int)((double)this.Damage * buffPercent);
		return FinalizeBuff(buff);
	}

	protected int FluidPercentBuff(double buffPercent) // This buff scales depending on the monster's current damage
	{
		int buff = (int)((double)this.TemporaryDamage * buffPercent);
		return FinalizeBuff(buff);
	}

	protected int FinalizeBuff(int buff) // Display message and return 0
    {
		this.TemporaryDamage += buff;
		Console.WriteLine("{0} increased it's damage by {1}!\n", this.Name, buff);
		return 0;
    }

	// Heal methods
	protected int Heal() // This heal is a constant 25% of the permanent base life of this monster
    {
		int heal = (int)((float)this.Life * 0.25);
		return FinalizeHeal(heal);
    }

	protected int FlatHeal(int heal)
	{
		return FinalizeHeal(heal);
	}

	protected int BasePercentHeal(int healPercent) // This heal is a constant percentage of the permanent base life of this monster
	{
		int heal = (int)((float)this.Life * healPercent);
		return FinalizeHeal(heal);
	}

	protected int FluidPercentHeal(int healPercent) // This heal scales depending on the monster's current life
	{
		int heal = (int)((float)this.TemporaryLife * healPercent);
		return FinalizeHeal(heal);
	}

	protected int FinalizeHeal(int heal) // Display message and return 0
	{
		this.TemporaryLife += heal;
		Console.WriteLine("{0} gained {1} life!\n", this.Name, heal);
		return 0;
	}
}
