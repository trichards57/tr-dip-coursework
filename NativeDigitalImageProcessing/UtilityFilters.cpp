#include "stdafx.h"
#include "UtilityFilters.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT BicubicInterpolationScaling(int dataLength, char data[], int outputLength, char dataOut[], int picWidth, int picHeight, int targetWidth, int targetHeight)
{
	float xScale = picWidth / targetWidth;
	float yScale = picHeight / targetHeight;

	for (int i = 0; i < targetWidth; i++)
	{
		
	}
}