using System;

public class Bharbeast : Monster // No parameters are necessary to create a fully-functioning child class of Monster
{
	private const string NAME = "Bharbeast";
	private const int ID_NUMBER = 6;
	private const int AREA_NUMBER = 1;
	private const int LIFE = 129;
	private const int DAMAGE = 18;
	private const int REWARD_XP = 2;
	private const int LOOT_TABLE = 2;
	private const float ITEM_CHANCE = 0.50f; // Effective for 2 decimal places
	private const bool IS_BOSS = false;

	// Constructors
	public Bharbeast() : base(NAME, ID_NUMBER, AREA_NUMBER, LIFE, DAMAGE, REWARD_XP, LOOT_TABLE, ITEM_CHANCE, IS_BOSS) { }
	//public Spidersaurus() : base("Spidersaurus", 0, 30, 6, 1, 'a', 0.5f) { }

	// Methods
	public override int UseAbility() // Must always return an integer. If the ability isn't an attack, return 0.
	{
		return WildAttack();
	}

	public static int GetID()
	{
		return ID_NUMBER;
	}
}