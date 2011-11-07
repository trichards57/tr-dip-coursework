#pragma once

#include "stdafx.h"
#include "NativeDigitalImageProcessing.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT BitwiseAndFilter(int dataLength, unsigned char data[], char dataOut[], char mask);
extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT MedianFilter(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int windowSize);
extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT HistogramMedianFilter(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int windowSize);
extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT AdaptiveHistogramMedianFilter(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int windowSize, double centreWeight, double scalingConstant);
