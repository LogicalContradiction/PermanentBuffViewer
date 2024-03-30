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

        public UISubrow GetSubrow(int index)
        {
            return items[index];
        }

        public void SetSubrowDefaults(UISubrow subrow)
        {
            subrow.Height.Set(subrow.defaultHeight, 0f);
            subrow.Width = StyleDimension.Fill;
        }

        // returns the index of the new item
        public int AddSubrow(UISubrow item) 
        { 
            items.Add(item);
            base.Append(item);
            Recalculate();
            return Count - 1;
        }

        public int CreateNewSubrow()
        {
            UISubrow subrow = new UISubrow();
            SetSubrowDefaults(subrow);
            Recalculate();
            return AddSubrow(subrow);
        }

        public void AddElementToSubrow(int subrowIndex, BuffItemUIElement element)
        {
            GetSubrow(subrowIndex).Add(element);
            Recalculate();
        }

        public void AddAllElementsToSubrow(int subrowIndex, IEnumerable<BuffItemUIElement> elements)
        {
            UISubrow subrow = GetSubrow(subrowIndex);
            foreach (BuffItemUIElement element in elements) subrow.Add(element);
            Recalculate();
        }

        public bool Remove(UISubrow item) 
        {
            base.RemoveChild(item);
            return items.Remove(item);
        }

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
