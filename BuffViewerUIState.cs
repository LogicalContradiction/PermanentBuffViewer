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

        public DraggableUIButton openButton;

        public TestPanels testPanels;

        private bool showPanels = false;


        public override void OnInitialize()
        {
            ContentInstance.Register(this);
            updateOnWorldEnter = new List<DiffLockedUITest>();
            testPanels = new TestPanels();
            //Append(testPanels);

            openButton = new DraggableUIButton(
                Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay"));
            openButton.OnLeftClick += ButtonOnClickHandler;
            openButton.Top.Set(900f, 0f);
            openButton.Left.Set(900f, 0f);
            Append(openButton);
        }

        /// <summary>
        /// Handler for the open button's OnClick method
        /// </summary>
        /// <param name="evt">The event fired when the button is clicked.</param>
        /// <param name="element">The UIElement that was clicked on.</param>
        private void ButtonOnClickHandler(UIMouseEvent evt, UIElement element)
        {
            showPanels = !showPanels;
            this.AddOrRemoveChild(testPanels, showPanels);
            RemoveChild(openButton);
            Append(openButton);
        }

        public override void Update(GameTime gameTime)
        {
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
