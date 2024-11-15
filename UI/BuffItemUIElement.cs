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
        internal Item Item { get; }

        internal Condition usedItem;

        internal LocalizedText itemUsedHoverText;
        internal LocalizedText itemNotUsedHoverText;
        internal LocalizedText howToObtainText;
        internal LocalizedText statModified;

        internal static ItemIconComparer sortOrder;

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
            this.Item = item;
            this.usedItem = usedItem;
            itemUsedHoverText = Language.GetOrRegister(itemUsedHoverTextKey);
            itemNotUsedHoverText = Language.GetOrRegister(itemNotUsedHoverTextKey);
            howToObtainText = Language.GetOrRegister(howToObtainKey);
            this.statModified = Language.GetOrRegister(statModifiedKey);
            base.Width.Set(32f, 0f);
            base.Height.Set(32f, 0f);
            if (sortOrder == null) sortOrder = new ItemIconComparer();
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
            int itemID = Item.type;
            Texture2D value = Main.Assets.Request<Texture2D>($"Images/Item_{Item.type}").Value;
            Rectangle frame = ((Main.itemAnimations[itemID] == null) ? value.Frame() : Main.itemAnimations[itemID].GetFrame(value));
            Vector2 origin = frame.Size() / 2f;
            Vector2 screenPositionForCenter = GetDimensions().Center();
            // calculate the scale of the item
            float sizeLimit = 32f;
            float itemScale = Item.scale;
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
        public bool IsInUI()
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

        // May not need, just use comparer
        public override int CompareTo(object obj)
        {
            if (obj is BuffItemUIElement) return sortOrder.Compare(this, (BuffItemUIElement)obj);
            return base.CompareTo(obj);
        }

        /// <summary>
        /// Creates a single BuffItemUIElement (or derived class) for the given itemID.<br/>
        /// If called with an item that isn't one of the 16 vanilla buff items, returns an icon for a dirt block.
        /// </summary>
        /// <param name="itemID">The ItemID of the item to make an icon for.</param>
        /// <returns>A BuffItemUIElement (or derived class) to be used in the UI representing the buff item, or an icon of a dirt block if called with an invalid ID.</returns>
        public static BuffItemUIElement CreateSingleElement(int itemID)
        {
            switch (itemID)
            {
                case ItemID.LifeCrystal:
                    return new MultiUseBuffItemUIIcon(
                        item: new Item(ItemID.LifeCrystal), usedItem: BuffViewerCondition.UsedLifeCrystal, maxNumCanUse: 15,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.LifeCrystal",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.LifeCrystal");

                case ItemID.LifeFruit:
                    return new MultiUseBuffItemUIIcon(
                        item: new Item(ItemID.LifeFruit), usedItem: BuffViewerCondition.UsedLifeFruit, maxNumCanUse: 20,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.LifeFruit",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.LifeFruit");

                case ItemID.AegisCrystal:
                    return new BuffItemUIIcon(
                       item: new Item(ItemID.AegisCrystal), usedItem: BuffViewerCondition.UsedAegisCrystal,
                       itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                       itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                       howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.AegisCrystal",
                       statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.AegisCrystal");

                case ItemID.ManaCrystal:
                    return new MultiUseBuffItemUIIcon(
                        item: new Item(ItemID.ManaCrystal), usedItem: BuffViewerCondition.UsedManaCrystal, maxNumCanUse: 9,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.ManaCrystal",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.ManaCrystal");

                case ItemID.ArcaneCrystal:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.ArcaneCrystal), usedItem: BuffViewerCondition.UsedArcaneCrystal,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.ArcaneCrystal",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.ArcaneCrystal");

                case ItemID.GummyWorm:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.GummyWorm), usedItem: BuffViewerCondition.UsedGummyWorm,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.GummyWorm",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.GummyWorm");

                case ItemID.Ambrosia:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.Ambrosia), usedItem: BuffViewerCondition.UsedAmbrosia,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.Ambrosia",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.Ambrosia");

                case ItemID.GalaxyPearl:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.GalaxyPearl), usedItem: BuffViewerCondition.UsedGalaxyPearl,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.GalaxyPearl",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.GalaxyPearl");

                case ItemID.AegisFruit:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.AegisFruit), usedItem: BuffViewerCondition.UsedAegisFruit,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.AegisFruit",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.AegisFruit");

                case ItemID.ArtisanLoaf:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.ArtisanLoaf), usedItem: BuffViewerCondition.UsedArtisanLoaf,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.ArtisanLoaf",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.ArtisanLoaf");

                case ItemID.TorchGodsFavor:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.TorchGodsFavor), usedItem: BuffViewerCondition.UsedTorchGod,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.TorchGodsFavor",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.TorchGodsFavor");

                case ItemID.DemonHeart:
                    return new DifficultyLockedItemUIIcon(
                        item: new Item(ItemID.DemonHeart), usedItem: BuffViewerCondition.UsedDemonHeart,
                        availableDifficulties: Condition.InExpertMode,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.DemonHeart",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.DemonHeart",
                        itemNotAvailableInCurrentDifficulty: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotAvailableDifficulty");

                case ItemID.MinecartPowerup:
                    return new DifficultyLockedItemUIIcon(
                        item: new Item(ItemID.MinecartPowerup), usedItem: BuffViewerCondition.UsedMinecartUpgrade,
                        availableDifficulties: Condition.InExpertMode,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.MinecartUpgrade",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.MinecartUpgrade",
                        itemNotAvailableInCurrentDifficulty: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotAvailableDifficulty");

                case ItemID.CombatBook:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.CombatBook), usedItem: BuffViewerCondition.UsedCombatBook,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.CombatBook",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.CombatBook");

                case ItemID.CombatBookVolumeTwo:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.CombatBookVolumeTwo), usedItem: BuffViewerCondition.UsedCombatBookVolumeTwo,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.CombatBookVolumeTwo",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.CombatBook");

                case ItemID.PeddlersSatchel:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.PeddlersSatchel), usedItem: BuffViewerCondition.UsedPeddlersSatchel,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.PeddlersSatchel",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.PeddlersSatchel");

                default:
                    return new BuffItemUIIcon(
                        item: new Item(ItemID.DirtBlock), usedItem: BuffViewerCondition.DebugAlwaysTrue,
                        itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.DebugUsed",
                        itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.DebugNotUsed",
                        howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.Debug",
                        statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.Debug");
            }
        }

        /// <summary>
        /// Creates multiple BuffItemUIElements of the same item.
        /// </summary>
        /// <param name="itemID">The ItemID of the item to make multiple ui elements of.</param>
        /// <param name="numElements">The number of ui elements to make.</param>
        /// <returns>An enumerable containing exactly numElements BuffItemUIElements for item itemID.</returns>
        public static IEnumerable<BuffItemUIElement> CreateMultipleElements(int itemID, int numElements)
        {
            if (numElements < 1) throw new ArgumentOutOfRangeException(nameof(numElements),
                $"CreateMultipleElements called for less than 1 element ({numElements} elements).");
            var result = new List<BuffItemUIElement>();
            for (int i = 0; i < numElements; i++) result.Add(CreateSingleElement(itemID));
            return result;
        }

        /// <summary>
        /// Creates one of each BuffItemUIElement for each of the 16 vanilla buff items.<br/>
        /// Access each element using its ItemID.
        /// </summary>
        /// <returns>A dictionary containing one BuffItemUIElement for each of the 16 vanilla buff items.</returns>
        public static Dictionary<int, BuffItemUIElement> CreateVanillaBuffItemIcons()
        {
            var result = new Dictionary<int, BuffItemUIElement>();
            var keys = new List<int>()
            {
                ItemID.LifeCrystal,
                ItemID.LifeFruit,
                ItemID.AegisCrystal,
                ItemID.ManaCrystal,
                ItemID.ArcaneCrystal,
                ItemID.GummyWorm,
                ItemID.Ambrosia,
                ItemID.GalaxyPearl,
                ItemID.AegisFruit,
                ItemID.ArtisanLoaf,
                ItemID.TorchGodsFavor,
                ItemID.DemonHeart,
                ItemID.MinecartPowerup,
                ItemID.CombatBook,
                ItemID.CombatBookVolumeTwo,
                ItemID.PeddlersSatchel,
            };
            foreach (int key in keys) result.Add(key, CreateSingleElement(key));
            return result;
        }

        public override string ToString()
        {
            return $"Buff Element Item: {Item.Name}, Condition: {usedItem}, itemUsedText: \"{itemUsedHoverText}\", " +
                $"itemNotUsedText: \"{itemNotUsedHoverText}\", howToObtainText: \"{howToObtainText}\", statModifiedText: \"{statModified}\"";
        }

    }
}
