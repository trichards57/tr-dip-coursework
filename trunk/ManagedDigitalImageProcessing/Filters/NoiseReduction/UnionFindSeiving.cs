using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    // From http://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=993556 A Comparison of Algorithms for Connected Set Openings and Closings
    class UnionFindSeiving : FilterBase
    {
        int[] _parent;

        byte[] _pixelData;
        int _lambda;

        public UnionFindSeiving(int lambda)
        {
            _lambda = lambda;
        }

        private void MakeSet(int x)
        {
            _parent[x] = -1;
        }

        private int FindRoot(int x)
        {
            if (_parent[x] >= 0)
            {
                _parent[x] = FindRoot(_parent[x]);
                return _parent[x];
            }
            else
                return x;
        }

        private bool Criterian(int x, int y)
        {
            return ((_pixelData[x] == _pixelData[y]) || (-_parent[x] < _lambda));
        }

        private void Union(int n, int p)
        {
            var r = FindRoot(n);
            if (r != p)
            {
                if (Criterian(r, p))
                {
                    _parent[p] = _parent[p] + _parent[r];
                    _parent[r] = p;
                }
                else
                    _parent[p] = -_lambda;
            }
        }

        public static int[] SortPixels(byte[] data)
        {
            var counter = 0;
            var output = new int[data.Length];

            for (var i = 255; i >= 0; i--)
            {
                for (var j = 0; j < data.Length; j++)
                {
                    if (data[j] == i)
                        output[counter++] = j;
                }
            }

            return output;
        }

        public PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage() { Header = input.Header, Data = new byte[input.Data.Length] };
            _pixelData = input.Data;
            _parent = new int[_pixelData.Length];
            
            var sortedPixels = SortPixels(_pixelData);
            var processedPixels = new bool[_pixelData.Length];

            for (var i = 0; i < processedPixels.Length; i++)
                processedPixels[i] = false;

            for (var i = 0; i < sortedPixels.Length; i++)
            {
                var pix = sortedPixels[i];
                var x = pix % input.Header.Width;
                var y = pix / input.Header.Height;

                MakeSet(pix);

                var neighbourX = new[] { x - 1, x, x + 1 };
                var neighbourY = new[] { y - 1, y, y + 1 };

                foreach (var xN in neighbourX)
                {
                    if (xN > 0 && xN < input.Header.Width)
                    {
                        foreach (var yN in neighbourY)
                        {
                            if (yN > 0 && yN < input.Header.Height && !(x==xN && y == yN))
                            {
                                var neighbourIndex = CalculateIndex(xN, yN, input.Header.Width, input.Header.Height);
                                if (processedPixels[neighbourIndex])
                                    if ((_pixelData[pix] < _pixelData[neighbourIndex]) || ((_pixelData[pix] == _pixelData[neighbourIndex])&&(neighbourIndex<pix)))
                                        Union(neighbourIndex, pix);
                            }
                        }
                    }
                }
                processedPixels[pix] = true;
            }

            for (var i = _parent.Length - 1; i >= 0; i--)
            {
                var s = sortedPixels[i];
                if (_parent[s] >= 0)
                    _parent[s] = _parent[_parent[s]];
                else
                    _parent[s] = _pixelData[s];
            }

            for (var i = 0; i < _parent.Length; i++)
                output.Data[i] = (byte)_parent[i];

            return output;
        }
    }
}
