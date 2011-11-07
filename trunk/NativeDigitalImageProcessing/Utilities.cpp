#include "stdafx.h"
#include "Utilities.h"

int CalculateIndex(int x, int y, int width, int height)
{
	if (x < 0)
		x = -x;
	if (x >= width)
		x = width - (x - width) - 1;

	if (y < 0)
		y = -y;
	if (y >= height)
		y = height - (y - height) - 1;

	return CalculateIndex(x,y,width);
}

inline int CalculateIndex(int x, int y, int width)
{
	return x + y * width;
}