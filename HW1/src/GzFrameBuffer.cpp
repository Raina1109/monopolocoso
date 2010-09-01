#include "GzFrameBuffer.h"

//Put your implementation here------------------------------------------------
void GzFrameBuffer::initFrameSize (GzInt width, GzInt height)
{
	width_ = width;
	height_ = height;
}

GzImage GzFrameBuffer::toImage()
{
}

void GzFrameBuffer::clear(GzFunctional buffer)
{
}

void GzFrameBuffer::setClearColor(const GzColor& color)
{
}

void GzFrameBuffer::setClearDepth(GzReal depth)
{
}

void GzFrameBuffer::drawPoint(const GzVertex& v, const GzColor& c, GzFunctional status)
{
}