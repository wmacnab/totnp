using System;
using System.Collections.Generic;
using System.IO;

public static class Store
{
    private static int StoreExclusiveItemChance = 30;
    private static int ProgressionItemChance = 70;
    private static int ChanceForTierOneProgression = 60;
    private static int ChanceForTierTwoProgression = 30;
    private static int ChanceForTierThreeProgression = 9;
    private static int ChanceForTierFourProgression = 1;
    private static int ChanceForTierTwoStoreExclusive = 75;
    private static int ChanceForTierThreeStoreExclusive = 25;
    private static int ChanceForTierFourStoreExclusive = 0;

    private static Item[] wares = new Item[3]; // 5 slots in the shop
    public static Item[] Wares { get { return wares; } }

    // ... 4 regular items and 1 shop specific item for sale...
    // ... refresh the contents of the shop for a certain amount of money...
    // ShopData.txt simply stores items similar to the player inventory.

    public static void FillStoreSlot(int pos)
    {
        wares[pos] = PickItemForSlot();
        SaveWares();
    }

    public static void BuyItem(int pos, Player player)
    {
        bool HasInventorySpace = false;
        int inventorySlotToFill = 0;
        for (int i = 0; i < player.InventoryItems.Length; i++)
        {
            if (player.InventoryItems[i].ID == 0)
            {
                HasInventorySpace = true;
                inventorySlotToFill = i;
                break;
            }
        }
        if (HasInventorySpace) // Must guarantee there is inventory space for Player.AwardItem(Item item) method
        {
            //player.InventoryItems[inventorySlotToFill] = wares[pos];
            player.DecreaseMoney(Wares[pos - 1].Money);
            player.AwardItem(Wares[pos - 1]);
            FillStoreSlot(pos - 1);
        }
        else
        {
            Console.WriteLine("Your inventory is full.\n");
        }
    }

    public static void LoadWares()
    {
        StreamReader reader = new StreamReader("StoreData.txt");
        Item[] items = Item.GetAllItemsArray();
        using (reader)
        {
            for (int i = 0; i < wares.Length; i++)
            {
                wares[i] = items[int.Parse(reader.ReadLine())];
            }
        }
        reader.Close();
    }

    public static void SaveWares()
    {
        StreamWriter writer = new StreamWriter("StoreData.txt");
        using (writer)
        {
            for (int i = 0; i < wares.Length; i++)
            {
                writer.WriteLine(wares[i].ID);
            }
        }
        writer.Close();
    }

    public static void RefreshWares()
    {
        for (int i = 0; i < Wares.Length; i++)
        {
            FillStoreSlot(i);
        }
    }

    // Method that uses probabilities to find what item should be placed into the wares[] array representing the shop inventory...
    // ... called by the FillStoreSlot(int pos) method.
    private static Item PickItemForSlot()
    {
        Random random = new Random();
        int r = random.Next(1, 101);

        List<Item> itemsOfTier = new List<Item>();
        Item selectedItem;

        if (r <= StoreExclusiveItemChance) // Shop exclusive item
        {
            Item[] storeItems = Item.GetStoreItemsArray();
            r = random.Next(1, 101);

            if (r <= ChanceForTierTwoStoreExclusive)
            {
                foreach (Item item in storeItems)
                {
                    if (item.Tier == 2) itemsOfTier.Add(item);
                }
            }
            else if (r <= ChanceForTierThreeStoreExclusive + ChanceForTierTwoStoreExclusive)
            {
                foreach (Item item in storeItems)
                {
                    if (item.Tier == 3) itemsOfTier.Add(item);
                }
            }
            else if (r <= ChanceForTierFourStoreExclusive + ChanceForTierThreeStoreExclusive + ChanceForTierTwoStoreExclusive)
            {
                foreach (Item item in storeItems)
                {
                    if (item.Tier == 4) itemsOfTier.Add(item);
                }
            }

            // Returning a remaining item
            r = random.Next(0, itemsOfTier.Count);
            selectedItem = itemsOfTier[r];
            return selectedItem;
        }



        else if (r <= ProgressionItemChance + StoreExclusiveItemChance) // Progression item
        {
            // Getting item pool
            Item[] allItems = Item.GetAllItemsArray();

            // Randomly selecting the tier of the item
            r = random.Next(1, 101);

            // Adding all items of the selected tier to the itemsOfTier list
            if (r <= ChanceForTierOneProgression) // T1 progression item
            {
                foreach (Item item in allItems)
                {
                    if (item.Tier == 1) itemsOfTier.Add(item);
                }
            }
            else if (r <= ChanceForTierTwoProgression + ChanceForTierOneProgression)
            {
                foreach (Item item in allItems)
                {
                    if (item.Tier == 2) itemsOfTier.Add(item);
                }
            }
            else if (r <= ChanceForTierThreeProgression + ChanceForTierTwoProgression + ChanceForTierOneProgression)
            {
                foreach (Item item in allItems)
                {
                    if (item.Tier == 3) itemsOfTier.Add(item);
                }
            }
            else if (r <= ChanceForTierFourProgression + ChanceForTierThreeProgression + ChanceForTierTwoProgression + ChanceForTierOneProgression)
            {
                foreach (Item item in allItems)
                {
                    if (item.Tier == 4) itemsOfTier.Add(item);
                }
            }

            // Returning a random remaining item
            r = random.Next(0, itemsOfTier.Count);
            selectedItem = itemsOfTier[r];
            return selectedItem;
        }

        // If for some reason no choice above is returned
        selectedItem = new ItemZero();
        return selectedItem;
    }
}
