﻿using PermanentBuffViewer.UI;
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
using ReLogic.Content;

namespace PermanentBuffViewer
{
    internal class BuffViewerUIState : UIState
    {
        public static BuffViewerModSystem ModSystem { get; private set; }
        public List<DiffLockedUITest> updateOnWorldEnter;

        public DraggableUIButton openButton;

        public TestPanels testPanels;

        public UIPanel buffPanel;

        private bool showPanels = false;

        // These variables hold the alignment information
        float leftColAlignPixels = 0f;          // The right pixel location of the left column (text labels)
        float rightColAlignPixels = 100f;       // The left pixel location of the right column (UISingleRow)
        float rowTopPixels = 32f;               // The pixel location of the top of the next UIText
        float rowSpriteOffsetFromText = -8f;    // Offset from top of text to top of SingleRow (to center sprites)
        float rowTopSpace = 40f;                // Space between the top of the text for each new row


        public override void OnInitialize()
        {
            ContentInstance.Register(this);
            updateOnWorldEnter = new List<DiffLockedUITest>();
            // For testing the drawing of everything
            //testPanels = new TestPanels();

            openButton = new DraggableUIButton(
                Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay",
                mode: AssetRequestMode.ImmediateLoad));
            openButton.OnLeftClick += ButtonOnClickHandler;
            openButton.Top.Set(900f, 0f);
            openButton.Left.Set(900f, 0f);
            Append(openButton);

            // Make the panel
            buffPanel = new UIPanel();
            buffPanel.HAlign = 0.5f;
            buffPanel.VAlign = 0.5f;
            buffPanel.Width.Set(300f, 0f);
            buffPanel.Height.Set(300f, 0f);

            // Panel title
            UIText panelTitleText = new UIText("Permanent Buffs");
            panelTitleText.Top.Set(0f, 0f);
            panelTitleText.HAlign = 0.5f;
            buffPanel.Append(panelTitleText);

            // First row, Health-related buffs
            UIText healthRowText = new UIText("Health:");
            UISingleRow healthRow = new UISingleRow();
            foreach (var healthElement in BuffItemUIElement.CreateElementsByID(
                ItemID.LifeCrystal, ItemID.LifeFruit, ItemID.AegisCrystal)) healthRow.Add(healthElement);
            SetupTextAndRow(row: healthRow, text: healthRowText);
            buffPanel.Append(healthRowText);
            buffPanel.Append(healthRow);

            // Second, Mana-related buffs
            UIText manaRowText = new UIText("Mana:");
            UISingleRow manaRow = new UISingleRow();
            foreach (var manaElement in BuffItemUIElement.CreateElementsByID(
                ItemID.ManaCrystal, ItemID.ArcaneCrystal)) manaRow.Add(manaElement);
            SetupTextAndRow(row: manaRow, text: manaRowText);
            buffPanel.Append(manaRowText);
            buffPanel.Append(manaRow);

            // Third, player stats
            UIText statRowText = new UIText("Stats:");
            UISingleRow statRow = new UISingleRow();
            foreach (var statElement in BuffItemUIElement.CreateElementsByID(
                ItemID.GummyWorm, ItemID.Ambrosia, ItemID.GalaxyPearl, ItemID.AegisFruit)) statRow.Add(statElement);
            SetupTextAndRow(row:  statRow, text: statRowText);
            buffPanel.Append(statRowText);
            buffPanel.Append(statRow);

            // 4th, miscellaneous buffs
            UIText miscRowText = new UIText("Misc:");
            UISingleRow miscRow = new UISingleRow();
            foreach (var miscElement in BuffItemUIElement.CreateElementsByID(
                ItemID.ArtisanLoaf, ItemID.TorchGodsFavor, ItemID.DemonHeart, ItemID.MinecartPowerup)) miscRow.Add(miscElement);
            SetupTextAndRow(row: miscRow, text: miscRowText);
            buffPanel.Append(miscRowText);
            buffPanel.Append(miscRow);

            // 5th, world buffs
            UIText worldRowText = new UIText("World:");
            UISingleRow worldRow = new UISingleRow();
            foreach (var worldElement in BuffItemUIElement.CreateElementsByID(
                ItemID.CombatBook, ItemID.CombatBookVolumeTwo, ItemID.PeddlersSatchel)) worldRow.Add(worldElement);
            SetupTextAndRow(row: worldRow, text: worldRowText);
            buffPanel.Append(worldRowText);
            buffPanel.Append(worldRow);

        }

        /// <summary>
        /// Handler for the open button's OnClick method
        /// </summary>
        /// <param name="evt">The event fired when the button is clicked.</param>
        /// <param name="element">The UIElement that was clicked on.</param>
        private void ButtonOnClickHandler(UIMouseEvent evt, UIElement element)
        {
            showPanels = !showPanels;
            // Code for testPanels
            if (testPanels != null)
            {
                this.AddOrRemoveChild(testPanels, showPanels);
                // Hack to make sure testPanels isn't drawn over the button.
                RemoveChild(openButton);
                Append(openButton);
                return;
            }
            this.AddOrRemoveChild(buffPanel, showPanels);
            
        }

        private void SetupTextAndRow(UISingleRow row, UIText text)
        {
            SetupSingleRow(row);
            AlignSingleRow(row);
            AlignRowText(text);

            // update rowTopPixels so it's ready for the next row
            rowTopPixels += rowTopSpace;
        }

        /// <summary>
        /// Helper used to align the sprite row 
        /// </summary>
        /// <param name="row">The row that is being aligned</param>
        private void AlignSingleRow(UISingleRow row)
        {
            row.Left.Set(rightColAlignPixels, 0f);
            row.Top.Set(rowTopPixels + rowSpriteOffsetFromText, 0f);
        }

        /// <summary>
        /// Helper used to align the text of a row
        /// </summary>
        /// <param name="text">The text for the row</param>
        private void AlignRowText(UIText text)
        {
            text.Left.Set(leftColAlignPixels, 0f);
            text.Top.Set(rowTopPixels, 0f);
        }

        /// <summary>
        /// Used to set up the dimensions of the Single Row
        /// </summary>
        /// <param name="row">The row to set the dimensions of.</param>
        /// <param name="numSprites">The number of sprites the row will hold.</param>
        private void SetupSingleRow(UISingleRow row)
        {
            var rowHeight = 32f;
            var rowWidth = 32f * row.Count;

            row.Height.Set(rowHeight, 0f);
            row.Width.Set(rowWidth, 0f);
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
