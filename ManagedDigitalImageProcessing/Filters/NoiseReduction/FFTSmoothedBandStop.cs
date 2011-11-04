using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedDigitalImageProcessing.FFT;
using ManagedDigitalImageProcessing.Filters.Utilities;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    class FFTSmoothedBandStop : FilterBase
    {
        private int _inner;
        private int _outer;
        private int _rampLength;

        public FFTSmoothedBandStop(int inner, int outer, int rampLength)
        {
            _inner = inner;
            _outer = outer;
            _rampLength = rampLength;
        }

        public PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage { Header = input.Header };
            var width = input.Header.Width;
            var height = input.Header.Height;

            var newWidth = (int)Math.Pow(2, Math.Ceiling(Math.Log(width, 2)));
            var newHeight = (int)Math.Pow(2, Math.Ceiling(Math.Log(height, 2)));

            var resizer = new Resizer(newWidth, newHeight);
            var resizedImage = resizer.Filter(input);

            var complexInputData = new ComplexNumber[newWidth * newHeight];
            Parallel.For(0, newWidth * newHeight, i => complexInputData[i] = resizedImage.Data[i]);

            var fftOutput = FFT.FFT.DitFFT2D(complexInputData, newWidth, newHeight);
                
            var centerX = newWidth / 2.0 + 0.5;
            var centerY = newHeight / 2.0 + 0.5;

            Parallel.For(0, newWidth, i =>
                                          {
                                              for (var j = 0; j < newHeight; j++)
                                              {
                                                  var distX = i - centerX;
                                                  var distY = j - centerY;
                                                  var dist = Math.Sqrt(distX * distX + distY * distY);
                                                  if (dist < _inner && dist > _inner - _rampLength)
                                                  {
                                                      fftOutput[CalculateIndex(i, j, newWidth, newHeight)] *=
                                                          (Math.Abs(dist - _inner)/_rampLength);
                                                  }
                                                  else if (dist > _inner && dist < _outer)
                                                      fftOutput[CalculateIndex(i, j, newWidth, newHeight)] = 0;
                                                  else if (dist > _outer && dist < _outer + _rampLength)
                                                      fftOutput[CalculateIndex(i, j, newWidth, newHeight)] *=
                                                          (Math.Abs(dist - _outer) / _rampLength);
                                              }
                                          });

            var ifftOutput = FFT.FFT.InverseDitFFT2D(fftOutput, newWidth, newHeight);

            var magnitudes = ifftOutput.Select(n => n.Magnitude()).ToList();
            var max = magnitudes.Max();


            output.Data = magnitudes.Select(n => (byte)(255 * n / max)).ToArray();
            output.Header = new PgmHeader {Height = newHeight, Width = newWidth};

            return output;
        }
    }
}
