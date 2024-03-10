using PermanentBuffViewer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.UI;

namespace PermanentBuffViewer
{
    internal class BuffViewerUIState : UIState
    {
        public static BuffViewerModSystem ModSystem { get; private set; }
        public List<UIPanel> testPanels;


        public override void OnInitialize()
        {
            ContentInstance.Register(this);
            CreateAllTestPanels();
        }

        public override void Update(GameTime gameTime)
        {
            if (testPanels != null)
            {
                foreach (var panel in testPanels) this.AddOrRemoveChild(panel, Main.playerInventory);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Creates all test panels and adds them to the list for debugging.
        /// </summary>
        public void CreateAllTestPanels()
        {
            testPanels = new List<UIPanel>();
            UIPanel testItemSpritePanel = CreateTestItemSpritePanel();
            testPanels.Add(testItemSpritePanel);
        }

        /// <summary>
        /// Create a panel for testing each of the derived classes of BuffItemUIIcons
        /// </summary>
        /// <returns>A panel with each BuffItemUIIcons placed in it to be used to debug.</returns>
        public UIPanel CreateTestItemSpritePanel()
        {
            UIPanel panel = new UIPanel();
            panel.Width = StyleDimension.FromPixels(300);
            panel.Height = StyleDimension.FromPercent(0.5f);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;

            UIText headerText = new UIText("Item Sprite Test");
            headerText.HAlign = 0.5f;
            panel.Append(headerText);

            Dictionary<string, BuffItemUIElement> elements = CreateBuffItemIcons();

            // place each element in the panel
            int rowCount = 0;
            int columnCount = 0;
            BuffItemUIElement prevElement = null;
            foreach (var key in elements.Keys)
            {
                BuffItemUIElement element = elements[key];
                if (prevElement == null)
                {
                    // place first element
                    element.Left = StyleDimension.FromPixels(0);
                    element.Top = StyleDimension.FromPixels(30);
                    columnCount++;
                    panel.Append(element);
                    prevElement = element;
                    continue;
                }
                if (columnCount == 0) element.Left = StyleDimension.FromPixels(0);
                else element.Left = StyleDimension.FromPixels(prevElement.Left.Pixels + 40);
                if (rowCount == 0) element.Top = StyleDimension.FromPixels(30);
                else element.Top = StyleDimension.FromPixels((rowCount * 40) + 30);

                panel.Append(element);
                prevElement = element;
                // increment counters
                if (columnCount + 1 < 5) columnCount++;
                else
                {
                    columnCount = 0;
                    rowCount++;
                }
            }
            return panel;
        }

        public Dictionary<string, BuffItemUIElement> CreateBuffItemIcons()
        {
            Dictionary<string, BuffItemUIElement> elements = new Dictionary<string, BuffItemUIElement>();

            MultiUseBuffItemUIIcon lifeCrystal = new MultiUseBuffItemUIIcon(
                item: new Item(ItemID.LifeCrystal), usedItem: BuffViewerCondition.UsedLifeCrystal, maxNumCanUse: 15,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.LifeCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.LifeCrystal");
            elements.Add("lifeCrystal", lifeCrystal);

            MultiUseBuffItemUIIcon lifeFruit = new MultiUseBuffItemUIIcon(
                item: new Item(ItemID.LifeFruit), usedItem: BuffViewerCondition.UsedLifeFruit, maxNumCanUse: 20,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.LifeFruit",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.LifeFruit");
            elements.Add("lifeFruit", lifeFruit);

            BuffItemUIIcon aegisCrystal = new BuffItemUIIcon(
                item: new Item(ItemID.AegisCrystal), usedItem: BuffViewerCondition.UsedAegisCrystal,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.AegisCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.AegisCrystal");
            elements.Add("aegisCrystal", aegisCrystal);

            MultiUseBuffItemUIIcon manaCrystal = new MultiUseBuffItemUIIcon(
                item: new Item(ItemID.ManaCrystal), usedItem: BuffViewerCondition.UsedManaCrystal, maxNumCanUse: 9,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.ManaCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.ManaCrystal");
            elements.Add("manaCrystal", manaCrystal);

            BuffItemUIIcon arcaneCrystal = new BuffItemUIIcon( 
                item: new Item(ItemID.ArcaneCrystal), usedItem: BuffViewerCondition.UsedArcaneCrystal,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.ArcaneCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.ArcaneCrystal");
            elements.Add("arcaneCrystal", arcaneCrystal);

            BuffItemUIIcon gummyWorm = new BuffItemUIIcon( 
                item: new Item(ItemID.GummyWorm), usedItem: BuffViewerCondition.UsedGummyWorm,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.GummyWorm",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.GummyWorm");
            elements.Add("gummyWorm", gummyWorm);

            BuffItemUIIcon ambrosia = new BuffItemUIIcon(
                item: new Item(ItemID.Ambrosia), usedItem: BuffViewerCondition.UsedAmbrosia,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.Ambrosia",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.Ambrosia");
            elements.Add("ambrosia", ambrosia);

            BuffItemUIIcon galaxyPearl = new BuffItemUIIcon(
                item: new Item(ItemID.GalaxyPearl), usedItem: BuffViewerCondition.UsedGalaxyPearl,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.GalaxyPearl",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.GalaxyPearl");
            elements.Add("galaxyPearl", galaxyPearl);

            BuffItemUIIcon aegisFruit = new BuffItemUIIcon(
                item: new Item(ItemID.AegisFruit), usedItem: BuffViewerCondition.UsedAegisFruit,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.AegisFruit",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.AegisFruit");
            elements.Add("aegisFruit", aegisFruit);

            BuffItemUIIcon artisanLoaf = new BuffItemUIIcon(
                item: new Item(ItemID.ArtisanLoaf), usedItem: BuffViewerCondition.UsedArtisanLoaf,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.ArtisanLoaf",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.ArtisanLoaf");
            elements.Add("artisanLoaf", artisanLoaf);

            BuffItemUIIcon torchGod = new BuffItemUIIcon(
                item: new Item(ItemID.TorchGodsFavor), usedItem: BuffViewerCondition.UsedTorchGod,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.TorchGodsFavor",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.TorchGodsFavor");
            elements.Add("torchGod", torchGod);

            DifficultyLockedItemUIIcon demonHeart = new DifficultyLockedItemUIIcon(
                item: new Item(ItemID.DemonHeart), usedItem: BuffViewerCondition.UsedDemonHeart,
                availableDifficulties: Condition.InExpertMode, 
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.DemonHeart",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.DemonHeart",
                itemNotAvailableInCurrentDifficulty: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotAvailableDifficulty");
            elements.Add("demonHeart", demonHeart);

            DifficultyLockedItemUIIcon minecartUpgrade = new DifficultyLockedItemUIIcon(
                item: new Item(ItemID.MinecartPowerup), usedItem: BuffViewerCondition.UsedMinecartUpgrade,
                availableDifficulties: Condition.InExpertMode,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.MinecartUpgrade",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.MinecartUpgrade",
                itemNotAvailableInCurrentDifficulty: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotAvailableDifficulty");
            elements.Add("minecartUpgrade", minecartUpgrade);

            BuffItemUIIcon combatBook = new BuffItemUIIcon(
                item: new Item(ItemID.CombatBook), usedItem: BuffViewerCondition.UsedCombatBook,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.CombatBook",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.CombatBook");
            elements.Add("combatBook", combatBook);

            BuffItemUIIcon combatBookVolumeTwo = new BuffItemUIIcon( 
                item: new Item(ItemID.CombatBookVolumeTwo), usedItem: BuffViewerCondition.UsedCombatBookVolumeTwo,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.CombatBookVolumeTwo",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.CombatBook");
            elements.Add("combatBookVolumeTwo", combatBookVolumeTwo);

            BuffItemUIIcon peddlersSatchel = new BuffItemUIIcon(
                item: new Item(ItemID.PeddlersSatchel), usedItem: BuffViewerCondition.UsedPeddlersSatchel,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.PeddlersSatchel",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.PeddlersSatchel");
            elements.Add("peddlersSatchel", peddlersSatchel);
            return elements;
        }

    }
}
