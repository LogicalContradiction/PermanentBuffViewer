using PermanentBuffViewer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace PermanentBuffViewer
{
    /// <summary>
    /// Holds a condition and a DifficultyLockedItemUIIcon to be used in evaluating
    /// if a UI element should be added/removed based on the difficulty of the world.
    /// </summary>
    internal class DiffLockedUITest
    {

        internal Condition condition;
        internal DifficultyLockedItemUIIcon UIElement { get; }
        internal UIElement Parent { get; }

        public DiffLockedUITest(Condition condition, DifficultyLockedItemUIIcon element, UIElement parent)
        {
            this.condition = condition;
            this.UIElement = element;
            this.Parent = parent;
        }

        public bool ShouldBeAddedToUI()
        {
            return condition.IsMet();
        }


        public override string ToString()
        {
            return $"Diff Locked Test Condition: {condition}, element: {UIElement}, parent: {Parent}";
        }




    }
}
