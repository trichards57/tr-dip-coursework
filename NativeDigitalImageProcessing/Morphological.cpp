#include "stdafx.h"
#include "Morphological.h"
#include "Utilities.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Erode(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square)
{
	int i;
	int offset = orderingFunctionSize / 2;
	int dataCount = orderingFunctionSize * orderingFunctionSize;
	
	#pragma omp parallel for private(i) shared(data, dataOut)
	for (i = 0; i < picWidth; i++)
	{
		for (int j = 0; j < picHeight; j++)
		{
			unsigned char min = 255;

			for (int k = -offset; k <= offset; k++)
			{
				for (int l = -offset; l <= offset; l++)
				{
					if (!square && sqrt((double)k*k + l*l) >= offset) continue;
					unsigned char value = data[CalculateIndex(i+k, j+l, picWidth, picHeight)];
					if (value < min)
						min = value;
				}
			}

			dataOut[CalculateIndex(i,j,picWidth)] = min; 
		}
	}

	return 0;
}

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Dilate(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square)
{
	int i;
	int offset = orderingFunctionSize / 2;
	int dataCount = orderingFunctionSize * orderingFunctionSize;
	
	#pragma omp parallel for private(i) shared(data, dataOut)
	for (i = 0; i < picWidth; i++)
	{
		for (int j = 0; j < picHeight; j++)
		{
			unsigned char max = 0;

			for (int k = -offset; k <= offset; k++)
			{
				for (int l = -offset; l <= offset; l++)
				{
					if (!square && sqrt((double)k*k + l*l) >= offset) continue;
					unsigned char value = data[CalculateIndex(i+k, j+l, picWidth, picHeight)];
					if (value > max)
						max = value;
				}
			}

			dataOut[CalculateIndex(i,j,picWidth)] = max; 
		}
	}

	return 0;
}

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Close(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square)
{
	unsigned char* buffer = new unsigned char[dataLength];

	Dilate(dataLength, data, buffer, picWidth, picHeight, orderingFunctionSize, square);
	Erode(dataLength, buffer, dataOut, picWidth, picHeight, orderingFunctionSize, square);

	return 0;
}

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Open(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square)
{
	unsigned char* buffer = new unsigned char[dataLength];

	Erode(dataLength, data, buffer, picWidth, picHeight, orderingFunctionSize, square);
	Dilate(dataLength, buffer, dataOut, picWidth, picHeight, orderingFunctionSize, square);

	delete[] buffer;

	return 0;
}

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT MorphologicalGradient(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square)
{
	unsigned char* erodeBuffer = new unsigned char[dataLength];
	unsigned char* dilateBuffer = new unsigned char[dataLength];

	Erode(dataLength, data, erodeBuffer, picWidth, picHeight, orderingFunctionSize, square);
	Dilate(dataLength, data, dilateBuffer, picWidth, picHeight, orderingFunctionSize, square);

	for (int i = 0; i < dataLength - 15; i+= 16)
	{
		__m128i	erodeReg = _mm_loadu_si128((__m128i*)(erodeBuffer+i));
		__m128i	dilateReg = _mm_loadu_si128((__m128i*)(dilateBuffer+i));

		__m128i result = _mm_sub_epi8(dilateReg, erodeReg);
		_mm_storeu_si128((__m128i*)(dataOut+i), result);
	}

	return 0;
}