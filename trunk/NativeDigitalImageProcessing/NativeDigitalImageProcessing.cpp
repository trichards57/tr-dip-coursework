// NativeDigitalImageProcessing.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NativeDigitalImageProcessing.h"

using namespace std;

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT BitwiseAndFilter(int dataLength, char data[], char dataOut[], char mask)
{
	int i;

	for (i = 0; i < dataLength - 15; i += 16)
	{
		__m128i	reg = _mm_loadu_si128((__m128i*)(data+i));
		__m128i msk = _mm_set1_epi8(mask);

		__m128i result = _mm_and_si128(reg,msk);

		_mm_storeu_si128((__m128i*)(dataOut+i), result);
	}

	return 0;
}

int CalculateIndex(int x, int y, int width, int height)
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
			dataOut[CalculateIndex(i, j, picWidth, picHeight)] = lists[threadNum][medianPosition];
		}
	}

	for (i = 0; i < threads; i++)
		delete[] lists[i];
	delete[] lists;

	return 0;
}