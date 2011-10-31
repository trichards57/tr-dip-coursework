#pragma once

#include "stdafx.h"
#include "NativeDigitalImageProcessing.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT BitwiseAndFilter(int dataLength, char data[], char dataOut[], char mask);
extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT MedianFilter(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int windowSize);