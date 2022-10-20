using System;

// For a copy past, change: all constants, UseAbility(), and that's all you have to do!

public class Flewshoo : Monster // No parameters are necessary to create a fully-functioning child class of Monster
{
	private const string NAME = "Flewshoo";
	private const int ID_NUMBER = 4;
	private const int AREA_NUMBER = 1;
	private const int LIFE = 80;
	private const int DAMAGE = 12;
	private const int REWARD_XP = 2;
	private const int LOOT_TABLE = 2;
	private const float ITEM_CHANCE = 0.50f; // Effective for 2 decimal places
	private const bool IS_BOSS = false;

	// Constructors
	public Flewshoo() : base(NAME, ID_NUMBER, AREA_NUMBER, LIFE, DAMAGE, REWARD_XP, LOOT_TABLE, ITEM_CHANCE, IS_BOSS) { }
	//public Spidersaurus() : base("Spidersaurus", 0, 30, 6, 1, 'a', 0.5f) { }

	// Methods
	public override int UseAbility()
	{
		return WildAttack();
	}

	public static int GetID()
	{
		return ID_NUMBER;
	}
}