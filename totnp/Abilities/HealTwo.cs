using System;
using System.IO;

public class HealTwo : Ability
{

	public const int DEFAULT_NUMBER = 5;
	public const int REQUIRED_LEVEL = 7;
	public const string DEFAULT_NAME = "Healing Touch";

	// Constant specific to this ability
	//public const int DEFAULT_REGENAMOUNT = 10;
	public const int DEFAULT_TURNS = 3;

	// Instance variables specific to this ability
	private int healingTouchTurns;
	private bool isActive;
	public int HealingTouchTurns { get { return this.healingTouchTurns; } set { this.healingTouchTurns = value; } }
	public bool IsActive { get { return this.isActive; } set { this.isActive = value; } }

	// Constructors
	public HealTwo() : this(DEFAULT_NUMBER, REQUIRED_LEVEL, DEFAULT_NAME) { }
	public HealTwo(int number, int requiredLevel, string abilityName) : base(number, requiredLevel, abilityName)
    {
		this.IsActive = false;
    }

	// Methods
	public override int UseAbility(Player player)
	{
		// This was moved to the bottom of the combat method to account for critical and other buffs interfering with with the division
		//int debuff = player.TemporaryDamage / 2;
		//player.TemporaryDamage -= debuff;

		healingTouchTurns = DEFAULT_TURNS;
		this.isActive = true;

		Console.WriteLine("You are granted healing touch for {0} turns! Your damage was halved.\n", HealingTouchTurns);

		return 0;
	}
}
