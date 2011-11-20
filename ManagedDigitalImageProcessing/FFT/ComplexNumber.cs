// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexNumber.cs" company="Tony Richards">
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
//   Defines the ComplexNumber type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.FFT
{
    using System;

    /// <summary>
    /// Represents a complex number, used by the FFT functions.
    /// </summary>
    /// <seealso cref="FFT"/>
    public sealed class ComplexNumber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexNumber"/> class.
        /// </summary>
        /// <param name="real">The real component.</param>
        /// <param name="imaginary">The imaginary component.</param>
        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        /// <summary>
        /// Gets or sets the real component of the number.
        /// </summary>
        /// <value>
        /// The real component of the value.
        /// </value>
        public double Real { get; set; }

        /// <summary>
        /// Gets or sets the imaginary component of the number.
        /// </summary>
        /// <value>
        /// The imaginary component of the value.
        /// </value>
        public double Imaginary { get; set; }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="c1">Complex number one.</param>
        /// <param name="c2">Complex number two.</param>
        /// <returns>
        /// Returns \f$c1+c2\f$.
        /// </returns>
        public static ComplexNumber operator +(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="c1">Complex number one.</param>
        /// <param name="c2">Complex number two.</param>
        /// <returns>
        /// Returns \f$c1-c2\f$.
        /// </returns>
        public static ComplexNumber operator -(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="c1">Complex number one.</param>
        /// <param name="c2">Complex number two.</param>
        /// <returns>
        /// Returns \f$c1 \times c2\f$.
        /// </returns>
        public static ComplexNumber operator *(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber((c1.Real * c2.Real) - (c1.Imaginary * c2.Imaginary), (c1.Real * c2.Imaginary) + (c2.Real * c1.Imaginary));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="ManagedDigitalImageProcessing.FFT.ComplexNumber"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Returns a ComplexNumber with <see cref="Real"/> set to <paramref name="value"/>.
        /// </returns>
        public static implicit operator ComplexNumber(int value)
        {
            return new ComplexNumber(value, 0);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Double"/> to <see cref="ManagedDigitalImageProcessing.FFT.ComplexNumber"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Returns a ComplexNumber with <see cref="Real"/> set to <paramref name="value"/>.
        /// </returns>
        public static implicit operator ComplexNumber(double value)
        {
            return new ComplexNumber(value, 0);
        }

        /// <summary>
        /// Returns the magnitude of this complex number.
        /// </summary>
        /// <returns>The magnitude of the complex number, defined as: \f[ \sqrt{(Real \times Real)+(Imaginary \times Imaginary)} \f].</returns>
        public double Magnitude()
        {
            return Math.Sqrt((Real * Real) + (Imaginary * Imaginary));
        }

        /// <summary>
        /// Returns the phase of this complex number.
        /// </summary>
        /// <returns>The phase, in radians, of the complex number, defined as: \f$ \arctan{\left(\frac{Imaginary}{Real}\right)} \f$</returns>
        public double Phase()
        {
            return Math.Atan(Imaginary / Real);
        }

        /// <summary>
        /// Swaps the imaginary and real terms. Used by the FFT functions.
        /// </summary>
        /// <returns>The complex number with the imaginary and real terms swapped.</returns>
        /// <seealso cref="FFT"/>
        /// <remarks>
        /// Converts \f$ a+bi \f$ to \f$ b+ai \f$.
        /// </remarks>
        public ComplexNumber Swap()
        {
            return new ComplexNumber(Imaginary, Real);
        }
    }
}
