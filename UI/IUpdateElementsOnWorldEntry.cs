using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermanentBuffViewer.UI
{
    internal interface IUpdateElementsOnWorldEntry
    {
        /// <summary>
        /// Called when an element has UIElements to update when entering a world.
        /// </summary>
        void UpdateElementsOnWorldEntry();
    }
}
