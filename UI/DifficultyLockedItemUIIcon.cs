using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PermanentBuffViewer.UI
{
    /// <summary>
    /// Used to display permanent buffs that are locked to a minimum difficulty.
    /// </summary>
    internal class DifficultyLockedItemUIIcon : BuffItemUIElement
    {

        private Condition minDifficultyAvailable;
        private LocalizedText itemNotAvailableInCurrentDifficulty;
        private Color transparent = new Color(r:0xFF, g:0xFF, b:0xFF, alpha:0.3f);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">The item this element will be displaying.</param>
        /// <param name="usedItem">The condition for showing the full sprite. The sprite will be a silhouette until the item has been used.</param>
        /// <param name="availableDifficulties">The condition representing the difficulties this item is available in.</param>
        /// <param name="itemUsedHoverTextKey">Localization key for the text displayed when an item has been used.</param>
        /// <param name="itemNotUsedHoverTextKey">Localization key for the text displayed when an item has not yet been used.</param>
        /// <param name="howToObtainKey">Localization key describing how to obtain this item.</param>
        /// <param name="statModifiedKey">Localization key describing the stat that is modified.</param>
        /// <param name="itemNotAvailableInCurrentDifficulty">Localization key describing the item not being available in the current world difficulty.</param>
        public DifficultyLockedItemUIIcon(Item item, Condition usedItem, 
            Condition availableDifficulties, string itemUsedHoverTextKey, 
            string itemNotUsedHoverTextKey, string howToObtainKey, 
            string statModifiedKey, string itemNotAvailableInCurrentDifficulty) : 
            base(item, usedItem, itemUsedHoverTextKey, itemNotUsedHoverTextKey, 
                howToObtainKey, statModifiedKey)
        {
            this.minDifficultyAvailable = availableDifficulties;
            this.itemNotAvailableInCurrentDifficulty = 
                Language.GetOrRegister(itemNotAvailableInCurrentDifficulty);
        }

        public override bool ShouldBeAddedToRendering()
        {
            return usedItem.IsMet() || minDifficultyAvailable.IsMet();
        }

        /// <summary>
        /// Calculates the color for use when drawing the sprite.
        /// </summary>
        /// <returns>Color.White if item's been used, Color.Black if it hasn't, transparent white if the item's been used but not available in the world difficulty.</returns>
        internal override Color GetDrawColor()
        {
            if (!minDifficultyAvailable.IsMet())
            {
                if (!usedItem.IsMet())
                {
                    // Item is not available and hasn't been used. Should never be reached.
                    ModContent.GetInstance<PermanentBuffViewer>().Logger.Error(
                        "GetDrawColor() for difficulty locked UI Icon was called in a world where " +
                        "the icon should not be drawn. The following item should not have been added to the " +
                        $"rendered UI: {item.Name}");
                    return Color.Black;
                }
                // Item has been used but isn't available in current world.
                else return transparent;
            }
            // Item was used and is available in this world.
            if (usedItem.IsMet()) return Color.White;
            // Item wasn't used but is available.
            return Color.Black;
        }

        public override string CreateHoverText()
        {
            if (!minDifficultyAvailable.IsMet())
            {
                if (!usedItem.IsMet())
                {
                    // Item is not available and hasn't been used. Should never be reached.
                    ModContent.GetInstance<PermanentBuffViewer>().Logger.Error(
                        "CreateHoverText() for difficulty locked UI Icon was called in a world where " +
                        "the icon should not be drawn. The following item should not have been added to the " +
                        $"rendered UI: {item.Name}");
                    return itemNotUsedHoverText.Format();
                }
                // Item has been used but is not available in current world.
                return itemNotAvailableInCurrentDifficulty.Format();
            }
            // Item was used and is available in this world.
            if (usedItem.IsMet()) return itemUsedHoverText.Format();
            // Item wasn't used but is available.
            return itemNotUsedHoverText.Format();
        }
    }
}
