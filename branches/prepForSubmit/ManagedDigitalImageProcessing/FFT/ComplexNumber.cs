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
