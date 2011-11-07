#pragma once

#include "NativeDigitalImageProcessing.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Erode(int dataLength, char data[], char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square);
extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Dilate(int dataLength, char data[], char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square);