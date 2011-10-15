using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    /// <summary>
    /// A base class to allow the CalculateIndex function to be reused, and to make calling multiple different
    /// filters in sequence easier.
    /// </summary>
    abstract class FilterBase
    {
        /// <summary>
        /// Calculates the index of a point at given coordinates.  Points that fall out of the range of the
        /// image are reflected back in to the image (only works for positions less than width or height out
        /// of range.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>An index to access the given point in a 1D array.</returns>
        protected static int CalculateIndex(int x, int y, int width, int height)
        {
            if (x < 0)
                x = -x;
            if (x >= width)
                x = width - (x - width) - 1;

            if (y < 0)
                y = -y;
            if (y >= height)
                y = height - (y - height) - 1;

            return x + y * width;
        }

        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <returns>The filtered image.</returns>
        public abstract PgmImage Filter(PgmImage input);
    }
}
