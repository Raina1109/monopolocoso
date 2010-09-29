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
void Gz::lookAt(GzReal eyeX, GzReal eyeY,
                GzReal eyeZ, GzReal centerX, GzReal centerY,
                GzReal centerZ, GzReal upX, GzReal upY, GzReal upZ)
{
    //Define viewing transformation
    //See http://www.opengl.org/sdk/docs/man/xhtml/gluLookAt.xml
    //Or google: gluLookAt
    GzReal forward[] = {centerX - eyeX,centerY - eyeY,centerZ - eyeZ};
    GzReal up[] = {upX, upY, upZ};
    GzReal side[3];
    //--------------
    //side = forward x up
    CrossProduct(side,forward,up);
    eucledianNorm(side);
    //--------------
    //recompute up vector as: up = side x forward
    CrossProduct(up,side,forward);
    //--------------
    GzReal arrayM[] = { side[0], side[1], side[2], 0,
                        up[0], up[1], up[2], 0,
                       -forward[0],-forward[1],-forward[2],0,
                        0, 0, 0, 1};
    GzMatrix M;
    copyArrayToMatrix(arrayM,M,4,4);

    multMatrix(M);
    translate(-eyeX,-eyeY, -eyeZ);
}

void Gz::translate(GzReal x, GzReal y, GzReal z) {
	//Multiply transMatrix by a translation matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/glTranslate.xml
	//    http://en.wikipedia.org/wiki/Translation_(geometry)
	//Or google: glTranslate
    GzReal arrayM[] = {1, 0, 0, x,
                       0, 1, 0, y,
                       0, 0, 1, z,
                       0, 0, 0, 1};

    GzMatrix M;
    copyArrayToMatrix(arrayM,M,4,4);
    multMatrix(M);
}

void Gz::rotate(GzReal angle, GzReal x, GzReal y, GzReal z) {
	//Multiply transMatrix by a rotation matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/glRotate.xml
	//    http://en.wikipedia.org/wiki/Rotation_(geometry)
	//Or google: glRotate
    angle = angle*PI/180;
    GzReal c = cos(angle);
    GzReal s = sin(angle);

    GzReal v[] = {x, y, z};
    eucledianNorm(v);

    GzReal arrayM[] = {v[X]*v[X]*(1-c) + c, v[X]*v[Y]*(1-c) - v[Z]*s, v[X]*v[Z]*(1-c) + v[Y]*s, 0,
                       v[Y]*v[X]*(1-c) + v[Z]*s, v[Y]*v[Y]*(1-c) + c, v[Y]*v[Z]*(1-c) - v[X]*s, 0,
                       v[X]*v[Z]*(1-c) - v[Y]*s, v[Y]*v[Z]*(1-c) + v[X]*s, v[Z]*v[Z]*(1-c) + c, 0,
                       0, 0, 0, 1};

    GzMatrix M;
    copyArrayToMatrix(arrayM,M,4,4);

    multMatrix(M);

}

void Gz::scale(GzReal x, GzReal y, GzReal z) {
	//Multiply transMatrix by a scaling matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/glScale.xml
	//    http://en.wikipedia.org/wiki/
	//Or google: glScale
    GzReal arrayM[] = {x, 0, 0, 0,
                       0, y, 0, 0,
                       0, 0, z, 0,
                       0, 0, 0, 1};

    GzMatrix M;
    copyArrayToMatrix(arrayM,M,4,4);

    multMatrix(M);
}

void Gz::multMatrix(GzMatrix mat) {
	//Multiply transMatrix by the matrix mat
	transMatrix=transMatrix*mat;
}
//End of Transformations------------------------------------------------------

//Projections-----------------------------------------------------------------
void Gz::perspective(GzReal fovy, GzReal aspect, GzReal zNear, GzReal zFar) {
	//Set up a perspective projection matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/gluPerspective.xml
	//Or google: gluPerspective
    GzReal f = cotan(fovy/2);
    GzReal arrayM[] = {f/aspect, 0, 0, 0,
                       0, f, 0, 0,
                       0, 0, (zFar+zNear)/(zNear - zFar), 2*(zFar*zNear)/(zNear-zFar),
                       0, 0, -1, 0};

    GzMatrix M;
    copyArrayToMatrix(arrayM,M,4,4);

    multMatrix(M);
}

void Gz::orthographic(GzReal left, GzReal right, GzReal bottom, GzReal top, GzReal nearVal, GzReal farVal) {
	//Set up a orthographic projection matrix
	//See http://www.opengl.org/sdk/docs/man/xhtml/glOrtho.xml
	//Or google: glOrtho

    double tx = -(right + left)/(right - left);
    double ty = -(top + bottom)/(top - bottom);
    double tz = -(farVal + nearVal)/(farVal - nearVal);

    GzReal arrayM[] =  {2/(right - left), 0, 0, tx,
                        0, 2/(top - bottom), 0, ty,
                        0, 0, -2/(farVal-nearVal), tz,
                        0, 0, 0, 1};

    GzMatrix M;
    copyArrayToMatrix(arrayM,M,4,4);

    multMatrix(M);
}
//End of Projections----------------------------------------------------------

void Gz::eucledianNorm(double v[])
{
    GzReal norm = 0;

    for (int i = 0; i < 3; i ++)
        norm += pow(v[i], 2);

    norm = sqrt(norm);

    for (int j = 0; j < 3; j++)
        v[j] = v[j]/norm;
}
void Gz::CrossProduct(double result[], double v1[], double v2[])
{
    result[0]= v1[1]*v2[2] - v1[2]*v2[1];
    result[1]= -(v1[0]*v2[2] - v1[2]*v2[0]);
    result[2]= v1[0]*v2[1] - v1[1]*v2[0];
}
void Gz::copyArrayToMatrix(double a[], GzMatrix m, int nRow, int nCol)
{
    m.resize(nRow,nCol);
    for (int i = 0; i < nRow*nCol; i++)
        m[i/nRow][i%nRow] = a[i];
}

//============================================================================
//End of Implementations in Assignment #3
//============================================================================
