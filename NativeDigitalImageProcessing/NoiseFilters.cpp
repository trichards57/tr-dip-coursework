#include "stdafx.h"
#include "NativeDigitalImageProcessing.h"
#include "Utilities.h"

using namespace std;

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT BitwiseAndFilter(int dataLength, char data[], char dataOut[], char mask)
{
	int i;

#pragma omp parallel for private(i) shared(data, dataOut, mask)
	for (i = 0; i < dataLength - 15; i += 16)
	{
		__m128i	reg = _mm_loadu_si128((__m128i*)(data+i));
		__m128i msk = _mm_set1_epi8(mask);

		__m128i result = _mm_and_si128(reg,msk);

		_mm_storeu_si128((__m128i*)(dataOut+i), result);
	}

	return 0;
}

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT MedianFilter(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int windowSize)
{
	int i;
	int offset = windowSize / 2;
	int dataCount = windowSize * windowSize;
	int medianPosition = dataCount / 2;
	int threads = omp_get_max_threads();

	unsigned char** lists = new unsigned char* [threads];
	for (i = 0; i < threads; i++)
		lists[i] = new unsigned char[dataCount];

#pragma omp parallel for private(i) shared(data, dataOut, lists)
	for (i = 0; i < picWidth; i++)
	{
		int threadNum = omp_get_thread_num();
		for (int j = 0; j < picHeight; j++)
		{
			int count = 0;
			for (int k = -offset; k <= offset; k++)
			{
				for (int l = -offset; l <= offset; l++)
				{
					lists[threadNum][count++] = data[CalculateIndex(i+k, j+l, picWidth, picHeight)];
				}
			}

			sort(lists[threadNum], lists[threadNum]+dataCount);
			dataOut[CalculateIndex(i, j, picWidth)] = lists[threadNum][medianPosition];
		}
	}

	for (i = 0; i < threads; i++)
		delete[] lists[i];
	delete[] lists;

	return 0;
}

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT HistogramMedianFilter(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int windowSize)
{
	int i;
	int offset = windowSize / 2;
	int dataCount = windowSize * windowSize;
	unsigned int medianPosition = dataCount / 2;

#pragma omp parallel for private(i) shared(data, dataOut)
	for (i = 0; i < picWidth; i++)
	{
		unsigned int histogram[256];

		memset(histogram, 0, 256*sizeof(unsigned int));

		for (int j = 0; j < picHeight; j++)
		{
			unsigned int counter = 0;

			if (j == 0)
			{
				for (int k = -offset; k <= offset; k++)
				{
					for (int l = -offset; l <= offset; l++)
					{
						histogram[data[CalculateIndex(i+k,j+l,picWidth,picHeight)]]++;
					}
				}
			}
			else
			{
				for (int k = -offset; k <= offset; k++)
				{
					histogram[data[CalculateIndex(i+k,j-offset-1,picWidth,picHeight)]]--;
					histogram[data[CalculateIndex(i+k,j+offset,picWidth,picHeight)]]++;
				}
			}
			
			for (unsigned char k = 0; k < 256; k++)
			{
				counter += histogram[k];
				if (counter < medianPosition) continue;
				dataOut[CalculateIndex(i,j,picWidth)] = k;
				break;
			}
		}
	}

	return 0;
}

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT AdaptiveHistogramMedianFilter(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int windowSize, double centreWeight, double scalingConstant)
{
	int i;
	int offset = windowSize / 2;
	int dataCount = windowSize * windowSize;
	

	#pragma omp parallel for private(i) shared(data, dataOut)
	for (i = 0; i < picWidth; i++)
	{
		for (int j = 0; j < picHeight; j++)
		{
			unsigned int histogram[256];
			memset(histogram, 0, 256*sizeof(unsigned int));

			unsigned int sum = 0;

			for (int k = -offset; k <= offset; k++)
			{
				for (int l = -offset; l <= offset; l++)
				{
					sum += data[CalculateIndex(i+k,j+l,picWidth,picHeight)];
				}
			}

			double mean = (double)sum / dataCount;

			double squareDiffTotal = 0;

			for (int k = -offset; k <= offset; k++)
			{
				for (int l = -offset; l <= offset; l++)
				{
					double level = data[CalculateIndex(i+k,j+l,picWidth,picHeight)];
					double diff = level - mean;
					squareDiffTotal += diff * diff;
				}
			}

			double variance = squareDiffTotal / dataCount;
			unsigned int weightSum = 0;

			for (int k = -offset; k <= offset; k++)
			{
				for (int l = -offset; l <= offset; l++)
				{
					double distance = sqrt((double)k*k+l*l);
					double weight = centreWeight - scalingConstant * distance * variance / mean;

					if (weight < 0)
						weight = 0;
					else
						weight = floor(weight);
					histogram[data[CalculateIndex(i+k,j+l,picWidth,picHeight)]] += weight;
					weightSum += weight;
				}
			}

			unsigned int medianPosition = (weightSum / 2) + 1;
			unsigned int counter = 0;

			for (unsigned char k = 0; k < 256; k++)
			{
				counter += histogram[k];
				if (counter < medianPosition) continue;

				dataOut[CalculateIndex(i,j,picWidth)] = k;
				break;
			}
		}
	}

	return 0;
}