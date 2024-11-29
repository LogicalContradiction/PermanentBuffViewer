using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace PermanentBuffViewer.UI
{
    internal class UISingleRow : UIElement
    {

        public List<BuffItemUIElement> items = new List<BuffItemUIElement>();

        public int Count => items.Count;

        public float InternalSpacer { get; set; } = 5f;

        public UISingleRow() 
        {


        }

        /// <summary>
        /// Adds an element to this row. Sorts after adding.
        /// </summary>
        /// <param name="item">The UI item to be added to this row.</param>
        public void Add(BuffItemUIElement item)
        {
            items.Add(item);
            UpdateOrder();
            Append(item);
        }

        /// <summary>
        /// Removes an element from this row.
        /// </summary>
        /// <param name="item">The item to be removed from this row.</param>
        /// <returns>True if the element was successfully removed.<br></br>
        /// False if not removed or not found.</returns>
        public bool Remove(BuffItemUIElement item)
        {
            RemoveChild(item);
            return items.Remove(item);
        }

        /// <summary>
        /// Updates the order elements are drawn in by sorting them.
        /// </summary>
        public void UpdateOrder()
        {
            items.Sort(new ItemIconComparer());
        }

        /// <summary>
        /// Removes all elements from this row.
        /// </summary>
        public void Clear()
        {
            RemoveAllChildren();
            items.Clear();
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();

            var left = 0f;
            foreach (var item in items)
            {
                DifficultyLockedItemUIIcon diffItem = item as DifficultyLockedItemUIIcon;
                if (diffItem != null) // This is a diff locked item, so determine if it goes in the rendering
                {
                    this.AddOrRemoveChild(item, diffItem.ShouldBeAddedToRendering());
                    if (!diffItem.ShouldBeAddedToRendering()) continue;
                }
                item.Left.Set(left, 0f);
                CalculatedStyle outerDimensions = item.GetOuterDimensions();
                left += outerDimensions.Width + InternalSpacer;
            }
        }



    }
}
