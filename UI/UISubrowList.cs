using Microsoft.CodeAnalysis.Operations;
using PermanentBuffViewer.UI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PermanentBuffViewer.UI
{
    internal class UISubrowList : UIElement, IAdjustableSize,
        IExpectedSize, IUpdateElementsOnWorldEntry
    {

        public List<UISubrow> items = new List<UISubrow>();

        public float listPadding = 5f;

        public int Count => items.Count;

        public float ExpectedHeight 
        {
            get
            {
                float height = 0f;
                foreach (UISubrow item in items)
                    height += item.ExpectedHeight + listPadding;
                if (height - listPadding > 0f) return height - listPadding;
                return height;
            }
        }

        public float ExpectedWidth
        {
            get
            {
                float width = 0f;
                foreach (UISubrow item in items)
                    width = Math.Max(width, item.ExpectedWidth);
                return width;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numSubrows">The number of subrows to create this list with.<br/>
        /// Must be one or greater.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if numSubrows is less than 1.</exception>
        public UISubrowList(int numSubrows = 1) 
        { 
            if (numSubrows < 1) throw new ArgumentOutOfRangeException(nameof(numSubrows),
                $"Must have at least 1 subrow. Tried to create with {numSubrows} rows.");
            for (int i = 0; i < numSubrows; i++)
            {
                var subrow = new UISubrow();
                SetSubrowDefaults(subrow);
                this.AddSubrow(subrow);
            }
        }

        /// <summary>
        /// Gets the subrow from a given index.
        /// </summary>
        /// <param name="index">The index of subrow to get.</param>
        /// <returns>The subrow for the given index.</returns>
        public UISubrow GetSubrow(int index)
        {
            return items[index];
        }

        /// <summary>
        /// Sets the default height and width of a subrow.<br/>
        /// Height = subrow.defaultHeight<br/>
        /// Width = StyleDimension.Fill
        /// </summary>
        /// <param name="subrow">The subrow to set the defaults of.</param>
        public void SetSubrowDefaults(UISubrow subrow)
        {
            subrow.Height.Set(subrow.defaultHeight, 0f);
            subrow.Width = StyleDimension.Fill;
        }

        /// <summary>
        /// Adds a subrow to the list. Returns the index of the new subrow.
        /// </summary>
        /// <param name="subrow">The subrow to add to the list.</param>
        /// <returns>The index of the new subrow.</returns>
        public int AddSubrow(UISubrow subrow) 
        { 
            items.Add(subrow);
            base.Append(subrow);
            Recalculate();
            return Count - 1;
        }

        /// <summary>
        /// Creates a new, empty subrow and adds it to the list.
        /// </summary>
        /// <returns>The index of the newly created subrow.</returns>
        public int CreateNewSubrow()
        {
            UISubrow subrow = new UISubrow();
            SetSubrowDefaults(subrow);
            Recalculate();
            return AddSubrow(subrow);
        }

        /// <summary>
        /// Adds a BuffItemUIElement to a subrow chosen by index.
        /// </summary>
        /// <param name="subrowIndex">The index of the subrow to add element to.</param>
        /// <param name="element">The BuffItemUIElement to add to the subrow.</param>
        public void AddElementToSubrow(int subrowIndex, BuffItemUIElement element)
        {
            GetSubrow(subrowIndex).Add(element);
            Recalculate();
        }

        /// <summary>
        /// Adds all BuffItemUIElements to the subrow indicated by index.
        /// </summary>
        /// <param name="subrowIndex">The index of the subrow to add elements to.</param>
        /// <param name="elements">The elements that will be added to the subrow.</param>
        public void AddAllElementsToSubrow(int subrowIndex, IEnumerable<BuffItemUIElement> elements)
        {
            UISubrow subrow = GetSubrow(subrowIndex);
            foreach (BuffItemUIElement element in elements) subrow.Add(element);
            Recalculate();
        }

        /// <summary>
        /// Removes the first instance of the chosen subrow from the list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed, false if it wasn't removed or could not be found.</returns>
        public bool Remove(UISubrow item) 
        {
            base.RemoveChild(item);
            return items.Remove(item);
        }

        /// <summary>
        /// Removes all children from the list.
        /// </summary>
        public void Clear()
        {
            base.RemoveAllChildren();
            items.Clear();
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();
            float top = 0f;
            foreach (UIElement item in items)
            {
                item.Top.Set(top, 0f);
                top += item.GetOuterDimensions().Height + listPadding; 
            }
            AdjustSize();
        }

        public void AdjustSize()
        {
            foreach (UISubrow item in items) item.AdjustSize();
            if (Width.Pixels < ExpectedWidth) Width.Set(ExpectedWidth, 0f);
            if (Height.Pixels <  ExpectedHeight) Height.Set(ExpectedHeight, 0f);
        }

        public void UpdateElementsOnWorldEntry()
        {
            foreach (UISubrow item in items)
            {
                item.UpdateElementsOnWorldEntry();
            }
        }
    }
}
