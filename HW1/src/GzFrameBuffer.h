#ifndef __GZ_FRAME_BUFFER_H_
#define __GZ_FRAME_BUFFER_H_

#include "GzCommon.h"
#include "GzImage.h"
#include <vector>
using namespace std;

//Frame buffer with Z-buffer -------------------------------------------------
class GzFrameBuffer {
public:
	//The common interface
	void initFrameSize(GzInt width, GzInt height);
	GzImage toImage();

	void clear(GzFunctional buffer);
	void setClearColor(const GzColor& color);
	void setClearDepth(GzReal depth);

	void drawPoint(const GzVertex& v, const GzColor& c, GzFunctional status);

private:
	
	GzColor bgColor, *Color_Buffer;
	GzReal *Depth_Buffer, defDepth;
	GzInt Width;
	GzInt Height;

	
	//Put any variables and private functions for your implementation here

};
//----------------------------------------------------------------------------

// convert 2D index to 1D index
inline int idx2D_1D (int x, int y, int width)
{
	return x + y*width;
}

inline int round(GzReal a) {
	if (a > 0)
		return int(a + 0.5);
	else 
		return int(a - 0.5);
}
inline bool checkBound(int x,int y, int width, int height)
{
	return (x < width) && (y < height) && (x >= 0) && (y >= 0);
}

#endif
