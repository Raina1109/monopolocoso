#include "Octree.h"


Octree::Octree(int d, Vec3f c1, Vec3f c2)
{
	corner1 = c1;
	corner2 = c2;
	center = (c1 + c2)/2;
	depth = d;
	hasChildren = false;
}


Octree::~Octree(void)
{
	if(hasChildren)
	{

	}
}

void Octree::clearNode()
{
	if(hasChildren)
	{
		for(int i = 0; i < 8; i++)
		{

		}
	}
}