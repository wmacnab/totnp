using System;

public class Attack : Ability
{
	public const int DEFAULT_NUMBER = 1; // ID for the skill which impacts what key it is bound to
	public const int REQUIRED_LEVEL = 1;
	public const string DEFAULT_NAME = "Attack";

	// Constructors
	public Attack() : this(DEFAULT_NUMBER, REQUIRED_LEVEL, DEFAULT_NAME) { }
	public Attack(int number, int requiredLevel, string abilityName) : base(number, requiredLevel, abilityName) { }

	// Methods
	public override int UseAbility(Player player)
    {
		//int currentDamageMin = player.Damage
		//int currentDamageMax;
		int damageResult = player.TemporaryDamage;

		Console.WriteLine("You inflicted {0} damage upon your foe!{1}", damageResult, "\n");

		return damageResult;
    }
}
