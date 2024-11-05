using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace PermanentBuffViewer.UI
{
    /// <summary>
    /// Used to display permanent buff items that can be used only once.
    /// </summary>
    internal class BuffItemUIIcon : BuffItemUIElement
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">The item this element will be displaying.</param>
        /// <param name="usedItem">The condition for showing the full sprite. The sprite will be a silhouette until the item has been used.</param>
        /// <param name="itemUsedHoverTextKey">Localization key for the text displayed when an item has been used.</param>
        /// <param name="itemNotUsedHoverTextKey">Localization key for the text displayed when an item has not yet been used.</param>
        /// <param name="howToObtainKey">Localization key describing how to obtain this item.</param>
        /// <param name="statModifiedKey">Localization key describing the stat that is modified.</param>
        public BuffItemUIIcon(Item item, Condition usedItem, string itemUsedHoverTextKey, 
            string itemNotUsedHoverTextKey, string howToObtainKey, string statModifiedKey) : 
            base(item, usedItem, itemUsedHoverTextKey, 
                itemNotUsedHoverTextKey, howToObtainKey, statModifiedKey)
        {

        }

        public override string CreateHoverText()
        {
            return usedItem.IsMet() ?
                itemUsedHoverText.Format(Item.Name, statModified.Value) :
                itemNotUsedHoverText.Format(howToObtainText.Value);
        }

        public override string ToString()
        {
            return $"Buff Icon item: {Item.Name}, Condition: {usedItem}, itemUsedText: \"{itemUsedHoverText}\", " +
                $"itemNotUsedText: \"{itemNotUsedHoverText}\", howToObtainText: \"{howToObtainText}\", statModifiedText: \"{statModified}\"";
        }
    }
}
