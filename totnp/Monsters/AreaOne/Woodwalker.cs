using System;

public class Woodwalker : Monster // No parameters are necessary to create a fully-functioning child class of Monster
{
	private const string NAME = "Woodwalker";
	private const int ID_NUMBER = 5;
	private const int AREA_NUMBER = 1;
	private const int LIFE = 99;
	private const int DAMAGE = 15;
	private const int REWARD_XP = 2;
	private const int LOOT_TABLE = 2;
	private const float ITEM_CHANCE = 0.50f; // Effective for 2 decimal places
	private const bool IS_BOSS = false;

	// Constructors
	public Woodwalker() : base(NAME, ID_NUMBER, AREA_NUMBER, LIFE, DAMAGE, REWARD_XP, LOOT_TABLE, ITEM_CHANCE, IS_BOSS) { }
	//public Spidersaurus() : base("Spidersaurus", 0, 30, 6, 1, 'a', 0.5f) { }

	// Methods
	public override int UseAbility() // Must always return an integer. If the ability isn't an attack, return 0.
	{
		// Randomly selecting which ability will be used
		Random random = new Random();
		int r = random.Next(1, 6); // Randomly pick a number between 1 and 5

		// 20% chance to buff, 80% chance to attack
		if (r == 1) return BasePercentBuff(0.7);
		return MidAttack();
	}

	public static int GetID()
	{
		return ID_NUMBER;
	}
}