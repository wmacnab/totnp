using System;

// Check documentation for game design information
// Regular progression tables are number chronologically. 1, 2, 3, ..., n.
// Special mini boss or boss tables begin at 101, 102, 103, ..., n.
// Vault tables begin at 1_001, 1_002, 1_003, ..., n.
public static class RewardTables
{
	private static Random random = new Random();

	// This method is called, identifies which reward table the monster has, and then calls the correct method within this class
	public static void PickAndApplyReward(int table, Player player)
    {
		if (table == 1) TableOne(player);
		else if (table == 2) TableTwo(player);
		else if (table == 3) TableThree(player);
		else if (table == 101) TableOneHundredOne(player);
		else if (table == 1_001) TableOneThousandOne(player);
    }

	// Zone 1 reward table
	private static void TableOne(Player player)
    {
		int r = random.Next(1, 101);

		if (r <= 50) player.AwardItem(Item.PickProgressionItem(1, 60, 30, 10));
		else if (r <= 98) player.AwardMoney(1);
		else if (r <= 100) player.AwardLife(1);
	}

	private static void TableTwo(Player player)
	{
		int r = random.Next(1, 101);

		if (r <= 50) player.AwardItem(Item.PickProgressionItem(1, 30, 60, 10));
		else if (r <= 96) player.AwardMoney(2);
		else if (r <= 100) player.AwardLife(1);
	}

	private static void TableThree(Player player)
	{
		int r = random.Next(1, 101);

		if (r <= 50) player.AwardItem(Item.PickProgressionItem(1, 10, 60, 30));
		else if (r <= 94) player.AwardMoney(3);
		else if (r <= 100) player.AwardLife(1);
	}

	// Zone 1 gatekeeper reward table
	private static void TableOneHundredOne(Player player)
	{
		int r = random.Next(1, 101);

		if (r <= 50) player.AwardItem(Item.PickProgressionItem(1, 10, 30, 60));
		else if (r <= 80) player.AwardMoney(4);
		else if (r <= 82)
		{
			Item MN_LIGHT = new ItemSeven();
			player.AwardItem(MN_LIGHT);
		}
		else if (r <= 91) player.AwardLife(1);
		else if (r <= 92) player.AwardLife(2);
		else if (r <= 93) player.AwardLifeRegen(1);
		else if (r <= 94) player.AwardLifeRegen(2);
		else if (r <= 95) player.AwardDodge(1);
		else if (r <= 96) player.AwardPrevention(1);
		else if (r <= 97) player.AwardCritical(1);
		else if (r <= 98) player.AwardSuperCritical(1);
		else if (r <= 99) player.AwardMiracle(1);
		else if (r <= 100) player.AwardSuperMiracle(1);
	}

	// First vault reward table
	private static void TableOneThousandOne(Player player)
    {
		int r = random.Next(1, 101);

		if (r <= 60) player.AwardMoney(5);
		else if (r <= 90) player.AwardMoney(10);
		else if (r <= 100) player.AwardMoney(15);
        {
			Item SEI = new StoreItemOne();
			player.AwardItem(SEI);
        }
	}
}
