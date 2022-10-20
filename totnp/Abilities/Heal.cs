using System;
using System.Collections.Generic;
using System.IO;

public class Heal : Ability
{
	// Static, copied when ability is used.
	public const int DEFAULT_NUMBER = 3;
	public const int REQUIRED_LEVEL = 3;
	public const string DEFAULT_NAME = "Regen";

	// Instance variables specific to this ability
	private bool isActive;
	private List<int> totalRegen = new List<int>();
	private List<int> allRegenTurns = new List<int>();
	public List<int> TotalRegen { get { return this.totalRegen; } }
	public List<int> AllRegenTurns { get { return this.allRegenTurns; } }

	// Constant specific to this ability
	public const float DEFAULT_REGENPERCENTAGE = 0.06f;
	public const int DEFAULT_TURNS = 5;

	// Constructors
	public Heal() : this(DEFAULT_NUMBER, REQUIRED_LEVEL, DEFAULT_NAME) { }
	public Heal(int number, int requiredLevel, string abilityName) : base(number, requiredLevel, abilityName)
    {
		this.isActive = false;
    }

	// Methods
	public override int UseAbility(Player player)
	{
		// Calculating the regen
		float BaseLife = ((float)(player.Life) * (1 + ((float)player.TemporaryLifeBuffPercent / 100)));
		int regen = (int)(BaseLife * DEFAULT_REGENPERCENTAGE);
		totalRegen.Add(regen);
		allRegenTurns.Add(DEFAULT_TURNS);
		//player.TemporaryLifeRegen += regen;
		Console.WriteLine("You increased your life regen by {0} for {1} turns!\n", regen, DEFAULT_TURNS);



		return 0;
	}

	private void ApplyRegenInstances(int regen, Player player)
    {

    }
}