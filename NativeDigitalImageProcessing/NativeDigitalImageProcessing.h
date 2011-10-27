// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the NATIVEDIGITALIMAGEPROCESSING_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// NATIVEDIGITALIMAGEPROCESSING_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef NATIVEDIGITALIMAGEPROCESSING_EXPORTS
#define NATIVEDIGITALIMAGEPROCESSING_API __declspec(dllexport)
#else
#define NATIVEDIGITALIMAGEPROCESSING_API __declspec(dllimport)
#endif

// This class is exported from the NativeDigitalImageProcessing.dll
class NATIVEDIGITALIMAGEPROCESSING_API CNativeDigitalImageProcessing {
public:
	CNativeDigitalImageProcessing(void);
	// TODO: add your methods here.
};

extern NATIVEDIGITALIMAGEPROCESSING_API int nNativeDigitalImageProcessing;

NATIVEDIGITALIMAGEPROCESSING_API int fnNativeDigitalImageProcessing(void);