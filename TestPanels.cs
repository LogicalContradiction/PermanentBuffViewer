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

            Dictionary<int, BuffItemUIElement> elements = 
                BuffItemUIElement.CreateVanillaBuffItemIcons();

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

        







    }
}
