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
        public List<IUpdateElementsOnWorldEntry> updateOnWorldEnter;

        public TestPanels testPanels;


        public override void OnInitialize()
        {
            ContentInstance.Register(this);
            updateOnWorldEnter = new List<IUpdateElementsOnWorldEntry>();
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
        /// Registers the element so it will be evaluated when the player enters a world to determine if elements need to be removed or added to it according to the availability of items.
        /// </summary>
        /// <param name="element">The element to register.</param>
        public void RegisterUIElementForWorldUpdate(IUpdateElementsOnWorldEntry element)
        {
            updateOnWorldEnter.Add(element);
        }

        /// <summary>
        /// To be called when a player enters a world. Has registered elements determine if children need to be added/removed based on the world.
        /// </summary>
        public void UpdateUIElementsOnWorldEnter()
        {
            foreach (var element in updateOnWorldEnter)
            {
                /*if (element is BuffItemUIGrid)
                {
                    ((BuffItemUIGrid)element).UpdateGridUIElementsOnWorldEnter();
                }*/
                //if (element is IUpdateElementsOnWorldEntry row) row.UpdateElementsOnWorldEntry();
                element.UpdateElementsOnWorldEntry();
            }
        }

    }
}
