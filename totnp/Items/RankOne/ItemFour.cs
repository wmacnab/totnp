using System;

public class ItemFour : Item
{
	private const string NAME = "ItemFour";
	private const int ID = 4;
	public const int RANK = 1;
	private const int TIER = 2;
	private const int MONEY = 10;

	private const int FLAT_LIFE = 10;
	private const int PERCENT_LIFE = 10;
	private const int FLAT_DAMAGE = 0;
	private const int PERCENT_DAMAGE = 0;
	private const int LIFE_REGEN = 0;
	private const int MIRACLE = 0;
	private const int SUPER_MIRACLE = 0;
	private const int CRITICAL = 0;
	private const int SUPER_CRITICAL = 0;
	private const int FLAT_RESISTANCE = 0;
	private const int PERCENT_RESISTANCE = 0;
	private const int PREVENTION = 0;
	private const int DODGE = 0;

	public ItemFour() : this(NAME, ID, RANK, TIER, MONEY, FLAT_LIFE, PERCENT_LIFE, FLAT_DAMAGE, PERCENT_DAMAGE, LIFE_REGEN, MIRACLE, SUPER_MIRACLE, CRITICAL, SUPER_CRITICAL,
		FLAT_RESISTANCE, PERCENT_RESISTANCE, PREVENTION, DODGE)
	{ }
	public ItemFour(string name, int id, int rank, int tier, int money, int flatLife, int percentLife, int flatDamage, int percentDamage, int lifeRegen, int miracle, int superMiracle,
		int critical, int superCritical, int flatResistance, int percentResistance, int prevention, int dodge) : base(name, id, rank, tier, money, flatLife,
			percentLife, flatDamage, percentDamage, lifeRegen, miracle, superMiracle, critical, superCritical,
			flatResistance, percentResistance, prevention, dodge)
	{ }
}
