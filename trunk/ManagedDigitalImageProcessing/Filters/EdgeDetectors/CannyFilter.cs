// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CannyFilter.cs" company="Tony Richards">
//   Copyright (c) 2011, Tony Richards
//   All rights reserved.
//
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//
//   Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
//   disclaimer.
//   
//   Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the 
//   following disclaimer in the documentation and/or other materials provided with the distribution.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <summary>
//   Defines the CannyFilter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// Filter class, used to perform the canny edge detection on an image in one step.
    /// </summary>
    /// <remarks>
    /// This code skips the intial smoothing/filtering step, which should be performed on the data before it is passed in.
    /// </remarks>
    public sealed class CannyFilter
    {
        /// <summary>
        /// The hysteresis thresholding filter used by the process.
        /// </summary>
        private readonly HysteresisThresholding hysteresis;

        /// <summary>
        /// The non maximul suppression filter used by the process.
        /// </summary>
        private readonly NonMaximumSuppression nonMaximal;
        
        /// <summary>
        /// The sobel operator filter used by the process.
        /// </summary>
        private readonly SobelOperator edgeFilter;

        /// <summary>
        /// The upper threshold for the hysteresis thresholding.
        /// </summary>
        private readonly int highT;

        /// <summary>
        /// The lower threshold for the hysteresis thresholding.
        /// </summary>
        private readonly int lowT;

        /// <summary>
        /// Initializes a new instance of the <see cref="CannyFilter"/> class.
        /// </summary>
        /// <param name="highThreshold">The high threshold.</param>
        /// <param name="lowThreshold">The low threshold.</param>
        public CannyFilter(int highThreshold, int lowThreshold)
        {
            hysteresis = new HysteresisThresholding(highThreshold, lowThreshold);
            nonMaximal = new NonMaximumSuppression();
            edgeFilter = new SobelOperator();
            highT = highThreshold;
            lowT = lowThreshold;
        }

        /// <summary>
        /// Applies the Canny Edge Detector to the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The edge detected output.</returns>
        /// <remarks>Algorithm taken from @cite imageProcessingBook</remarks>
        public ImageData Filter(ImageData input)
        {
            return hysteresis.Filter(nonMaximal.Filter(edgeFilter.FilterSplit(input)).ToPgmImage());
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Canny {0} {1}", highT, lowT);
        }
    }
}
