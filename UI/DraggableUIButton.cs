using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PermanentBuffViewer.UI
{
    internal class DraggableUIButton : UIImageButton
    {
        // Used to remember the position of the panel relative to the mouse position
        private Vector2 offset;
        // Used to indicate the button is being dragged
        private bool dragging = false;
        // Used to indicate the button can be dragged
        private bool Draggable {  get; set; }
        
        public DraggableUIButton(Asset<Texture2D> texture) : base(texture)
        {      
            // get Draggable from the config file, default to false
        }

        public override void RightMouseDown(UIMouseEvent evt)
        {
            base.RightMouseDown(evt);
            DragStart(evt);
        }


        public override void RightMouseUp(UIMouseEvent evt)
        {
            base.RightMouseUp(evt);
            DragEnd(evt);
        }

        /// <summary>
        /// Helper for when user starts dragging this button.
        /// </summary>
        /// <param name="evt">The event fired when dragging starts.</param>
        private void DragStart(UIMouseEvent evt)
        {
            // Use offset to remember the location on the button the user stated dragging at
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }

        /// <summary>
        /// Helper for when the user finishes dragging this button.
        /// </summary>
        /// <param name="evt">The event fired when dragging ends.</param>
        private void DragEnd(UIMouseEvent evt)
        {
            Vector2 endMousePosition = evt.MousePosition;
            dragging = false;

            Left.Set(endMousePosition.X - offset.X, 0f);
            Top.Set(endMousePosition.Y - offset.Y, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Disables mouseclicks on this button from activating the player's items
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                // Move the button to where the mouse is, offset included
                // Makes the button stay in the same place relative to the mouse
                Left.Set(Main.mouseX - offset.X, 0f); 
                Top.Set(Main.mouseY - offset.Y, 0f);
            }

            // Makes sure the button stays completely inside the parent element
            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Contains(parentSpace))
            {
                // Math to prevent the button from leaving the parent
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
            }
        }


    }
}
