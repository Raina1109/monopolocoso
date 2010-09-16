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
	//cout << vlist[0].at(X) << endl;
  //  cout <<"===========================" << endl
		//<< "triangle drawing" << endl
		//<< "===========================" << endl;
	SortingY (vlist,clist,3);
    Edge3D edge12(vlist[0],vlist[1],clist[0],clist[1]);
    Edge3D edge23(vlist[1],vlist[2],clist[1],clist[2]);
    Edge3D edge13(vlist[0],vlist[2],clist[0],clist[2]);

	
    for (int i = 0; i + vlist[0].at(Y) <= vlist[2].at(Y);i++)
    {
        GzVertex left;
        GzColor leftColor;
        GzVertex right;
        GzColor rightColor;
        double deltaY = i;
		double currentY = i + vlist[0].at(Y);

        if (currentY <= vlist[1][Y])
        {
			left.at(X) = Interpolate(edge12.start[Y],edge12.end[Y],edge12.start[X],edge12.end[X],currentY);
            left.at(Y) = currentY;
            //left.at(Z) = edge12.start[Z] + (edge12.slope_z * deltaY);
			left[Z] = Interpolate(edge12.start[Y],edge12.end[Y],edge12.start[Z],edge12.end[Z],currentY);
			
			//cout << "----------" << endl
			//  << edge12.start.at(X) << " " << edge12.slope_x << " " << deltaY << endl
			//	<< edge12.start[X] + edge12.slope_x * deltaY << endl
			//	<< currentY << endl
			//	<< edge12.start[Z] + edge12.slope_z * deltaY << endl;
			//printf("%f %f %f \n",left[X],left[Y],left[Z]);

            leftColor = linColorInterpolator(edge12.start[Y],edge12.end[Y]
                                        ,edge12.cstart,edge12.cend,currentY);
			
        } else {
			left.at(X) = Interpolate(edge23.start[Y],edge23.end[Y],edge23.start[X],edge23.end[X],currentY);
            //left[X] = edge23.start[X] + (edge23.slope_x * deltaY);
            left[Y] = currentY;
            left[Z] = Interpolate(edge23.start[Y],edge23.end[Y],edge23.start[Z],edge23.end[Z],currentY);
			
			leftColor = linColorInterpolator(edge23.start[Y],edge23.end[Y]
                                        ,edge23.cstart,edge23.cend,currentY);
            
        }
		//printf("%d %d %d \n",left[X],left[Y],left[Z]);
            //right[X] = edge13.start[X] + (edge13.slope_x * deltaY);
			right[X] = Interpolate(edge13.start[Y],edge13.end[Y],edge13.start[X],edge13.end[X],currentY);
            right[Y] = currentY;
            //right[Z] = edge13.start[Z] + (edge13.slope_z * deltaY);
			right[Z] = Interpolate(edge13.start[Y],edge13.end[Y],edge13.start[Z],edge13.end[Z],currentY);
		 
		//cout << "----------" << endl
		//	<< edge13.start.at(X) << " " << edge13.slope_x << " " << deltaY << endl
		//	<< edge13.start[X] + edge13.slope_x * deltaY << endl
		//	<< currentY << endl
		//	<< edge13.start[Z] + edge13.slope_z * deltaY << endl;
		//
		//printf("%f %f %f \n",right[X],right[Y],right[Z]);

        rightColor = linColorInterpolator(edge13.start[Y],edge13.end[Y]
                                     ,edge13.cstart,edge13.cend,currentY);
		//printf("%f %f %f \n",right[X],right[Y],right[Z]);
        if(left[X] > right[X])
        {
            GzVertex v; GzColor c;
            v = left;
            left = right;
            right = v;
            c = leftColor;
            leftColor = rightColor;
            rightColor = c;

			//printf("switching L-R point\n");
        }
		//printf("%f %f %f \n",left[X],left[Y],left[Z]);
		//printf("%f %f %f \n",right[X],right[Y],right[Z]);

        for(int deltaX = 0; left[X] + deltaX < right[X]; deltaX++)
        {
            GzVertex current;

			current[X] = deltaX + left[X];
            current[Y] = currentY;
            current[Z] = Interpolate(left[X],right[X],left[Z],right[Z],current[X]);
			
			//printf("%f %f %f \n",current[X],current[Y],current[Z]);
            GzColor currentColor = linColorInterpolator(left[X],right[X],leftColor,rightColor,current[X]);
			//printf("%f %f %f %f\n",currentColor[R],currentColor[G],currentColor[B],currentColor[A]);

            this->drawPoint(current,currentColor,status);
        }
    }

}
