using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace PermanentBuffViewer.UI
{
    internal class UISingleRow : UIElement
    {

        public List<BuffItemUIElement> items = new List<BuffItemUIElement>();



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
                item.Left.Set(left, 0f);
                CalculatedStyle outerDimensions = item.GetOuterDimensions();
                left += outerDimensions.Width;
            }
        }



    }
}
