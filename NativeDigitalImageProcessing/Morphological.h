#pragma once

#include "NativeDigitalImageProcessing.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Erode(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square);
extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Dilate(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square);
extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Close(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square);
extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT Open(int dataLength, unsigned char data[], unsigned char dataOut[], int picWidth, int picHeight, int orderingFunctionSize, bool square);