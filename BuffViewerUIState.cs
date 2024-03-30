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

        /// <summary>
        /// Creates BuffItemUIElements for the 16 vanilla terraria buff items.<br/>
        /// The key for an element is a string of the name of the variable associated with that item.<br/>
        /// ex) "lifeCrystal" = Life Crystal icon, "aegisCrystal" = Vital Crystal icon.
        /// </summary>
        /// <returns>A dictionary containing all 16 BuffItemUIElements.</returns>
        public Dictionary<string, BuffItemUIElement> CreateBuffItemIcons()
        {
            Dictionary<string, BuffItemUIElement> elements = new Dictionary<string, BuffItemUIElement>();

            MultiUseBuffItemUIIcon lifeCrystal = new MultiUseBuffItemUIIcon(
                item: new Item(ItemID.LifeCrystal), usedItem: BuffViewerCondition.UsedLifeCrystal, maxNumCanUse: 15,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.LifeCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.LifeCrystal");
            elements.Add("lifeCrystal", lifeCrystal);

            MultiUseBuffItemUIIcon lifeFruit = new MultiUseBuffItemUIIcon(
                item: new Item(ItemID.LifeFruit), usedItem: BuffViewerCondition.UsedLifeFruit, maxNumCanUse: 20,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.LifeFruit",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.LifeFruit");
            elements.Add("lifeFruit", lifeFruit);

            BuffItemUIIcon aegisCrystal = new BuffItemUIIcon(
                item: new Item(ItemID.AegisCrystal), usedItem: BuffViewerCondition.UsedAegisCrystal,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.AegisCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.AegisCrystal");
            elements.Add("aegisCrystal", aegisCrystal);

            MultiUseBuffItemUIIcon manaCrystal = new MultiUseBuffItemUIIcon(
                item: new Item(ItemID.ManaCrystal), usedItem: BuffViewerCondition.UsedManaCrystal, maxNumCanUse: 9,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.MultiUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.ManaCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.ManaCrystal");
            elements.Add("manaCrystal", manaCrystal);

            BuffItemUIIcon arcaneCrystal = new BuffItemUIIcon( 
                item: new Item(ItemID.ArcaneCrystal), usedItem: BuffViewerCondition.UsedArcaneCrystal,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.ArcaneCrystal",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.ArcaneCrystal");
            elements.Add("arcaneCrystal", arcaneCrystal);

            BuffItemUIIcon gummyWorm = new BuffItemUIIcon( 
                item: new Item(ItemID.GummyWorm), usedItem: BuffViewerCondition.UsedGummyWorm,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.GummyWorm",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.GummyWorm");
            elements.Add("gummyWorm", gummyWorm);

            BuffItemUIIcon ambrosia = new BuffItemUIIcon(
                item: new Item(ItemID.Ambrosia), usedItem: BuffViewerCondition.UsedAmbrosia,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.Ambrosia",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.Ambrosia");
            elements.Add("ambrosia", ambrosia);

            BuffItemUIIcon galaxyPearl = new BuffItemUIIcon(
                item: new Item(ItemID.GalaxyPearl), usedItem: BuffViewerCondition.UsedGalaxyPearl,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.GalaxyPearl",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.GalaxyPearl");
            elements.Add("galaxyPearl", galaxyPearl);

            BuffItemUIIcon aegisFruit = new BuffItemUIIcon(
                item: new Item(ItemID.AegisFruit), usedItem: BuffViewerCondition.UsedAegisFruit,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.AegisFruit",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.AegisFruit");
            elements.Add("aegisFruit", aegisFruit);

            BuffItemUIIcon artisanLoaf = new BuffItemUIIcon(
                item: new Item(ItemID.ArtisanLoaf), usedItem: BuffViewerCondition.UsedArtisanLoaf,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.ArtisanLoaf",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.ArtisanLoaf");
            elements.Add("artisanLoaf", artisanLoaf);

            BuffItemUIIcon torchGod = new BuffItemUIIcon(
                item: new Item(ItemID.TorchGodsFavor), usedItem: BuffViewerCondition.UsedTorchGod,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.TorchGodsFavor",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.TorchGodsFavor");
            elements.Add("torchGod", torchGod);

            DifficultyLockedItemUIIcon demonHeart = new DifficultyLockedItemUIIcon(
                item: new Item(ItemID.DemonHeart), usedItem: BuffViewerCondition.UsedDemonHeart,
                availableDifficulties: Condition.InExpertMode, 
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.DemonHeart",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.DemonHeart",
                itemNotAvailableInCurrentDifficulty: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotAvailableDifficulty");
            elements.Add("demonHeart", demonHeart);

            DifficultyLockedItemUIIcon minecartUpgrade = new DifficultyLockedItemUIIcon(
                item: new Item(ItemID.MinecartPowerup), usedItem: BuffViewerCondition.UsedMinecartUpgrade,
                availableDifficulties: Condition.InExpertMode,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.MinecartUpgrade",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.MinecartUpgrade",
                itemNotAvailableInCurrentDifficulty: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotAvailableDifficulty");
            elements.Add("minecartUpgrade", minecartUpgrade);

            BuffItemUIIcon combatBook = new BuffItemUIIcon(
                item: new Item(ItemID.CombatBook), usedItem: BuffViewerCondition.UsedCombatBook,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.CombatBook",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.CombatBook");
            elements.Add("combatBook", combatBook);

            BuffItemUIIcon combatBookVolumeTwo = new BuffItemUIIcon( 
                item: new Item(ItemID.CombatBookVolumeTwo), usedItem: BuffViewerCondition.UsedCombatBookVolumeTwo,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.CombatBookVolumeTwo",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.CombatBook");
            elements.Add("combatBookVolumeTwo", combatBookVolumeTwo);

            BuffItemUIIcon peddlersSatchel = new BuffItemUIIcon(
                item: new Item(ItemID.PeddlersSatchel), usedItem: BuffViewerCondition.UsedPeddlersSatchel,
                itemUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.SingleUsed",
                itemNotUsedHoverTextKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.NotUsed",
                howToObtainKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.HowToObtain.PeddlersSatchel",
                statModifiedKey: "Mods.PermanentBuffViewer.UI.ItemIcon.HoverText.ModifiedStats.PeddlersSatchel");
            elements.Add("peddlersSatchel", peddlersSatchel);
            return elements;
        }

    }
}
