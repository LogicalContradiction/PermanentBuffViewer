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
using Microsoft.Xna.Framework.Graphics;

namespace PermanentBuffViewer
{
    internal class BuffViewerUIState : UIState
    {
        public static BuffViewerModSystem ModSystem { get; private set; }
        public List<UIPanel> testPanels;
        public List<UIElement> updateOnWorldEnter;


        public override void OnInitialize()
        {
            ContentInstance.Register(this);
            updateOnWorldEnter = new List<UIElement>();
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

            // panel to test the item sprites
            UIPanel testItemSpritePanel = CreateTestItemSpritePanel();
            testItemSpritePanel.Left = StyleDimension.FromPixels(5);
            testItemSpritePanel.VAlign = 0.5f;
            //testPanels.Add(testItemSpritePanel);

            // panel to test grid and sorting of elements
            UIPanel testGridSortPanel = CreateGridTestPanel();
            testGridSortPanel.Left = StyleDimension.FromPixels(testItemSpritePanel.Left.Pixels + testItemSpritePanel.Width.Pixels + 15);
            testGridSortPanel.VAlign = 0.5f;
            //testPanels.Add(testGridSortPanel);

            // panel to test custom grid
            UIPanel testCustomGridPanel = CreateCustomGridTestPanel();
            testCustomGridPanel.Left = StyleDimension.FromPixels(testGridSortPanel.Left.Pixels + testGridSortPanel.Width.Pixels + 15);
            testCustomGridPanel.VAlign = 0.5f;
            //testPanels.Add(testCustomGridPanel);

            // panel to test resizing UISingleRow
            UIPanel testUISingleRowPanel = CreateUISingleRowTestPanel();
            testUISingleRowPanel.Left.Set(testItemSpritePanel.Left.Pixels + testItemSpritePanel.Width.Pixels + 15, 0f);
            testUISingleRowPanel.VAlign = 0.5f;
            //testPanels.Add(testUISingleRowPanel);

            // panel to test sorting of UISingleRow
            UIPanel testUISingleRowSort = CreateUISingleRowSortTestPanel();
            testUISingleRowSort.HAlign = 0.5f;
            testUISingleRowSort.VAlign = 0.5f;
            testPanels.Add(testUISingleRowSort);


        }

        /// <summary>
        /// Create a panel for testing each of the derived classes of BuffItemUIIcons
        /// </summary>
        /// <returns>A panel with each BuffItemUIIcons placed in it to be used to debug.</returns>
        public UIPanel CreateTestItemSpritePanel()
        {
            UIPanel panel = new UIPanel();
            panel.Width = StyleDimension.FromPixels(300);
            panel.Height = StyleDimension.FromPixels(300);
            
            //panel.HAlign = 0.5f;
            //panel.VAlign = 0.5f;

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

        /// <summary>
        /// Create a panel for testing the default UIGrid and the sorting of the BuffItemUIIcons.
        /// </summary>
        /// <returns>A panel with all the BuffItemUIIcons in a grid as well as noting the order they were added.</returns>
        public UIPanel CreateGridTestPanel()
        {
            var panel = new UIPanel();
            panel.Width = StyleDimension.FromPixels(300);
            panel.Height = StyleDimension.FromPixels(625);

            UIText headerLabel = new UIText("Grid and Sorting Test");
            headerLabel.HAlign = 0.5f;
            panel.Append(headerLabel);
            
            UIGrid grid = new UIGrid();
            grid.Width = StyleDimension.Fill;
            grid.Height = StyleDimension.FromPixels(105);
            grid.Top = StyleDimension.FromPixels(25);
            panel.Append(grid);

            Dictionary<string, BuffItemUIElement> elements = CreateBuffItemIcons();
            List<string> orderAdded = new List<string>();
            // add the elements in reverse order
            foreach (var key in elements.Keys.Reverse())
            {
                orderAdded.Add(key);
                grid.Add(elements[key]);
            }
            UIText orderAddedText = new UIText("Order Added:\n" + String.Join("\n", orderAdded));
            orderAddedText.Top = StyleDimension.FromPixels(grid.Top.Pixels + grid.Height.Pixels + 10);
            panel.Append(orderAddedText);

            return panel;
        }

        /// <summary>
        /// Create a panel for testing BuffItemUIGrid.
        /// </summary>
        /// <returns></returns>
        public UIPanel CreateCustomGridTestPanel()
        {
            var panel = new UIPanel();
            panel.Width = StyleDimension.FromPixels(300);
            panel.Height = StyleDimension.FromPixels(300);

            UIText headerLabel = new UIText("Custom Grid Test");
            headerLabel.HAlign = 0.5f;
            panel.Append(headerLabel);

            Dictionary<string, BuffItemUIElement> elements = CreateBuffItemIcons();
            BuffItemUIGrid grid = new BuffItemUIGrid();
            grid.Top = StyleDimension.FromPixels(25);
            grid.Width = StyleDimension.Fill;
            grid.Height = StyleDimension.Fill;
            panel.Append(grid);

            foreach (var key in elements.Keys)
            {
                grid.Add(elements[key]);
            }
            grid.RegisterWorldCheckElement(elements["demonHeart"]);
            grid.RegisterWorldCheckElement(elements["minecartUpgrade"]);
            // register this grid so it gets updated when the player enters the world
            updateOnWorldEnter.Add(grid);
            return panel;
        }

        /// <summary>
        /// Creates a panel for testing the resizing of UISingleRow
        /// </summary>
        /// <returns></returns>
        public UIPanel CreateUISingleRowTestPanel()
        {
            var panel = new UIPanel();
            panel.Width = StyleDimension.FromPixels(300);
            panel.Height = StyleDimension.FromPixels(300);

            UIText headerLabel = new UIText("UISingleRow Test");
            headerLabel.HAlign = 0.5f;
            panel.Append(headerLabel);
            
            UISingleRow row = new UISingleRow();
            row.Top.Set(25f, 0f);
            row.Width.Set(80f, 0f);
            row.Height.Set(40f, 0f);
            panel.Append(row);

            UIText rowExpectedWidthText = new UIText($"Row expected width: {row.ExpectedWidth}");
            rowExpectedWidthText.Top.Set(60f, 0f);
            panel.Append(rowExpectedWidthText);

            UIText rowExpectedHeightText = new UIText($"Row excpected height: {row.ExpectedHeight}");
            rowExpectedHeightText.Top.Set(90f, 0f);
            panel.Append(rowExpectedHeightText);

            UIText rowWidthText = new UIText($"Row width: {row.Width.Pixels}");
            rowWidthText.Top.Set(120f, 0f);
            panel.Append(rowWidthText);

            UIText rowHeightText = new UIText($"Row height: {row.Height.Pixels}");
            rowHeightText.Top.Set(150f, 0f);
            panel.Append(rowHeightText);

            UIImageButton button = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", mode: ReLogic.Content.AssetRequestMode.ImmediateLoad));
            button.Top.Set(180f, 0f);
            button.OnLeftClick += delegate
            {
                MultiUseBuffItemUIIcon lifeCrystal = new MultiUseBuffItemUIIcon(
                item: new Item(ItemID.LifeCrystal), usedItem: BuffViewerCondition.UsedLifeCrystal, maxNumCanUse: 15,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.LifeCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.LifeCrystal");
                row.Add(lifeCrystal);
                
                rowExpectedWidthText.SetText($"Row expected width: {row.ExpectedWidth}");
                rowExpectedHeightText.SetText($"Row excpected height: {row.ExpectedHeight}");
                rowWidthText.SetText($"Row width: {row.Width.Pixels}");
                rowHeightText.SetText($"Row height: {row.Height.Pixels}");
            };
            panel.Append(button);

            return panel;
        }

        /// <summary>
        /// Creates a panel to test the sorting of UISingleRow
        /// </summary>
        /// <returns></returns>
        public UIPanel CreateUISingleRowSortTestPanel()
        {
            var panel = new UIPanel();
            panel.Width.Set(630f, 0f);
            panel.Height.Set(600f, 0f);

            var headerText = new UIText("Test Sorting UISingleRow");
            headerText.HAlign = 0.5f;
            panel.Append(headerText);

            var row = new UISingleRow();
            row.Width.Set(590f, 0f);
            row.Height.Set(40f, 0f);
            row.Top.Set(25f, 0f);
            panel.Append(row);

            Dictionary<string, BuffItemUIElement> elements = CreateBuffItemIcons();
            List<string> reverseKeys = elements.Keys.Reverse().ToList();

            foreach (var key in reverseKeys) row.Add(elements[key]);
            var orderAddedText = new UIText("Order Added:\n" + String.Join("\n", reverseKeys));
            orderAddedText.Top = StyleDimension.FromPixels(row.Top.Pixels + row.Height.Pixels + 10);
            panel.Append(orderAddedText);

            // place so item gets updated on world enter
            updateOnWorldEnter.Add(row);

            return panel;
        }


        /// <summary>
        /// To be called when a player enters a world. Has registered elements determine if children need to be added/removed based on the world.
        /// </summary>
        public void UpdateUIElementsOnWorldEnter()
        {
            foreach (var element in updateOnWorldEnter)
            {
                if (element is BuffItemUIGrid)
                {
                    ((BuffItemUIGrid)element).UpdateGridUIElementsOnWorldEnter();
                }
                if (element is IUpdateElementsOnWorldEntry row) row.UpdateElementsOnWorldEntry();
            }
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
