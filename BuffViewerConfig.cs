using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.Config;

namespace PermanentBuffViewer
{
    internal class BuffViewerConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ClientSide;

        // Vector2(Left[X], Top[Y])
        [DefaultValue(typeof(Vector2), "1523, 790")]
        [Range(0, 1920f)]
        public Vector2 openButtonPos;

        public float openButtonX { get; set; } = 1523f;
        private float openButtonY { get; set; } = 790f;



        /*[OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            a = new Vector2(Main.screenWidth, Main.screenHeight);
            Console.WriteLine($"OnDeserial:\n screenHeight: {Main.screenHeight}\n screenWidth: {Main.screenWidth}");
        }*/

    }
}
