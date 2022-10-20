using System;
using System.Collections.Generic;

public class Item
{
	private string name;
	private int id; // Position in the allItems[] array
	private int rank; // The area # it drops in
	private int tier; // Rarity
	private int money; // What it sells for

	private string display;

	private int flatLife;
	private int percentLife;
	private int flatDamage;
	private int percentDamage;
	private int lifeRegen;
	private int miracle;
	private int superMiracle;
	private int critical;
	private int superCritical;
	private int flatResistance;
	private int percentResistance;
	private int prevention;
	private int dodge;

	// 1/100, % chance to select a particular tier.
	private const int TIER_ONE_WEIGHT = 60;
	private const int TIER_TWO_WEIGHT = 30;
	private const int TIER_THREE_WEIGHT = 10;

	public string Name { get { return this.name; } }
	public int ID { get { return this.id; } }
	public int Rank { get { return this.rank; } }
	public int Tier { get { return this.tier; } }
	public int Money { get { return this.money; } }
	public int FlatLife { get { return this.flatLife; } }
	public int PercentLife { get { return this.percentLife; } }
	public int FlatDamage { get { return this.flatDamage; } }
	public int PercentDamage { get { return this.percentDamage; } }
	public int LifeRegen { get { return this.lifeRegen; } }
	public int Miracle { get { return this.miracle; } }
	public int SuperMiracle { get { return this.superMiracle; } }
	public int Critical { get { return this.critical; } }
	public int SuperCritical { get { return this.superCritical; } }
	public int FlatResistance { get { return this.flatResistance; } }
	public int PercentResistance { get { return this.percentResistance; } }
	public int Prevention { get { return this.prevention; } }
	public int Dodge { get { return this.dodge; } }

	public Item() { }
	public Item(string name, int id, int rank, int tier, int money, int flatLife, int percentLife, int flatDamage, int percentDamage, int lifeRegen, int miracle, int superMiracle, int critical, int superCritical,
		int flatResistance, int percentResistance, int prevention, int dodge)
    {
		this.name = name;
		this.id = id;
		this.rank = rank;
		this.tier = tier;
		this.money = money;
		this.flatLife = flatLife;
		this.percentLife = percentLife;
		this.flatDamage = flatDamage;
		this.percentDamage = percentDamage;
		this.lifeRegen = lifeRegen;
		this.miracle = miracle;
		this.superMiracle = superMiracle;
		this.critical = critical;
		this.superCritical = superCritical;
		this.flatResistance = flatResistance;
		this.percentResistance = percentResistance;
		this.prevention = prevention;
		this.dodge = dodge;

		this.display = "???";
    }

	public static Item[] GetAllItemsArray() // Each particular item in the array is positioned in the array equal to it's ID#.
    {
		Item[] p = GetProgressionItemsArray();
		Item[] s = GetStoreItemsArray();

		Item[] allItems = new Item[p.Length + s.Length];

        for (int i = 0; i < allItems.Length; i++)
        {
            foreach (Item item in p) { if (item.ID == i) allItems[i] = item; }
			foreach (Item item in s) { if (item.ID == i) allItems[i] = item; }
        }

		return allItems;
    }

	public static Item[] GetProgressionItemsArray()
    {
		Item[] progressionItems = new Item[8];

		Item aaa = new ItemZero();
		progressionItems[aaa.id] = aaa;

		// Zone 1 items
		Item aab = new ItemOne();
		progressionItems[aab.id] = aab;
		Item aac = new ItemTwo();
		progressionItems[aac.id] = aac;
		Item aad = new ItemThree();
		progressionItems[aad.id] = aad;
		Item aae = new ItemFour();
		progressionItems[aae.id] = aae;
		Item aaf = new ItemFive();
		progressionItems[aaf.id] = aaf;
		Item aag = new ItemSix();
		progressionItems[aag.id] = aag;
		Item aah = new ItemSeven(); // Mother Nature's item
		progressionItems[aah.id] = aah;

		// From here I will have to hard code index because of new shop items

		return progressionItems;
    }

	public static Item[] GetStoreItemsArray()
    {
		Item[] storeItems = new Item[7];

		Item aaa = new StoreItemOne();
		storeItems[0] = aaa;
		Item aab = new StoreItemTwo();
		storeItems[1] = aab;
		Item aac = new StoreItemThree();
		storeItems[2] = aac;
		Item aad = new StoreItemFour();
		storeItems[3] = aad;
		Item aae = new StoreItemFive();
		storeItems[4] = aae;
		Item aaf = new StoreItemSix();
		storeItems[5] = aaf;
		Item aag = new StoreItemSeven();
		storeItems[6] = aag;

		return storeItems;
    }

	public static Item PickProgressionItem(int rank)
    {
		return PickProgressionItem(rank, TIER_ONE_WEIGHT, TIER_TWO_WEIGHT, TIER_THREE_WEIGHT);
    }

	public static Item PickProgressionItem(int rank, int w1, int w2, int w3)
    {
		Item[] items = GetProgressionItemsArray();
		List<Item> itemsOfRank = new List<Item>();

		// Consider a varification clause such as this: if(w1 + w2 + w3 != 100)

		// Adding items of the specified rank to a list
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].rank == rank) itemsOfRank.Add(items[i]);
		}

		// Randomly selecting a tier, then moving every item of the chosen tier to a new list
		Random random = new Random();
		int r = random.Next(1, 101);

		if (r <= w1) r = 1;
		else if (r <= w2 + w1) r = 2;
		else if (r <= w3 + w2 + w1) r = 3;

		List<Item> itemsOfRankAndTier = new List<Item>();
		for (int i = 0; i < itemsOfRank.Count; i++)
		{
			if (itemsOfRank[i].tier == r) itemsOfRankAndTier.Add(itemsOfRank[i]);
		}

		// Randomly returning a random remaining item
		r = random.Next(0, itemsOfRankAndTier.Count); // Remember that lists begin at 0 as well
		return itemsOfRankAndTier[r];
	}

    public override string ToString()
    {
		string info = "";
		info += String.Format("{0} {1}.{2} ({3})\n", name, rank, tier, money);
		//info += String.Format("{0}\n", name);
		//info += String.Format("Value: {0}\n", money);

		if (flatLife > 0) info += "+" + flatLife + " life\n";
		if (percentLife > 0) info += "+" + percentLife + "% life\n";
		if (flatDamage > 0) info += "+" + flatDamage + " damage\n";
		if (percentDamage > 0) info += "+" + percentDamage + "% damage\n";
		if (lifeRegen > 0) info += "+" + lifeRegen + " life regen\n";
		if (miracle > 0) info += "+" + miracle + "% miracle\n";
		if (superMiracle > 0) info += "+" + superMiracle + "% super miracle\n";
		if (critical > 0) info += "+" + critical + "% critical\n";
		if (superCritical > 0) info += "+" + superCritical + "% super critical\n";
		if (flatResistance > 0) info += "+" + flatResistance + " resistance\n";
		if (percentResistance > 0) info += "+" + percentResistance + "% resistance\n";
		if (prevention > 0) info += "+" + prevention + "% prevention\n";
		if (dodge > 0) info += "+" + dodge + "% dodge";

		return info;
    }

	public string ToStringWithSellPrice()
	{
		string info = "";
		info += String.Format("{0} {1}.{2} ({3})\n", name, rank, tier, money / 5);
		//info += String.Format("{0}\n", name);
		//info += String.Format("Value: {0}\n", money);

		if (flatLife > 0) info += "+" + flatLife + " life\n";
		if (percentLife > 0) info += "+" + percentLife + "% life\n";
		if (flatDamage > 0) info += "+" + flatDamage + " damage\n";
		if (percentDamage > 0) info += "+" + percentDamage + "% damage\n";
		if (lifeRegen > 0) info += "+" + lifeRegen + " life regen\n";
		if (miracle > 0) info += "+" + miracle + "% miracle\n";
		if (superMiracle > 0) info += "+" + superMiracle + "% super miracle\n";
		if (critical > 0) info += "+" + critical + "% critical\n";
		if (superCritical > 0) info += "+" + superCritical + "% super critical\n";
		if (flatResistance > 0) info += "+" + flatResistance + " resistance\n";
		if (percentResistance > 0) info += "+" + percentResistance + "% resistance\n";
		if (prevention > 0) info += "+" + prevention + "% prevention\n";
		if (dodge > 0) info += "+" + dodge + "% dodge";

		return info;
	}
}
