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
        public BuffItemUIIcon(Item item, Condition usedItem, string itemUsedHoverTextKey, 
            string itemNotUsedHoverTextKey, string amountIncreaseByKey, string statIncrasedKey) : 
            base(item, usedItem, itemUsedHoverTextKey, 
                itemNotUsedHoverTextKey, amountIncreaseByKey, statIncrasedKey)
        {

        }

        public override string CreateHoverText()
        {
            return usedItem.IsMet() ?
                itemUsedHoverText.Format(item.Name, amountIncreaseBy.Value, statIncreased.Value) :
                itemNotUsedHoverText.Format();
        }
    }
}
