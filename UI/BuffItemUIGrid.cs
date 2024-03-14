using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace PermanentBuffViewer.UI
{
    /// <summary>
    /// Extension of UIGrid with custom logic for adding difficulty-locked UI elements.
    /// </summary>
    internal class BuffItemUIGrid : UIGrid
    {
        internal List<UIElement> worldCheckElements;

        public BuffItemUIGrid() 
        {
            worldCheckElements = new List<UIElement>();
        }

        /// <summary>
        /// Update all the elements in the grid that needs to be updated on entering a world.
        /// </summary>
        public void UpdateGridUIElementsOnWorldEnter()
        {
            // check the registered elements and see if they need to be added/removed
            foreach (var element in worldCheckElements)
            {
                if (element is DifficultyLockedItemUIIcon)
                {
                    DifficultyLockedItemUIIcon buffElement = (DifficultyLockedItemUIIcon)element;
                    // Add elements that aren't in but should be
                    if (buffElement.ShouldBeAddedToRendering() && !buffElement.IsInUI()) Add(buffElement);
                    // Remove elements that are in but shouldn't be
                    else if (!buffElement.ShouldBeAddedToRendering() && buffElement.IsInUI()) Remove(buffElement);

                }
            }
        }

        /// <summary>
        /// Register an element to be checked when a player joins a world.
        /// </summary>
        /// <param name="element">The UIElement to register.</param>
        public void RegisterWorldCheckElement(UIElement element)
        {
            worldCheckElements.Add(element);
        }
    }
}
