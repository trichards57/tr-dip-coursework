#include "stdafx.h"
#include "Morphological.h"
#include "Utilities.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Erode(int dataLength, char data[], char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square)
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

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Dilate(int dataLength, char data[], char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square)
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