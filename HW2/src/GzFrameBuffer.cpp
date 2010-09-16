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

	if(!checkBound(VXCoord,VYCoord,Width,Height))
		return;

	if (status & GZ_DEPTH_TEST)
	{
		if(v[Z] > Depth_Buffer[idx2D_1D(VXCoord,VYCoord,Width)])
		{
			Depth_Buffer[idx2D_1D(VXCoord,VYCoord,Width)] = v[Z];
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
    SortingY (vlist,clist,3);
    Edge3D edge12(vlist[0],vlist[1],clist[0],clist[1]);
    Edge3D edge23(vlist[1],vlist[2],clist[1],clist[2]);
    Edge3D edge13(vlist[0],vlist[2],clist[0],clist[2]);

    for (double i = vlist[0][Y]; i < vlist[2][Y];i++)
    {
        GzVertex left;
        GzColor leftColor;
        GzVertex right;
        GzColor rightColor;
        double deltaY = i-vlist[2][Y];

        if (i <= vlist[1][Y])
        {
            left = GzVertex(
                    edge23.start[X] + edge23.slope_x * deltaY,
                    i,
                    edge23.start[X] + edge23.slope_z * deltaY
                    );
            leftColor = linColorInterpolator(edge23.start[X],edge23.end[X]
                                        ,edge23.cstart,edge23.cend,i);

        } else {
            left = GzVertex(
                    edge12.start[X] + edge12.slope_x * deltaY,
                    i,
                    edge12.start[X] + edge12.slope_z * deltaY
                    );
            leftColor = linColorInterpolator(edge12.start[X],edge12.end[X]
                                        ,edge12.cstart,edge12.cend,i);
        }

        right = GzVertex(
                edge13.start[X] + edge13.slope_x * deltaY,
                i,
                edge13.start[X] + edge13.slope_z * deltaY
                );
        rightColor = linColorInterpolator(edge13.start[X],edge13.end[X]
                                     ,edge13.cstart,edge13.cend,i);

        if(left[X] > right[X])
        {
            GzVertex v; GzColor c;
            v = left;
            left = right;
            right = v;
            c = leftColor;
            leftColor = rightColor;
            rightColor = c;
        }

        for(double currentX = left[X]; currentX < right[X]; currentX++)
        {
            GzVertex current;

            current[X] = currentX;
            current[Y] = i;
            current[Z] = Interpolate(left[X],right[X],left[Z],right[Z],currentX);

            GzColor currentColor = linColorInterpolator(left[X],right[X],leftColor,rightColor,currentX);

            this->drawPoint(current,currentColor,status);
        }
    }

}
