using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace PermanentBuffViewer
{
    public class BuffViewerPlayer : ModPlayer
    {

        public int GetNumOfPermanentItemUsed(int itemID)
        {
            switch(itemID)
            {
                case ItemID.LifeCrystal: return this.Player.ConsumedLifeCrystals;
                case ItemID.LifeFruit: return this.Player.ConsumedLifeFruit;
                case ItemID.ManaCrystal: return this.Player.ConsumedManaCrystals;
                default: 
                    throw new ArgumentException(
                        "GetNumOfPermanentItemUsed() in BuffViewerPlayer was called with an invalid " +
                        $"ItemID.\nValid IDs:\nLifeCrystal: {ItemID.LifeCrystal}\nLifeFruit: {ItemID.LifeFruit}\n" +
                        $"ManaCrystal: {ItemID.ManaCrystal}\nItemID called: {itemID}");
            }
        }
    }
}
