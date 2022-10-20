using System;
using System.Runtime.CompilerServices;

// For a copy paste, change: all constants, UseAbility(), and that's all you have to do!

public class MotherNature : Monster // No parameters are necessary to create a fully-functioning child class of Monster
{
	private const string NAME = "Mother Nature";
	private const int ID_NUMBER = 9;
	private const int AREA_NUMBER = 1;
	private const int LIFE = 365;
	private const int DAMAGE = 52;
	private const int REWARD_XP = 5;
	private const int LOOT_TABLE = 101;
	private const float ITEM_CHANCE = 0.75f; // Effective for 2 decimal places
	private const bool IS_BOSS = true;

	// Constructors
	public MotherNature() : base(NAME, ID_NUMBER, AREA_NUMBER, LIFE, DAMAGE, REWARD_XP, LOOT_TABLE, ITEM_CHANCE, IS_BOSS) { }
	//public Spidersaurus() : base("Spidersaurus", 0, 30, 6, 1, 'a', 0.5f) { }

	// Methods
	public override int UseAbility() // Must always return an integer. If the ability isn't an attack, return 0.
	{
		Random random = new Random();
		int r = random.Next(1, 4);

		if (r == 1) return FlatHeal(100);
		if (r == 2) return FluidPercentBuff(.5);
		else return WildAttack();
	}

	public static int GetID()
	{
		return ID_NUMBER;
	}
}