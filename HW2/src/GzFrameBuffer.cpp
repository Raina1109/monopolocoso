#include "GzFrameBuffer.h"

//Put your implementation here------------------------------------------------



void GzFrameBuffer::initFrameSize (GzInt width, GzInt height)
{
	Width = width;
	Height = height;
	bgColor = GzColor(0,0,0);
	defDepth = 0;
	Color_Buffer = new GzColor[Width*Height];
	Depth_Buffer = new GzReal[Width*Height];

}

GzImage GzFrameBuffer::toImage()
{
	GzImage image(Width,Height);
	for(int j = 0; j <Height;j++)
			for(int i = 0; i < Width;i++)
				image.set(i,j,Color_Buffer[idx2D_1D(i,j,Width)]);

	return image;
}

void GzFrameBuffer::clear(GzFunctional buffer)
{
	if( buffer & GZ_COLOR_BUFFER )
	{
		for(int j = 0; j <Height;j++)
			for(int i = 0; i < Width;i++)
				Color_Buffer[idx2D_1D(i,j,Width)] = bgColor;
	}

	if ( buffer & GZ_DEPTH_BUFFER )
	{
		for(int j = 0; j <Height;j++)
			for(int i = 0; i < Width;i++)
				Depth_Buffer[idx2D_1D(i,j,Width)] = defDepth;
	}
}

void GzFrameBuffer::setClearColor(const GzColor& color)
{
	bgColor = color;
}

void GzFrameBuffer::setClearDepth(GzReal depth)
{
	defDepth = depth;
}

void GzFrameBuffer::drawPoint(const GzVertex& v, const GzColor& c, GzFunctional status)
{
	
	int VXCoord = round(v[X]);
	//cout << VXCoord << endl;
	int VYCoord = -round(v[Y])+Height-1;
	//cout << VYCoord << endl;
	int VZCoord = round(v[Z]);
	//cout << VZCoord << endl;

	if(!checkBound(VXCoord,VYCoord,Width,Height))
		return;

	if (status & GZ_DEPTH_TEST)
	{
		if(VZCoord > Depth_Buffer[idx2D_1D(VXCoord,VYCoord,Width)])
		{
			Depth_Buffer[idx2D_1D(VXCoord,VYCoord,Width)] = VZCoord;
			Color_Buffer[idx2D_1D(VXCoord,VYCoord,Width)] = c;
		}
	}
	else
	{
		Color_Buffer[idx2D_1D(VXCoord,VYCoord,Width)] = c;
	}
}

void GzFrameBuffer::drawTriangle(GzVertex *vlist, GzColor *clist, GzFunctional status)
{
    YSort(vlist,clist,3);
    Edge3D edge12(vlist[0],vlist[1],clist[0],clist[1]);
    Edge3D edge23(vlist[1],vlist[2],clist[1],clist[2]);
    Edge3D edge13(vlist[0],vlist[2],clist[0],clist[2]);

    for (int i = vlist[2][Y]; i < vlist[0][X];i++)
    {

    }

}
