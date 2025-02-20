using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace PermanentBuffViewer
{
    /// <summary>
    /// Preloads the textures for the open button
    /// </summary>
    internal class AssetsOpenButton : ILoadable
    {
        public static Asset<Texture2D> ShowingContent {  get; set; }
        public static Asset<Texture2D> ShowingContentOutline { get; set; }
        public static Asset<Texture2D> HidingContent { get; set; }
        public static Asset<Texture2D> HidingContentOutline { get; set; }


        void ILoadable.Load(Mod mod)
        {
            ShowingContent = ModContent.Request<Texture2D>("PermanentBuffViewer/Assets/button_content_showing");
            ShowingContentOutline = ModContent.Request<Texture2D>("PermanentBuffViewer/Assets/button_content_showing_outline");
            HidingContent = ModContent.Request<Texture2D>("PermanentBuffViewer/Assets/button_content_hiding", mode: AssetRequestMode.ImmediateLoad);
            HidingContentOutline = ModContent.Request<Texture2D>("PermanentBuffViewer/Assets/button_content_hiding_outline");
        }

        void ILoadable.Unload() { }


    }
}
