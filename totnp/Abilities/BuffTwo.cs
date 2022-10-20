using System;
using System.IO;

public class BuffTwo : Ability
{

	public const int DEFAULT_NUMBER = 4;
	public const int REQUIRED_LEVEL = 5;
	public const string DEFAULT_NAME = "Rant Force";

	// Instance variables specific to this ability
	private int buff;
	private bool usedLast;
	private bool isActive;

	// Constant specific to this ability
	public const float DEFAULT_BUFFAMOUNT = 2.00f;

	// Accessor properties
	public bool IsActive { get { return this.isActive; } set { this.isActive = value; } }
	public bool UsedLast { get { return this.usedLast; } set { this.usedLast = value; } }
	public int Buff { get { return this.buff; } set { this.buff = value; } }

	// Constructors
	public BuffTwo() : this(DEFAULT_NUMBER, REQUIRED_LEVEL, DEFAULT_NAME, DEFAULT_BUFFAMOUNT) { }
	public BuffTwo(int number, int requiredLevel, string abilityName, float buffAmount) : base(number, requiredLevel, abilityName)
	{
		//this.Display += String.Format(" ({0:P})", buffAmount);
		this.isActive = false;
	}

	// Methods
	public override int UseAbility(Player player)
	{
		//float tempBuffAmount = 0.3f;
		//float increasedDamage = (DEFAULT_BUFFAMOUNT * (float)player.TemporaryDamage);
		//int increasedDamagePoints = (int)increasedDamage;
		//player.TemporaryDamage += increasedDamagePoints;
		//if (usedLast) player.TemporaryDamage -= buff;

		//this.buff = (int)(player.TemporaryDamage * DEFAULT_BUFFAMOUNT);
		//player.TemporaryDamage += buff;
		Console.WriteLine("Rant Force: you have 2x damage for 1 turn!\n");
		this.isActive = true;
		//this.usedLast = true;
		return 0;

	}
	/* Tested flat % method
	public override int UseAbility(Player player)
	{
		//float tempBuffAmount = 0.3f;
		float increasedDamage = (DEFAULT_BUFFAMOUNT * ((float)player.Damage * (1 + (float)player.DamageBuffPercent / 100)));
		int increasedDamagePoints = (int)increasedDamage;

		player.TemporaryDamage += increasedDamagePoints;

		Console.WriteLine("You increased your damage by {0} points!{1}", increasedDamagePoints, "\n");

		return 0;
	}*/
}
