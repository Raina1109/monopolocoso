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

        void drawTriangle(GzVertex* vlist, GzColor *clist, GzFunctional status);
	void drawPoint(const GzVertex& v, const GzColor& c, GzFunctional status);

private:
	
	GzColor bgColor, *Color_Buffer;
	GzReal *Depth_Buffer, defDepth;
	GzInt Width;
	GzInt Height;

	
	//Put any variables and private functions for your implementation here

};


//----------------------------------------------------------------------------
//GzColor colorSlopeCal(const GzVertex& start,const GzVertex& end,
//                         const GzColor& startColor,const GzColor& endColor)
//{
//    GzColor result;
//    for(int i = 0; i < 4; i ++)
//    {
//        result[i] = (endColor[i]-startColor[i])/(end[Y]-start[Y]);
//    }
//    return result;
//}

struct Edge3D
{
    GzVertex start, end;
    GzColor cstart, cend;
//    GzColor slope_c;
    double slope_x, slope_z;

    Edge3D(const GzVertex& st,const GzVertex& ed,
           const GzColor& cst,const GzColor& ced)
    {
        start = st;
        end = ed;
        cstart = cst; cend = ced;
        slope_x = (end[X]-start[X])/(end[Y]-start[Y]);
        slope_z = (end[Z]-start[Z])/(end[Y]-start[Y]);
//        slope_c = colorSlopeCal(start,end,cstart,cend);
    }
};

//
//struct Span
//{
//    double start[2], end[2], slope_z;
//    GzColor slope_c;
//    Span(const GzVertex& st,const GzVertex& ed,
//         const GzColor& cst,const GzColor& ced)
//    {
//        start[X] = st[X]; start[X] = st[Y];
//        end[X] = ed[X]; end[Y] = st[Y];
//        slope_z = (ed[Z]-st[Z])/(ed[Y]-st[Y]);
//        slope_c = colorSlopeCal(st,ed,cst,ced);
//    }
//
//};

// linear intepolator for z value
inline double Interpolate (double x0, double x1, double y0, double y1, double x)
{
    double deltaX = x1 - x0;
    double deltaY = y1 - y0;
    return y0 + (x - x0)*(deltaY/deltaX);
}

//linear color interpolator
inline GzColor linColorInterpolator (double startX, double endX, GzColor start, GzColor end, double x)
{
    double deltaX = endX - startX;
    GzColor result;
    for (int i = 0; i < 4; i++)
    {
        result[i] = start[i] + (x-startX)*((end[i]-start[i])/deltaX);
    }
    return result;
}

// Sorting vertices in triangle according to their Y value Ascendingly
inline void SortingY (GzVertex* vlist,GzColor* clist, int size)
{
    bool swaped = true;
    GzVertex vtemp;
    GzColor ctemp;
    while (swaped)
    {
        swaped = false;
        for (int i = 0; i < size-1; i++)
        {
            if(vlist[i][Y] < vlist[i+1][Y])
            {
                swaped = true;
                vtemp = vlist[i];
                ctemp = clist[i];
                vlist[i] = vlist[i+1];
                clist[i] = clist[i+1];
                vlist[i+1] = vtemp;
                clist[i+1] = ctemp;
            }
        }
    }
}

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
