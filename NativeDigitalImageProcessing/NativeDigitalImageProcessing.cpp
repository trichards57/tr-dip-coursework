// NativeDigitalImageProcessing.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NativeDigitalImageProcessing.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT BitwiseAndFilter(int dataLength, char data[], char mask)
{
	int i;

	for (i = 0; i < dataLength-15;i+=16)
	{
		__m128i	reg = _mm_loadu_si128((__m128i*)(data+i));
		__m128i msk = _mm_set1_epi8(mask);

		__m128i result = _mm_and_si128(reg,msk);

		_mm_storeu_si128((__m128i*)(data+i), result);
	}

	return 0;
}
