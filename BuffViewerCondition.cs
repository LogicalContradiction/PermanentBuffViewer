using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace PermanentBuffViewer
{
    public class BuffViewerCondition
    {
        public static Condition UsedLifeCrystal = new Condition("Mods.PermanentBuffViewer.Conditions.UsedLifeCrystal", () => Main.LocalPlayer.ConsumedLifeCrystals > 0);
        public static Condition UsedLifeFruit = new Condition("Mods.PermanentBuffViewer.Conditions.UsedLifeFruit", () => Main.LocalPlayer.ConsumedLifeFruit > 0);
        public static Condition UsedAegisCrystal = new Condition("Mods.PermanentBuffViewer.Conditions.UsedAegisCrystal", () => Main.LocalPlayer.usedAegisCrystal);
        public static Condition UsedManaCrystal = new Condition("Mods.PermanentBuffViewer.Conditions.UsedManaCrystal", () => Main.LocalPlayer.ConsumedManaCrystals > 0);
        public static Condition UsedArcaneCrystal = new Condition("Mods.PermanentBuffViewer.Conditions.UsedArcaneCrystal", () => Main.LocalPlayer.usedArcaneCrystal);
        public static Condition UsedGummyWorm = new Condition("Mods.PermanentBuffViewer.Conditions.UsedGummyWorm", () => Main.LocalPlayer.usedGummyWorm);
        public static Condition UsedAmbrosia = new Condition("Mods.PermanentBuffViewer.Conditions.UsedAmbrosia", () => Main.LocalPlayer.usedAmbrosia);
        public static Condition UsedGalaxyPearl = new Condition("Mods.PermanentBuffViewer.Conditions.UsedGalaxyPearl", () => Main.LocalPlayer.usedGalaxyPearl);
        public static Condition UsedAegisFruit = new Condition("Mods.PermanentBuffViewer.Conditions.UsedAegisFruit", () => Main.LocalPlayer.usedAegisFruit);
        public static Condition UsedArtisanLoaf = new Condition("Mods.PermanentBuffViewer.Conditions.UsedArtisanLoaf", () => Main.LocalPlayer.ateArtisanBread);
        public static Condition UsedTorchGod = new Condition("Mods.PermanentBuffViewer.Conditions.UsedTorchGod", () => Main.LocalPlayer.unlockedBiomeTorches);
        public static Condition UsedDemonHeart = new Condition("Mods.PermanentBuffViewer.Conditions.UsedDemonHeart", () => Main.LocalPlayer.extraAccessory);
        public static Condition UsedMinecartUpgrade = new Condition("Mods.PermanentBuffViewer.Conditions.UsedDemonHeart", () => Main.LocalPlayer.unlockedSuperCart);
        public static Condition UsedCombatBook = new Condition("Mods.PermanentBuffViewer.Conditions.UsedCombatBook", () => NPC.combatBookWasUsed);
        public static Condition UsedCombatBookVolumeTwo = new Condition("Mods.PermanentBuffViewer.Conditions.UsedCombatBookVolumeTwo", () => NPC.combatBookVolumeTwoWasUsed);
        public static Condition UsedPeddlersSatchel = new Condition("Mods.PermanentBuffViewer.Conditions.UsedPeddlersSatchel", () => NPC.peddlersSatchelWasUsed);


    }
}
