using System;
using System.Runtime.InteropServices.ComTypes;

public class Ability
{
	private int number;
	private int requiredLevel;
	private string abilityName;
	private string display;

	public static int numberOfAbilities = 0;

	public int RequiredLevel { get { return this.requiredLevel; } }
	public int Number { get { return this.number; } }
	public string Display { get { return this.display; } }

	public Ability() { }
	public Ability(int number, int requiredLevel, string abilityName)
    {
		this.number = number;
		this.requiredLevel = requiredLevel;
		this.abilityName = abilityName;
		this.display = String.Format("{0}) {1}", this.number, this.abilityName);
		numberOfAbilities++;
    }

	// An object array holding all player abilities
	public static Ability[] GetAllAbilities()
    {
		Ability[] abilities = new Ability[5];

		Ability a = new Attack();
		abilities[0] = a;
		Ability b = new Buff();
		abilities[1] = b;
		Ability c = new Heal();
		abilities[2] = c;
		Ability d = new BuffTwo();
		abilities[3] = d;
		Ability e = new HealTwo();
		abilities[4] = e;

		/*
		Ability e = new BuffTwo();
		abilities[4] = e;
		Ability f = new AttackTwo();
		abilities[5] = f;
		Ability g = new HealThree();
		abilities[6] = g;
		*/

		return abilities;
    }

	// You can return 0 if the ability is one that does not deal damage.
	public virtual int UseAbility(Player player) { return 0; } // virtual keyword allows inherited members to override this method
}
