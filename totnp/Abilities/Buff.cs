using System;
using System.IO;

public class Buff : Ability
{
	// Static, copied when ability is used.
	//private static int BuffAmount;

	public const int DEFAULT_NUMBER = 2;
	public const int REQUIRED_LEVEL = 1;
	public const string DEFAULT_NAME = "Buff";

	// Constant specific to this ability
	public const int DEFAULT_BUFFAMOUNT = 5;

	// Constructors
	public Buff() : this(DEFAULT_NUMBER, REQUIRED_LEVEL, DEFAULT_NAME) { }
	public Buff(int number, int requiredLevel, string abilityName) : base(number, requiredLevel, abilityName)
    {
		//this.Display += String.Format(" ({0})", buffAmount);
    }

	// Methods
	public override int UseAbility(Player player)
	{
		//player.TemporaryDamage += DEFAULT_BUFFAMOUNT;
		//int playerBaseDamage = player.Damage + (int)((float)player.Damage * (1 + ((float)player.TemporaryDamageBuffPercent / 100)));

		int buff = (player.TemporaryDamage / 10) + DEFAULT_BUFFAMOUNT;
		player.TemporaryDamage += buff;
		Console.WriteLine("You increased your damage by {0} points!{1}", buff, "\n");
		return 0;
	}
}
