#include "Gz.h"



// Decleration in Assignment #2
// Function to calculate normal of an 3D triangle
typedef GzVertex vector3;
inline vector3 normal (const GzVertex& a,
					   const GzVertex& b, const GzVertex& c)
{
  vector3 v;
  v[X] = a[Y]*b[Z]-a[Z]*b[Y];
  v[Y] = a[Z]*b[X]-a[X]*b[Z];
  v[Z] = a[X]*b[Y]-a[Y]*b[X];
  return v;
}
// Function to calculate the distance from plan
// contain vertices a, b with normal vector v to projection surface

inline GzReal distance (const vector3& v, const GzVertex& a
						, const GzVertex& b)
{
  GzReal d = (b[X] - a[X])*v[X] + (b[Y]  - a[Y])*v[Y] + (b[Z] - a[Z])*v[Z];
  return d;
}

inline GzReal LEE (int x, int y, const GzVertex& a,
			  const GzVertex& b)
{
	GzReal dX = b[X] - a[X];
	GzReal dY = b[Y] - a[Y];
	return dY*(x - a[X]) - dX*(y - a[Y]);
}


bool signFunction (int x, int y, const GzVertex& a,
			  const GzVertex& b, const GzVertex& c)
{
	
	if(LEE(x,y,a,b) >= 0 && LEE(x,y,b,c) >= 0 && LEE(x,y,c,a) > 0)
	{
		return true;
	}

	return false;
}


GzReal interpolate_z(int x, int y, const vector3& n,GzReal d)
{
	return -(d+n[X]*x+n[Y]*y)/n[Z];
}
int min (GzReal v1, GzReal v2, GzReal v3)
{
	return (int)(min(v1,min(v2,v3))); 
}
int max (GzReal v1, GzReal v2, GzReal v3)
{
	return (int)(max(v1,max(v2,v3))); 
}


//============================================================================
//Implementations in Assignment #1
//============================================================================
void Gz::initFrameSize(GzInt width, GzInt height) {
	frameBuffer.initFrameSize(width, height);
}

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
	cout << vertexQueue.front()[X] << " "
		<< vertexQueue.front()[Y] << " "
		<< vertexQueue.front()[Z] << " " << endl;
}

void Gz::addColor(const GzColor& c) {
	colorQueue.push(c);
}
//============================================================================
//End of Implementations in Assignment #1
//============================================================================



//============================================================================
//Implementations in Assignment #2
//============================================================================
void Gz::end() {
	//In our implementation, all rendering is done when Gz::end() is called.
	//Depends on selected primitive, different number of vetices, colors, ect.
	//are pop out of the queue.
	switch (currentPrimitive) {
		case GZ_POINTS: {
			while ( (vertexQueue.size()>=1) && (colorQueue.size()>=1) ) {
				GzVertex v=vertexQueue.front(); vertexQueue.pop();
				GzColor c=colorQueue.front(); colorQueue.pop();
				frameBuffer.drawPoint(v, c, status);
			}
		} break;
		case GZ_TRIANGLES:
			{
			//Put your triangle drawing implementation here:
			//   - Pop 3 vertices in the vertexQueue
			//   - Pop 3 colors in the colorQueue
			//   - Call the draw triangle function 
			//     (you may put this function in GzFrameBuffer)
			GzVertex ve[3], vnow,v1;
			GzColor vColor[3], cnow;

			vector3 n;
			GzReal d;
			
			cout << vertexQueue.size() << " " << colorQueue.size() << endl;
			
			while ( (vertexQueue.size()>=3) && (colorQueue.size()>=3) )
			{
				for (int i = 0; i < 3; i++)
				{
				  ve[i] = vertexQueue.front();
				  vertexQueue.pop();
				  vColor[i] = colorQueue.front();
				  colorQueue.pop();
				}
				n = normal(ve[1],ve[2],ve[3]);
				d = distance(n,ve[1],ve[2]);

				int minx = min(ve[1][X],ve[2][X],ve[3][X]);
				int miny = min(ve[1][Y],ve[2][Y],ve[3][Y]);
				int maxx = max(ve[1][X],ve[2][X],ve[3][X]);
				int maxy = max(ve[1][Y],ve[2][Y],ve[3][Y]);

				for(int x=minx; x<=maxx;x++)
					for(int y=miny; y<=maxy;y++)
					{
						if(signFunction(x,y,ve[1],ve[2],ve[3]))
						{
							vnow[X] = x;
							vnow[Y] = y;
							vnow[Z] = interpolate_z(x,y,n,d);
							frameBuffer.drawPoint(vnow, GzColor(0,0,0,1), status);
						}
					}
			}
			}break;
	}
}

//============================================================================
//End of Implementations in Assignment #2
//============================================================================
