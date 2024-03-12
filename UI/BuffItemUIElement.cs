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
using Terraria.GameContent;
using Terraria.ID;
using ReLogic.Content;

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
        internal LocalizedText howToObtainText;
        internal LocalizedText statModified;

        internal static ItemIconSortOrder sortOrder;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">The item this element will be displaying.</param>
        /// <param name="usedItem">The condition for showing the full sprite. The sprite will be a silhouette until the item has been used.</param>
        /// <param name="itemUsedHoverTextKey">Localization key for the text displayed when an item has been used.</param>
        /// <param name="itemNotUsedHoverTextKey">Localization key for the text displayed when an item has not yet been used.</param>
        /// <param name="howToObtainKey">Localization key describing how this item is obtained.</param>
        /// <param name="statModifiedKey">Localization key describing the stat that is modified.</param>
        public BuffItemUIElement(Item item, Condition usedItem,
            string itemUsedHoverTextKey, string itemNotUsedHoverTextKey,
            string howToObtainKey, string statModifiedKey)
        {
            this.item = item;
            this.usedItem = usedItem;
            itemUsedHoverText = Language.GetOrRegister(itemUsedHoverTextKey);
            itemNotUsedHoverText = Language.GetOrRegister(itemNotUsedHoverTextKey);
            howToObtainText = Language.GetOrRegister(howToObtainKey);
            this.statModified = Language.GetOrRegister(statModifiedKey);
            base.Width.Set(32f, 0f);
            base.Height.Set(32f, 0f);
            if (sortOrder == null) sortOrder = new ItemIconSortOrder();
        }

        /// <summary>
        /// Creates the hover text that will be shown when the mouse is hovered.
        /// </summary>
        /// <returns>A string containing the text to be shown to the user on hover.</returns>
        public abstract string CreateHoverText();

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 screenPositionForItemCenter = base.GetDimensions().Center();
            //ItemSlot.DrawItemIcon(this.item, 31, spriteBatch, screenPositionForItemCenter, this.item.scale, 32f, GetDrawColor());
            int itemID = item.type;
            Texture2D value = Main.Assets.Request<Texture2D>($"Images/Item_{item.type}", AssetRequestMode.ImmediateLoad).Value;  //TextureAssets.Item[itemID].Value;
            Rectangle frame = ((Main.itemAnimations[itemID] == null) ? value.Frame() : Main.itemAnimations[itemID].GetFrame(value));
            Vector2 origin = frame.Size() / 2f;
            Vector2 screenPositionForCenter = GetDimensions().Center();
            // calculate the scale of the item
            float sizeLimit = 32f;
            float itemScale = item.scale;
            float scale1 = 1f;
            float scale2 = 1f;
            if ((float)frame.Width > sizeLimit || (float)frame.Height > sizeLimit)
            {
                scale2 = ((frame.Width <= frame.Height) ? (sizeLimit / (float)frame.Height) : (sizeLimit / (float)frame.Width));
            }
            float finalDrawScale = itemScale * scale1 * scale2;
            SpriteEffects effects = SpriteEffects.None;
            spriteBatch.Draw(value, screenPositionForCenter, frame, GetDrawColor(), 0f, origin, finalDrawScale, effects, 0f);

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

        public override int CompareTo(object obj)
        {
            if (obj is BuffItemUIElement) return sortOrder.Compare(this, (BuffItemUIElement)obj);
            return base.CompareTo(obj);
        }
    }
}
