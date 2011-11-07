#include "stdafx.h"
#include "FFT.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT DitFFT(int dataCount, double realIn[], double imagIn[], double realOut[], double imagOut[])
{
	int i;

	if (dataCount == 1)
	{
		realOut[0] = realIn[0];
		imagOut[0] = imagIn[0];

		return 0;
	}

	int halfDataCount = dataCount / 2;

	double * realBuffer1 = new double[halfDataCount];
	double * imagBuffer1 = new double[halfDataCount];
	double * realBuffer2 = new double[halfDataCount];
	double * imagBuffer2 = new double[halfDataCount];

	for (i = 0; i < dataCount; i++)
	{
		int halfIndex = i / 2;
		if (i % 2 == 0)
		{
			realBuffer1[halfIndex] = realIn[i];
			imagBuffer1[halfIndex] = imagIn[i];
		}
		else
		{
			realBuffer2[halfIndex] = realIn[i];
			imagBuffer2[halfIndex] = imagIn[i];
		}
	}

	double * realOut1 = new double[halfDataCount];
	double * realOut2 = new double[halfDataCount];
	double * imagOut1 = new double[halfDataCount];
	double * imagOut2 = new double[halfDataCount];

	DitFFT(halfDataCount, realBuffer1, imagBuffer1, realOut1, imagOut1);
	DitFFT(halfDataCount, realBuffer2, imagBuffer2, realOut2, imagOut2);

	for (i = 0; i < halfDataCount; i++)
	{
		double rTemp = cos(2*M_PI*i/dataCount);
		double iTemp = -sin(2*M_PI*i/dataCount);
		realOut[i] = realOut1[i] + COMPLEX_MULTIPLY_REAL(rTemp, iTemp, realOut2[i], imagOut2[i]);
		imagOut[i] = imagOut1[i] + COMPLEX_MULTIPLY_IMAG(rTemp, iTemp, realOut2[i], imagOut2[i]);
		realOut[i + halfDataCount] = realOut1[i] - COMPLEX_MULTIPLY_REAL(rTemp, iTemp, realOut2[i], imagOut2[i]);
		imagOut[i + halfDataCount] = imagOut1[i] - COMPLEX_MULTIPLY_IMAG(rTemp, iTemp, realOut2[i], imagOut2[i]);
	}

	return 0;
}