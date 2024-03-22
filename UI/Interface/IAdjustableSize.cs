using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermanentBuffViewer.UI.Interface
{
    internal interface IAdjustableSize
    {
        /// <summary>
        /// Adjusts the size of this element in case it is too small.
        /// </summary>
        void AdjustSize();
    }
}
