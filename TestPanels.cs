using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PermanentBuffViewer.UI;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace PermanentBuffViewer
{
    internal class TestPanels
    {
        internal List<UIPanel> testPanels = new List<UIPanel>();
        internal UIImageButton backButton;
        internal UIImageButton nextButton;

        public int CurrTestPanelIndex { get; private set; } = 0;
        internal int prevTestPanelIndex = 0;

        BuffViewerUIState buffViewerUIState;

        public TestPanels()
        {
            buffViewerUIState = ModContent.GetInstance<BuffViewerUIState>();

            nextButton = new UIImageButton(
                Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay",
                mode: AssetRequestMode.ImmediateLoad));

            
            backButton = new UIImageButton(
                ModContent.GetInstance<PermanentBuffViewer>().Assets.Request<Texture2D>(
                    "Assets/BackButton", 
                    mode: AssetRequestMode.ImmediateLoad));
            

            nextButton.OnLeftClick += IncrementCount;
            backButton.OnLeftClick += DecrementCount;

            CreateAllTestPanels();

            nextButton.VAlign = 0.5f;
            backButton.VAlign = 0.5f;

            // These starting positions are for a panel of size (300f, 300f)
            nextButton.Left.Set(1042.7273f, 0f);
            backButton.Left.Set(680.72723f, 0f);
        }

        /// <summary>
        /// Gets the current test panel to display as well as the next and back buttons.
        /// </summary>
        /// <returns>An enumerable containing the current test panel to show and the back and next buttons.</returns>
        public IEnumerable<UIElement> GetElementsToAdd()
        {
            List<UIElement> elements = new List<UIElement>();
            elements.Add(backButton);
            elements.Add(nextButton);
            elements.Add(testPanels[CurrTestPanelIndex]);
            return elements;
        }

#nullable enable
        /// <summary>
        /// Returns the previous panel if the current panel was changed.<br/>
        /// Also updates the prev panel index if the panel was changed.
        /// </summary>
        /// <returns>The previous panel if the panel was changed, else null</returns>
        public UIPanel? GetPrevPanel()
        {
            if (prevTestPanelIndex == CurrTestPanelIndex) return null;
            UIPanel prevPanel = testPanels[prevTestPanelIndex];
            prevTestPanelIndex = CurrTestPanelIndex;
            return prevPanel;
        }
#nullable disable

        /// <summary>
        /// Used as the OnLeftClick handler for the back button.
        /// </summary>
        /// <param name="evt">The event</param>
        /// <param name="listeningElement">The listening element</param>
        internal void DecrementCount(UIMouseEvent evt, UIElement listeningElement)
        {
            if (CurrTestPanelIndex - 1 < 0) CurrTestPanelIndex = testPanels.Count - 1;
            else CurrTestPanelIndex--;
        }

        /// <summary>
        /// Used as the OnLeftClick handler for the next button.
        /// </summary>
        /// <param name="evt">The event</param>
        /// <param name="listeningElement">The listenenr</param>
        internal void IncrementCount(UIMouseEvent evt, UIElement listeningElement)
        {
            CurrTestPanelIndex++;
            CurrTestPanelIndex %= testPanels.Count;
        }

        /// <summary>
        /// Called after the current test panel is placed to adjust the location of the next and back buttons.
        /// </summary>
        public void AdjustButtonLocations()
        {
            UIPanel currPanel = testPanels[CurrTestPanelIndex];
            Vector2 position = currPanel.GetDimensions().Position();
            float width = currPanel.GetDimensions().Width;

            backButton.Left.Set(position.X - backButton.GetOuterDimensions().Width - 20, 0f);
            nextButton.Left.Set(position.X + width + 20, 0f);
        }

        /// <summary>
        /// Creates all of the test panels and adds them to the collection of panels.
        /// </summary>
        public void CreateAllTestPanels()
        {
            UIPanel testItemSpritePanel = CreateTestItemSpritePanel();
            testItemSpritePanel.HAlign = 0.5f;
            testItemSpritePanel.VAlign = 0.5f;
            testPanels.Add(testItemSpritePanel);

            UIPanel testGridSortingPanel = CreateGridTestPanel();
            testGridSortingPanel.HAlign = 0.5f;
            testGridSortingPanel.VAlign = 0.5f;
            testPanels.Add(testGridSortingPanel);

            UIPanel testUISubrowResizePanel = CreateUISubrowTestPanel();
            testUISubrowResizePanel.HAlign = 0.5f;
            testUISubrowResizePanel.VAlign = 0.5f;
            testPanels.Add(testUISubrowResizePanel);

            UIPanel testUISubrowSortPanel = CreateUISubrowSortTestPanel();
            testUISubrowSortPanel.HAlign = 0.5f;
            testUISubrowSortPanel.VAlign = 0.5f;
            testPanels.Add(testUISubrowSortPanel);

            UIPanel testUISubrowListResizePanel = CreateSubrowListResizeTestPanel();
            testUISubrowListResizePanel.HAlign = 0.5f;
            testUISubrowListResizePanel.VAlign = 0.5f;
            testPanels.Add(testUISubrowListResizePanel);
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

            UIText headerText = new UIText("Item Sprite Test");
            headerText.HAlign = 0.5f;
            panel.Append(headerText);

            Dictionary<string, BuffItemUIElement> elements = GetBuffItemIcons();

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

            Dictionary<string, BuffItemUIElement> elements = GetBuffItemIcons();
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
        /// Creates a panel for testing the resizing of UISubrow
        /// </summary>
        /// <returns>A panel that can be used to test the resizing of UISubrow</returns>
        public UIPanel CreateUISubrowTestPanel()
        {
            var panel = new UIPanel();
            panel.Width = StyleDimension.FromPixels(300);
            panel.Height = StyleDimension.FromPixels(300);

            UIText headerLabel = new UIText("UISingleRow Test");
            headerLabel.HAlign = 0.5f;
            panel.Append(headerLabel);

            UISubrow row = new UISubrow();
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
        /// Creates a panel to test the sorting of UISubrow
        /// </summary>
        /// <returns>A panel to test the sorting of UISubrow</returns>
        public UIPanel CreateUISubrowSortTestPanel()
        {
            var panel = new UIPanel();
            panel.Width.Set(630f, 0f);
            panel.Height.Set(600f, 0f);

            var headerText = new UIText("Test Sorting UISingleRow");
            headerText.HAlign = 0.5f;
            panel.Append(headerText);

            var row = new UISubrow();
            row.Width.Set(590f, 0f);
            row.Height.Set(40f, 0f);
            row.Top.Set(25f, 0f);
            panel.Append(row);

            Dictionary<string, BuffItemUIElement> elements = GetBuffItemIcons();
            List<string> reverseKeys = elements.Keys.Reverse().ToList();

            foreach (var key in reverseKeys) row.Add(elements[key]);
            var orderAddedText = new UIText("Order Added:\n" + String.Join("\n", reverseKeys));
            orderAddedText.Top = StyleDimension.FromPixels(row.Top.Pixels + row.Height.Pixels + 10);
            panel.Append(orderAddedText);

            // register so item gets updated on world enter
            buffViewerUIState.RegisterUIElementForWorldUpdate(row);

            return panel;
        }

        /// <summary>
        /// Creates a panel to test the resizing of UISubrowList
        /// </summary>
        /// <returns>A panel to test the resizing of UISubrow</returns>
        public UIPanel CreateSubrowListResizeTestPanel()
        {
            var panel = new UIPanel();
            panel.Width.Set(300f, 0f);
            panel.Height.Set(700f, 0f);

            var headerText = new UIText("SubrowList resize test");
            headerText.HAlign = 0.5f;
            panel.Append(headerText);
            var subrowList = new UISubrowList();
            subrowList.Top.Set(30f, 0f);
            subrowList.Width.Set(80f, 0f);
            subrowList.Height.Set(120f, 0f);
            panel.Append(subrowList);

            UIText expectedHeightText = new UIText($"Expected height: {subrowList.ExpectedHeight}");
            expectedHeightText.Top.Set(500f, 0f);
            panel.Append(expectedHeightText);
            UIText expectedWidthText = new UIText($"Expected width: {subrowList.ExpectedWidth}");
            expectedWidthText.Top.Set(540f, 0f);
            panel.Append(expectedWidthText);
            UIText actualHeightText = new UIText($"Actual height: {subrowList.Height.Pixels}");
            actualHeightText.Top.Set(580f, 0f);
            panel.Append(actualHeightText);
            UIText actualWidthText = new UIText($"Actual width: {subrowList.Width.Pixels}");
            actualWidthText.Top.Set(620f, 0f);
            panel.Append(actualWidthText);

            var addElementButton = new UIImageButton(
                Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay",
                mode: AssetRequestMode.ImmediateLoad));
            addElementButton.Top.Set(400f, 0f);
            addElementButton.OnLeftClick += delegate
            {
                MultiUseBuffItemUIIcon lifeCrystal = new MultiUseBuffItemUIIcon(
                item: new Item(ItemID.LifeCrystal), usedItem: BuffViewerCondition.UsedLifeCrystal, maxNumCanUse: 15,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.LifeCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.LifeCrystal");
                int index = subrowList.Count - 1;
                subrowList.AddElementToSubrow(index, lifeCrystal);
                expectedHeightText.SetText($"Expected height: {subrowList.ExpectedHeight}");
                expectedWidthText.SetText($"Expected width: {subrowList.ExpectedWidth}");
                actualHeightText.SetText($"Actual height: {subrowList.Height.Pixels}");
                actualWidthText.SetText($"Actual width: {subrowList.Width.Pixels}");
            };
            panel.Append(addElementButton);

            var addRowButton = new UIImageButton(
                Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay",
                mode: AssetRequestMode.ImmediateLoad));
            addRowButton.Top.Set(440f, 0f);
            addRowButton.OnLeftClick += delegate
            {
                subrowList.CreateNewSubrow();
                expectedHeightText.SetText($"Expected height: {subrowList.ExpectedHeight}");
                expectedWidthText.SetText($"Expected width: {subrowList.ExpectedWidth}");
                actualHeightText.SetText($"Actual height: {subrowList.Height.Pixels}");
                actualWidthText.SetText($"Actual width: {subrowList.Width.Pixels}");
            };
            panel.Append(addRowButton);

            UIText addElementText = new UIText("Add element");
            addElementText.Top.Set(addElementButton.Top.Pixels, 0f);
            addElementText.Left.Set(40f, 0f);
            panel.Append(addElementText);

            UIText addRowText = new UIText("Add row");
            addRowText.Top.Set(addRowButton.Top.Pixels, 0f);
            addRowText.Left.Set(40f, 0f);
            panel.Append(addRowText);

            return panel;
        }


        /// <summary>
        /// Creates one BuffItemUIElement for each vanilla buff item.<br/>
        /// The key for a particular item is the internal name.
        /// </summary>
        /// <returns>A dictionary containing one BuffItemUIElement per vanilla buff.</returns>
        public Dictionary<string, BuffItemUIElement> GetBuffItemIcons()
        {
            return buffViewerUIState.CreateBuffItemIcons();
        }









    }
}
