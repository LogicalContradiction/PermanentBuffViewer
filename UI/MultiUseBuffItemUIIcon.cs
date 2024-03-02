﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace PermanentBuffViewer.UI
{
    /// <summary>
    /// Used to display permanent buffs that can be used more than once up to a maximum number.
    /// </summary>
    internal class MultiUseBuffItemUIIcon : BuffItemUIElement
    {
        private int maxNumCanUse;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">The item this element will be displaying.</param>
        /// <param name="usedItem">The condition for showing the full sprite. The sprite will be a silhouette until the item has been used./param>
        /// <param name="maxNumCanUse">The maximum number of this item a single player can use.</param>
        /// <param name="itemUsedHoverTextKey">Localization key for the text displayed when an item has been used.</param>
        /// <param name="itemNotUsedHoverTextKey">Localization key for the text displayed when an item has not yet been used.</param>
        /// <param name="amountIncreaseByKey">Localization key describing the amount the stat is increased by.</param>
        /// <param name="statIncrasedKey">Localization key describing the player stat that is increased.</param>
        public MultiUseBuffItemUIIcon(Item item, Condition usedItem, int maxNumCanUse, string itemUsedHoverTextKey, 
            string itemNotUsedHoverTextKey, string amountIncreaseByKey, string statIncrasedKey) : 
            base(item, usedItem, itemUsedHoverTextKey, itemNotUsedHoverTextKey, amountIncreaseByKey, 
                statIncrasedKey)
        {
            this.maxNumCanUse = maxNumCanUse;
        }

        public override string CreateHoverText()
        {
            return usedItem.IsMet() ?
                itemUsedHoverText.Format(item.Name, 
                Main.LocalPlayer.GetModPlayer<BuffViewerPlayer>().GetNumOfPermanentItemUsed(item.type),
                maxNumCanUse, amountIncreaseBy.Value, statIncreased.Value) : 
                itemNotUsedHoverText.Format();
        }
    }
}
