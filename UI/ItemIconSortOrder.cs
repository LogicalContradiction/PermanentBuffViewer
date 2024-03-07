using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace PermanentBuffViewer.UI
{
    /// <summary>
    /// Holds the display ordering of items for use in comparisons by UI elements in a grid.
    /// </summary>
    internal class ItemIconSortOrder
    {
        private List<int> order = new List<int>()
        {
            ItemID.LifeCrystal, ItemID.LifeFruit, ItemID.AegisCrystal,
            ItemID.ManaCrystal, ItemID.ArcaneCrystal,
            ItemID.GummyWorm, ItemID.Ambrosia, ItemID.GalaxyPearl, ItemID.AegisFruit,
            ItemID.ArtisanLoaf, ItemID.TorchGodsFavor, ItemID.DemonHeart, ItemID.MinecartPowerup,
            ItemID.CombatBook, ItemID.CombatBookVolumeTwo, ItemID.PeddlersSatchel,
        };
        // Health
        // Mana
        // Stats
        // Misc.
        // World

        private Dictionary<int, int> lookup;

        public ItemIconSortOrder() 
        {
            BuildLookup();   
        }

        /// <summary>
        /// Builds and sets the "lookup" dictionary to map ItemIDs to their position in the order.
        /// </summary>
        public void BuildLookup()
        {
            // Create lookup to map the id to the location in order
            lookup = new Dictionary<int, int>();
            for (int i = 0; i < order.Count; i++)
            {
                lookup[order[i]] = i;
            }
        }

        /// <summary>
        /// Compares two BuffItemUIElements and returns a value indicating which one should come first.
        /// </summary>
        /// <param name="item1">The first item to compare.</param>
        /// <param name="item2">The second item to compare.</param>
        /// <returns>
        /// item1 before item2 = -1<br/>
        /// item2 before item1 = 1<br/>
        /// item1 equals item2 = 0
        /// </returns>
        public int Compare(BuffItemUIElement item1, BuffItemUIElement item2)
        {
            int item1Position = lookup[item1.item.type];
            int item2Position = lookup[item2.item.type];

            if (item1Position < item2Position) return -1;
            if (item1Position > item2Position) return 1;
            return 0;
        }




    }
}
