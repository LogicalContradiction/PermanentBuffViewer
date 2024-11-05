using PermanentBuffViewer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace PermanentBuffViewer
{
    /// <summary>
    /// Holds a condition and a DifficultyLockedItemUIIcon to be used in evaluating
    /// if a UI element should be added/removed based on the difficulty of the world.
    /// </summary>
    internal class DiffLockedUITest
    {

        Condition condition;
        DifficultyLockedItemUIIcon uiElement;

        public DiffLockedUITest(Condition condition, DifficultyLockedItemUIIcon element)
        {
            this.condition = condition;
            this.uiElement = element;
        }

        public bool ShouldBeAddedToUI()
        {
            return condition.IsMet();
        }


        public override string ToString()
        {
            return $"Diff Locked Test Condition: {condition}, element: {uiElement}";
        }




    }
}
