using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace PermanentBuffViewer.UI
{
    internal class UISingleRow : UIElement
    {

        public List<UIElement> items = new List<UIElement>();
        
        public float ElementPadding = 5f;
        public float ExpectedWidth
        {
            get
            {
                float width = 0f;
                foreach (UIElement item in items)
                    width += item.GetOuterDimensions().Width + ElementPadding;
                if (width - ElementPadding >= 0f) return width - ElementPadding;
                else return 0f;
            }
        }
        public float ExpectedHeight 
        { 
            get
            {
                float height = 0f;
                foreach(UIElement item in items)
                    height = Math.Max(height, item.GetOuterDimensions().Height);
                return height;
            }
        
        }


        public UISingleRow()
        {

        }

        public void Add(UIElement item)
        {
            items.Add(item);
            Append(item);
            UpdateOrder();
            Recalculate();
        }

        public bool Remove(UIElement item)
        {
            RemoveChild(item);
            UpdateOrder();
            return items.Remove(item);
        }

        public void UpdateOrder()
        {
            items.Sort(DefaultSortMethod);
        }

        public int DefaultSortMethod(UIElement item1, UIElement item2)
        {
            return item1.CompareTo(item2);
        }

        public void Clear()
        {
            RemoveAllChildren();
            items.Clear();
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();
            
            var left = 0f;
            foreach (UIElement element in items)
            {
                element.Left.Set(left, 0f);
                CalculatedStyle outerDimensions = element.GetOuterDimensions();
                left += outerDimensions.Width + ElementPadding;
            }
            AdjustSize();
        }

        /// <summary>
        /// Sets the height and width of this element if it was set too small.
        /// </summary>
        public void AdjustSize()
        {
            if (Width.Pixels < ExpectedWidth) Width.Set(ExpectedWidth, 0f);
            if (Height.Pixels < ExpectedHeight) Height.Set(ExpectedHeight, 0f);
        }

    }
}
