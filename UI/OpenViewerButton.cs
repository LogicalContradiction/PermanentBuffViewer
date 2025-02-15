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
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace PermanentBuffViewer.UI
{
    internal class OpenViewerButton : UIImageButton
    {
        // Used to remember the position of the panel relative to the mouse position
        private Vector2 offset;
        // Used to indicate the button is being dragged
        private bool dragging = false;
        // Used to indicate the button can be dragged
        private bool Draggable {  get; set; }

        // Use to determine if the texture drawn should be show or hide
        private bool showContent = false;

        // Texture used when the content is showing
        private Asset<Texture2D> showingContentTexture;
        private Asset<Texture2D> showingContentOutlineTexture;
        // Texture used when the content is hidden
        private Asset<Texture2D> hidingContentTexture;
        private Asset<Texture2D> hidingContentOutlineTexture;

        private LocalizedText showingContentText;
        private LocalizedText hidingContentText;

        private Color? borderColor;

        private BuffViewerConfig config;

        /// <summary>
        /// Constructor for the Draggable button
        /// </summary>
        /// <param name="showingContentTexture">The texture of the button when the content is showing.</param>
        /// <param name="showingContentOutlineTexture">The texture of the button outline when the content is showing.</param>
        /// <param name="hidingContentTexture">The texture of the button when the content is hidden.</param>
        /// <param name="hidingContentOutlineTexture">The texture of the button outline when the content is hidden.</param>
        /// <param name="showingContentTextKey">The localization key for text when content is showing.</param>
        /// <param name="hidingContentTextKey">The localization key for text when content is hiding.</param>
        public OpenViewerButton(Asset<Texture2D> showingContentTexture, Asset<Texture2D>  showingContentOutlineTexture,
            Asset<Texture2D> hidingContentTexture, Asset<Texture2D> hidingContentOutlineTexture,
            string showingContentTextKey, string hidingContentTextKey) : base(hidingContentTexture)
        {      
            // get Draggable from the config file, default to false
            this.showingContentTexture = showingContentTexture;
            this.showingContentOutlineTexture = showingContentOutlineTexture;
            this.hidingContentTexture = hidingContentTexture;
            this.hidingContentOutlineTexture = hidingContentOutlineTexture;
            this.showingContentText = Language.GetOrRegister(showingContentTextKey);
            this.hidingContentText = Language.GetOrRegister(hidingContentTextKey);
            this.config = ModContent.GetInstance<BuffViewerConfig>();
        }

        public override void RightMouseDown(UIMouseEvent evt)
        {
            base.RightMouseDown(evt);
            if (!config.LockOpenButtonPos) DragStart(evt);
        }


        public override void RightMouseUp(UIMouseEvent evt)
        {
            base.RightMouseUp(evt);
            if (!config.LockOpenButtonPos) DragEnd(evt);
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

            borderColor = null;

            // Disables mouseclicks on this button from activating the player's items
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            // Moves the button as it's dragged
            if (dragging)
            {
                // Move the button to where the mouse is, offset included
                // Makes the button stay in the same place relative to the mouse (mostly)
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
            // Creates hovertext
            if (this.IsMouseHovering)
            {
                borderColor = Color.Gold;
                if (showContent) Main.instance.MouseText(showingContentText.Value);
                else Main.instance.MouseText(hidingContentText.Value);
            }
        }

        public void SwapTexture()
        {
            showContent = !showContent;
            if (showContent) base.SetImage(showingContentTexture);
            else base.SetImage(hidingContentTexture);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            Rectangle innerRectangle = this.GetInnerDimensions().ToRectangle();

            if (this.IsMouseHovering)
            {
                if (showContent) spriteBatch.Draw(showingContentOutlineTexture.Value, innerRectangle, borderColor.Value);
                else spriteBatch.Draw(hidingContentOutlineTexture.Value, innerRectangle, borderColor.Value);
            }
        }


    }
}
