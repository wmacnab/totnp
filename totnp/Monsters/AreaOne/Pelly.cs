using System;

public class Pelly : Monster // No parameters are necessary to create a fully-functioning child class of Monster
{
	private const string NAME = "Pelly";
	private const int ID_NUMBER = 3;
	private const int AREA_NUMBER = 1;
	private const int LIFE = 65;
	private const int DAMAGE = 10;
	private const int REWARD_XP = 1;
	private const int LOOT_TABLE = 1;
	private const float ITEM_CHANCE = 0.33f; // Effective for 2 decimal places
	private const bool IS_BOSS = false;

	// Constructors
	public Pelly() : base(NAME, ID_NUMBER, AREA_NUMBER, LIFE, DAMAGE, REWARD_XP, LOOT_TABLE, ITEM_CHANCE, IS_BOSS) { }

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