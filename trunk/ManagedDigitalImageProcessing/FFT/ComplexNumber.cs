using System;

namespace ManagedDigitalImageProcessing.FFT
{
    internal sealed class ComplexNumber
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public override string ToString()
        {
            return string.Format("{0}+({1})j", Real, Imaginary);
        }

        public double Magnitude()
        {
            return Math.Sqrt(Real * Real + Imaginary * Imaginary);
        }

        public double Phase()
        {
            return Math.Atan(Imaginary / Real);
        }

        public static ComplexNumber operator +(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }

        public static ComplexNumber operator -(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }

        public static ComplexNumber operator *(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary,
                                     c1.Real * c2.Imaginary + c2.Real * c1.Imaginary);
        }

        public static implicit operator ComplexNumber(int value)
        {
            return new ComplexNumber(value, 0);
        }

        public static explicit operator ComplexNumber(byte value)
        {
            return new ComplexNumber(value, 0);
        }

        public static implicit operator ComplexNumber(double value)
        {
            return new ComplexNumber(value, 0);
        }

        public ComplexNumber Swap()
        {
            return new ComplexNumber(Imaginary, Real);
        }
    }
}
