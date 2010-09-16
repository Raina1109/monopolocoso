#include "Gz.h"



//============================================================================
//Implementations in Assignment #1
//============================================================================
GzImage Gz::toImage() {
	return frameBuffer.toImage();
}

void Gz::clear(GzFunctional buffer) {
	frameBuffer.clear(buffer);
}

void Gz::clearColor(const GzColor& color) {
	frameBuffer.setClearColor(color);
}

void Gz::clearDepth(GzReal depth) {
	frameBuffer.setClearDepth(depth);
}

void Gz::enable(GzFunctional f) {
	status=status|f;
}

void Gz::disable(GzFunctional f) {
	status=status&(~f);
}

GzBool Gz::get(GzFunctional f) {
	if (status&f) return true; else return false;
}

void Gz::begin(GzPrimitiveType p) {
	currentPrimitive=p;
}

void Gz::addVertex(const GzVertex& v) {
	vertexQueue.push(v);
}

void Gz::addColor(const GzColor& c) {
	colorQueue.push(c);
}
//============================================================================
//End of Implementations in Assignment #1
//============================================================================



//============================================================================
//Implementations in Assignment #3
//============================================================================

void Gz::initFrameSize(GzInt width, GzInt height) {
	//This function need to be updated since we have introduced the viewport.
	//The viewport size is set to the size of the frame buffer.
	wViewport=(GzReal)width;
	hViewport=(GzReal)height;
	frameBuffer.initFrameSize(width, height);
	viewport(0, 0);			//Default center of the viewport 
}

void Gz::end() {
	//This function need to be updated since we have introduced the viewport,
	//projection, and transformations.
	//In our implementation, all rendering is done when Gz::end() is called.
	//Depends on selected primitive, different number of vetices, colors, ect.
	//are pop out of the queue.
	switch (currentPrimitive) {
		case GZ_POINTS: {
			while ( (vertexQueue.size()>=1) && (colorQueue.size()>=1) ) {
			}
		} break;
		case GZ_TRIANGLES: {
			//Put your triangle drawing implementation here:
			//   - Extract 3 vertices in the vertexQueue
			//   - Extract 3 colors in the colorQueue
			//   - Call the draw triangle function 
			//     (you may put this function in GzFrameBuffer)
		}
	}
}

void Gz::viewport(GzInt x, GzInt y) {
	//This function only updates xViewport and yViewport.
	//Viewport calculation will be done in different function, e.g. Gz::end().
	//See http://www.opengl.org/sdk/docs/man/xhtml/glViewport.xml
	//Or google: glViewport
	xViewport=x;
	yViewport=y;
}

//Transformations-------------------------------------------------------------
void Gz::lookAt(GzReal eyeX, GzReal eyeY, GzReal eyeZ, GzReal centerX, GzReal centerY, GzReal centerZ, GzReal upX, GzReal upY, GzReal upZ) {
	//Define viewing transformation
	//See http://www.opengl.org/sdk/docs/man/xhtml/gluLookAt.xml
	//Or google: gluLookAt
}

void Gz::translate(GzReal x, GzReal y, GzReal z) {
	//Multiply transMatrix by a translation matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/glTranslate.xml
	//    http://en.wikipedia.org/wiki/Translation_(geometry)
	//Or google: glTranslate
}

void Gz::rotate(GzReal angle, GzReal x, GzReal y, GzReal z) {
	//Multiply transMatrix by a rotation matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/glRotate.xml
	//    http://en.wikipedia.org/wiki/Rotation_(geometry)
	//Or google: glRotate
}

void Gz::scale(GzReal x, GzReal y, GzReal z) {
	//Multiply transMatrix by a scaling matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/glScale.xml
	//    http://en.wikipedia.org/wiki/
	//Or google: glScale
}

void Gz::multMatrix(GzMatrix mat) {
	//Multiply transMatrix by the matrix mat
	transMatrix=mat*transMatrix;
}
//End of Transformations------------------------------------------------------

//Projections-----------------------------------------------------------------
void Gz::perspective(GzReal fovy, GzReal aspect, GzReal zNear, GzReal zFar) {
	//Set up a perspective projection matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/gluPerspective.xml
	//Or google: gluPerspective
}

void Gz::orthographic(GzReal left, GzReal right, GzReal bottom, GzReal top, GzReal nearVal, GzReal farVal) {
	//Set up a orthographic projection matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/glOrtho.xml
	//Or google: glOrtho
}
//End of Projections----------------------------------------------------------


//============================================================================
//End of Implementations in Assignment #3
//============================================================================
