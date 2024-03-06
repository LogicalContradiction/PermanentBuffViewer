using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using Terraria;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PermanentBuffViewer.UI
{
    /// <summary>
    /// The base class used for displaying items that give a permanent buff.
    /// </summary>
    internal abstract class BuffItemUIElement : UIElement
    {
        internal Item item { get; }

        internal Condition usedItem;

        internal LocalizedText itemUsedHoverText;
        internal LocalizedText itemNotUsedHoverText;
        internal LocalizedText amountIncreaseBy;
        internal LocalizedText statIncreased;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">The item this element will be displaying.</param>
        /// <param name="usedItem">The condition for showing the full sprite. The sprite will be a silhouette until the item has been used.</param>
        /// <param name="itemUsedHoverTextKey">Localization key for the text displayed when an item has been used.</param>
        /// <param name="itemNotUsedHoverTextKey">Localization key for the text displayed when an item has not yet been used.</param>
        /// <param name="amountIncreaseByKey">Localization key describing the amount the stat is increased by.</param>
        /// <param name="statIncrasedKey">Localization key describing the player stat that is increased.</param>
        public BuffItemUIElement(Item item, Condition usedItem,
            string itemUsedHoverTextKey, string itemNotUsedHoverTextKey,
            string amountIncreaseByKey, string statIncrasedKey)
        {
            this.item = item;
            this.usedItem = usedItem;
            itemUsedHoverText = Language.GetOrRegister(itemUsedHoverTextKey);
            itemNotUsedHoverText = Language.GetOrRegister(itemNotUsedHoverTextKey);
            amountIncreaseBy = Language.GetOrRegister(amountIncreaseByKey);
            statIncreased = Language.GetOrRegister(statIncrasedKey);
            base.Width.Set(32f, 0f);
            base.Height.Set(32f, 0f);
        }

        /// <summary>
        /// Creates the hover text that will be shown when the mouse is hovered.
        /// </summary>
        /// <returns>A string containing the text to be shown to the user on hover.</returns>
        public abstract string CreateHoverText();

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 screenPositionForItemCenter = base.GetDimensions().Center();
            ItemSlot.DrawItemIcon(this.item, 31, spriteBatch, screenPositionForItemCenter, this.item.scale, 32f, GetDrawColor());

        }

        /// <summary>
        /// Calculates the color for use when drawing the sprite.
        /// </summary>
        /// <returns>Color.White if the sprite is shown, otherwise Color.Black</returns>
        virtual internal Color GetDrawColor()
        {
            return usedItem.IsMet() ? Color.White : Color.Black;
        }

        /// <summary>
        /// Checks if the current element is in the UI.
        /// </summary>
        /// <returns>True if this element is in the UI, False if it is not.</returns>
        public bool InUI()
        {
            return Parent != null;
        }

        /// <summary>
        /// Determines if this sprite should be added to rendering.
        /// </summary>
        /// <returns>True if this sprite should be added to the renderlist. False if not.</returns>
        virtual public bool ShouldBeAddedToRendering()
        {
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.IsMouseHovering)
            {
                Main.instance.MouseText(CreateHoverText());
            }
        }
    }
}
