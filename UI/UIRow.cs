using PermanentBuffViewer.UI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace PermanentBuffViewer.UI
{
    
    /// <summary>
    /// A row in the UI that has a label on the left, a spacer, then a list of elements to the right
    /// </summary>
    internal class UIRow : UIElement, IAdjustableSize, IExpectedSize,
        IUpdateElementsOnWorldEntry

    {
        public float ExpectedHeight
        {
            get
            {
                float height = 0f;
                height = Math.Max(leftLabel.GetOuterDimensions().Height, subrows.ExpectedHeight);
                return height;
            }
        }

        public float ExpectedWidth
        {
            get
            {
                float width = 0f;
                width = leftLabel.GetOuterDimensions().Width + CenterSpacerWidth + 
                    subrows.ExpectedWidth;
                return width;
            }
        }

        // UIText overestimates its width by a lot.
        // Can't set left using UIText width + centerSpacer.
        // Instead, subtract the expected width of the subrow from the total width of the row.
        public float ExpectedSubrowLeft
        {
            get
            {
                var width = this.GetDimensions().Width;
                var expWidth = subrows.GetOuterDimensions().Width;
                return this.GetDimensions().Width - subrows.GetOuterDimensions().Width;
            }
        }

        private float _centerSpacerWidth;

        public float CenterSpacerWidth
        {
            get
            {
                return _centerSpacerWidth;
            }
            set
            {
                _centerSpacerWidth = value;
                //var dimensions = leftLabel.GetDimensions();
                subrows.Left.Set(leftLabel.GetOuterDimensions().Width + _centerSpacerWidth, 0f);
            }
        }

        internal UIText leftLabel;

        internal UISubrowList subrows;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="leftLabelKey">Represents the key in the Localization file containing the text for the left label.</param>
        /// <param name="numSubrows">The number of subrows to create for this row.</param>
        /// <exception cref="ArgumentException">If the number of subrows is less than 1.</exception>
        public UIRow(string leftLabelKey, int numSubrows = 1)
        {
            if (numSubrows < 1) throw new ArgumentException("Must have at least 1 subrow");
            // Should the text be the same size as, bigger, or smaller than the icons?
            // change text scaling to adjust (default = 1f)
            leftLabel = new UIText(Language.GetOrRegister(leftLabelKey));
            // Should the text be top-aligned with the icons or center-aligned?
            // change VAlign to adjust this
            leftLabel.VAlign = 0.3f;
            this.Append(leftLabel);
            subrows = new UISubrowList(numSubrows);
            subrows.Width.Set(0f, 0.4f);
            subrows.Height.Set(40f * numSubrows, 0f);
            subrows.VAlign = 0.5f;
            this.Append(subrows);
            //CenterSpacerWidth = 25f;
        }

        public UISubrow GetSubrow(int subrowIndex)
        {
            return subrows.GetSubrow(subrowIndex);
        }

        public int AddSubrow(UISubrow subrow)
        {
            return subrows.AddSubrow(subrow);
        }

        public int CreateNewSubrow()
        {
            return subrows.CreateNewSubrow();
        }

        public void AddElementToSubrow(int subrowIndex, BuffItemUIElement element)
        {
            subrows.AddElementToSubrow(subrowIndex, element);
            this.AdjustSize();
            this.AdjustSubrowLeft();
        }

        public void AddAllElementsToSubrow(int subrowIndex, IEnumerable<BuffItemUIElement> elements)
        {
            subrows.AddAllElementsToSubrow(subrowIndex, elements);
            this.AdjustSize();
            this.AdjustSubrowLeft();
        }

        public void SetLeftText(string localizationKey)
        {
            leftLabel.SetText(Language.GetOrRegister(localizationKey));
        }

        public void AdjustSize()
        {
            subrows.AdjustSize();
            //if (Width.Pixels < ExpectedWidth) Width.Set(ExpectedWidth, 0f);
            //if (Height.Pixels < ExpectedHeight) Height.Set(ExpectedHeight, 0f);
        }

        public void AdjustSubrowLeft()
        {
            //subrows.AdjustSize();
            subrows.Left.Set(ExpectedSubrowLeft, 0f);
            Recalculate();
        }

        public void UpdateElementsOnWorldEntry()
        {
            subrows.UpdateElementsOnWorldEntry();
        }
    }
}
