#pragma once

#include "stdafx.h"
#include "NativeDigitalImageProcessing.h"

extern "C" NATIVEDIGITALIMAGEPROCESSING_API HRESULT DitFFT(int dataNumber, double realIn[], double imagIn[], double realOut[], double imageOut[]);

#define COMPLEX_MULTIPLY_REAL(r1,i1,r2,i2) (r1 * r2 - i1 * i2)
#define COMPLEX_MULTIPLY_IMAG(r1,i1,r2,i2) (r1 * i2 + i1 * r2)
