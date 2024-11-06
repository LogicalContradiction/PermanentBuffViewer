using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PermanentBuffViewer
{
    public class BuffViewerPlayer : ModPlayer
    {

        /// <summary>
        /// Gets the number of permanent buff items used based on an ItemID.
        /// Valid ItemIDs: Life Crystal (LifeCrystal), Life Fruit (LifeFruit), Mana Crystal (ManaCrystal)
        /// </summary>
        /// <param name="itemID">The ID of the item to get how many have been used. Must be one of the valid IDs.</param>
        /// <returns>The number of permanent buff items of the given type have been used if the ID was valid.</returns>
        /// <exception cref="ArgumentException">The ID given wasn't a valid ID.</exception>
        public int GetNumOfPermanentItemUsed(int itemID)
        {
            switch(itemID)
            {
                case ItemID.LifeCrystal: return this.Player.ConsumedLifeCrystals;
                case ItemID.LifeFruit: return this.Player.ConsumedLifeFruit;
                case ItemID.ManaCrystal: return this.Player.ConsumedManaCrystals;
                default:
                    Mod.Logger.Error("GetNumOfPermanentItemUsed() in BuffViewerPlayer was called with an invalid " +
                        $"ItemID.\nValid IDs:\nLifeCrystal: {ItemID.LifeCrystal}\nLifeFruit: {ItemID.LifeFruit}\n" +
                        $"ManaCrystal: {ItemID.ManaCrystal}\nItemID called: {new Item(itemID).Name} ({itemID})");
                    return -1;
            }
        }

    }
}
