using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermanentBuffViewer.UI.Interface
{
    internal interface IExpectedSize
    {
        /// <summary>
        /// The expected height of this element.
        /// </summary>
        public float ExpectedHeight { get; }

        /// <summary>
        /// The expected width of this element
        /// </summary>
        public float ExpectedWidth { get; }

    }
}
