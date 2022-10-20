using System;

public class KingOfWealth : Monster // No parameters are necessary to create a fully-functioning child class of Monster
{
	private const string NAME = "King of Wealth";
	private const int ID_NUMBER = 1001;
	private const int AREA_NUMBER = 1001;
	private const int LIFE = 1000;
	private const int DAMAGE = 1;
	private const int REWARD_XP = 1;
	private const int LOOT_TABLE = 1001;
	private const float ITEM_CHANCE = 1.00f; // Effective for 2 decimal places
	private const bool IS_BOSS = false;

	// Constructors
	public KingOfWealth() : base(NAME, ID_NUMBER, AREA_NUMBER, LIFE, DAMAGE, REWARD_XP, LOOT_TABLE, ITEM_CHANCE, IS_BOSS) { }

	// Methods
	public override int UseAbility() // Must always return an integer. If the ability isn't an attack, return 0.
	{
		Random random = new Random();
		int r = random.Next(1, 101);

		if (r <= 20) return FlatAttack(1);
		else if (r <= 40) return FlatAttack(2);
		else if (r <= 60) return FlatAttack(3);
		else if (r <= 80) return FlatAttack(5);
		return FlatAttack(8);
	}

	public static int GetID()
	{
		return ID_NUMBER;
	}
}