using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermanentBuffViewer.UI.Interface;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PermanentBuffViewer.UI
{
    /// <summary>
    /// Holds a list of subrows. Used to evenly space out a list of rows.
    /// </summary>
    internal class UISubrowList_OLD : UIList, IUpdateElementsOnWorldEntry,
        IExpectedSize, IAdjustableSize
    {
        public int numElements => _items.Count;
        
        public float ExpectedHeight 
        {
            get
            {
                float height = 0f;
                foreach (var item in _items)
                    if (item is IExpectedSize eSizeItem) height += eSizeItem.ExpectedHeight;
                    else height += item.GetOuterDimensions().Height + this.ListPadding;
                if (height - this.ListPadding >= 0f) return height - this.ListPadding;
                return 0f;
            }   
        }

        public float ExpectedWidth
        {
            get
            {
                float width = 0f;
                foreach (var item in _items)
                    if (item is IExpectedSize eSizeItem) width = 
                            Math.Max(width, eSizeItem.ExpectedWidth);
                    else width = Math.Max(width, item.GetOuterDimensions().Width);
                return width;
            }
        }
        
        
        
        public UISubrowList_OLD(int numSubrows = 1)
        {
            if (numSubrows <= 0) throw new ArgumentOutOfRangeException(nameof(numSubrows), 
                $"Must have at least 1 subrow. Tried to create with {numSubrows}");

            for (int i = 0; i < numSubrows; i++)
            {
                var subrow = new UISubrow();
                SetSubrowDefaults(subrow);
                Add(subrow);
            }

        }

        /// <summary>
        /// Internal method to set the default parameters of a subrow.
        /// </summary>
        /// <param name="subrow">The subrow to set the default parameters of.</param>
        internal void SetSubrowDefaults(UISubrow subrow)
        {
            subrow.Height.Set(subrow.defaultHeight, 0f);
            subrow.Width = StyleDimension.Fill;
        }

        /// <summary>
        /// Gets the subrow at given index.
        /// </summary>
        /// <param name="index">The index of the subrow to get.</param>
        /// <returns></returns>
        public UISubrow GetSubrow(int index)
        {
            return (UISubrow)_items[index];
        }

        /// <summary>
        /// Adds the given subrow to this list.
        /// </summary>
        /// <param name="subrow">The subrow to add.</param>
        /// <returns>The index of the newly added subrow.</returns>
        public int AddSubrow(UISubrow subrow)
        {
            Add(subrow);
            return _items.Count - 1;
        }

        /// <summary>
        /// Creates a new, empty subrow and adds it to this list.
        /// </summary>
        /// <returns>The index of the newly added subrow.</returns>
        public int CreateNewSubrow()
        {
            var subrow = new UISubrow();
            SetSubrowDefaults(subrow);
            Add(subrow);
            return _items.Count - 1;
        }

        /// <summary>
        /// Adds a single item to a subrow indicated by the index.
        /// </summary>
        /// <param name="subrowIndex">The index of the subrow to add to.</param>
        /// <param name="item">The item to add to the chosen subrow.</param>
        public void AddToSubrow(int subrowIndex, BuffItemUIElement item)
        {
            GetSubrow(subrowIndex).Add(item);
        }

        /// <summary>
        /// Adds multiple items to the subrow indicated by index.
        /// </summary>
        /// <param name="subrowIndex">The index of the subrow to add these items to.</param>
        /// <param name="items">The items to add to the subrow.</param>
        public void AddAllToSubrow(int subrowIndex, IEnumerable<BuffItemUIElement> items)
        { 
            UISubrow subrow = GetSubrow(subrowIndex);
            foreach (BuffItemUIElement item in items) subrow.Add(item);
        }
        
        public void AdjustSize()
        {
            foreach (var item in _items)
            {
                if (item is IAdjustableSize adjustItem) adjustItem.AdjustSize(); 
            }
            if (Width.Pixels < ExpectedWidth) Width.Set(ExpectedWidth, 0f);
            if (Height.Pixels < ExpectedHeight) Height.Set(ExpectedHeight, 0f);
        }
        
        public void UpdateElementsOnWorldEntry()
        {
            foreach (var item in _items) 
            {
                if (item is IUpdateElementsOnWorldEntry updateItem) 
                    updateItem.UpdateElementsOnWorldEntry();
            }
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();
            AdjustSize();
        }
    }
}
