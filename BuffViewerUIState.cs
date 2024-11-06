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
using PermanentBuffViewer.UI.Interface;
using System.Xml.Linq;

namespace PermanentBuffViewer
{
    internal class BuffViewerUIState : UIState
    {
        public static BuffViewerModSystem ModSystem { get; private set; }
        public List<DiffLockedUITest> updateOnWorldEnter;

        public TestPanels testPanels;


        public override void OnInitialize()
        {
            ContentInstance.Register(this);
            updateOnWorldEnter = new List<DiffLockedUITest>();
            testPanels = new TestPanels();
            //CreateAllTestPanels();
            
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var panel in testPanels.GetElementsToAdd()) this.AddOrRemoveChild(panel, Main.playerInventory);
            // Remove the old panel if there was one.
            UIPanel oldPanel = testPanels.GetPrevPanel();
            if (oldPanel != null)
            {
                this.RemoveChild(oldPanel);
                testPanels.AdjustButtonLocations();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Checks the UI element to see if it's a Difficulty Locked one and if so, register it to be added/removed on world entry.
        /// </summary>
        /// <param name="element">The UI element to be checked and potentially added.</param>
        /// <param name="parent">The parent of the UI element that will be added/removed from.</param>
        public void TryRegisterUIElementForWorldUpdate(BuffItemUIElement element, UIElement parent)
        {
            DifficultyLockedItemUIIcon icon = element as DifficultyLockedItemUIIcon;
            if (icon != null) updateOnWorldEnter.Add(new DiffLockedUITest (icon.MinDifficultyAvailable, icon, parent));
        }

    }
}
