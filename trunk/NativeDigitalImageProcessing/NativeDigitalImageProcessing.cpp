// NativeDigitalImageProcessing.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NativeDigitalImageProcessing.h"


// This is an example of an exported variable
NATIVEDIGITALIMAGEPROCESSING_API int nNativeDigitalImageProcessing=0;

// This is an example of an exported function.
NATIVEDIGITALIMAGEPROCESSING_API int fnNativeDigitalImageProcessing(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see NativeDigitalImageProcessing.h for the class definition
CNativeDigitalImageProcessing::CNativeDigitalImageProcessing()
{
	return;
}
