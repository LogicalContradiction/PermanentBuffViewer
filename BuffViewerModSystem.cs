using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace PermanentBuffViewer
{
    /// <summary>
    /// Creates and inserts the Buff Viewer user interface layer.
    /// </summary>
    [Autoload(Side = ModSide.Client)]
    internal class BuffViewerModSystem : ModSystem
    {
        internal BuffViewerUIState buffViewerUIState;
        private UserInterface userInterface;
        
        public override void Load()
        {
            buffViewerUIState = new BuffViewerUIState();
            buffViewerUIState.Activate();
            userInterface = new UserInterface();
            userInterface.SetState(buffViewerUIState);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            userInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "PermanentBuffViewer: Show permanent buffs",
                    delegate
                    {
                        userInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

    }
}
