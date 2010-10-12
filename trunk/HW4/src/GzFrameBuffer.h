#ifndef __GZ_FRAME_BUFFER_H_
#define __GZ_FRAME_BUFFER_H_

#include "GzCommon.h"
#include "GzImage.h"
#include "GzVector.h"
#include "GzMatrix.h"
#include <vector>
#include <cmath>
using namespace std;

//Frame buffer with Z-buffer -------------------------------------------------
class GzFrameBuffer {
public:

//============================================================================
//Declarations in Assignment #1, 2, 3
//============================================================================
	//The common interface
	void initFrameSize(GzInt width, GzInt height);
	GzImage toImage();

	void clear(GzFunctional buffer);
	void setClearColor(const GzColor& color);
	void setClearDepth(GzReal depth);

	void drawPoint(const GzVertex& v, const GzColor& c, GzFunctional status);
	void drawTriangle(vector<GzVertex>& v, vector<GzColor>& c, GzFunctional status);

private:
	//Put any variables and private functions for your implementation here
	GzImage image;
	vector<vector<GzReal> > depthBuffer;
	GzColor clearColor;
	GzReal clearDepth;

	void realInterpolate(GzReal key1, GzReal val1, GzReal key2, GzReal val2, GzReal key, GzReal& val);
	void colorInterpolate(GzReal key1, GzColor& val1, GzReal key2, GzColor& val2, GzReal key, GzColor& val);
	void drawRasLine(GzInt y, GzReal xMin, GzReal zMin, GzColor& cMin, GzReal xMax, GzReal zMax, GzColor& cMax, GzFunctional status);

//============================================================================
//End of Declarations in Assignment #1, 2, 3
//============================================================================



//============================================================================
//Declarations in Assignment #4
//============================================================================
public:
	//Set the current shade model (curShadeModel)
	void shadeModel(const GzInt model);	
	
	//Add a light source at position p with color c
	void addLight(const GzVector& v, const GzColor& c);	
	
	//Specify the meterial of the object, includes:
	//	_kA: The ambient coefficients
	//	_kD: The diffuse coefficients
	//	_kS: The specular coefficients
	//	_s: The spec power
	void material(GzReal _kA, GzReal _kD, GzReal _kS, GzReal _s);		
	
	//Load the transformation matrix into framebuffer
	void loadLightTrans(GzMatrix transMatrix);

private:
	GzInt curShadeModel;
	GzReal kA, kD, kS, s;
        Vector <GzLightSource> LightSource;

        void drawPoint(const GzVertex& v,
                       const GzColor& c,
                       const GzVector& n ,
                       GzFunctional status);

        void drawTriangle(vector<GzVertex>& v,
                          vector<GzColor>& c,
                          vector<GzVector>& n,
                          GzFunctional status);

        void drawRasLine(GzInt y,
                         GzReal xMin, GzReal zMin, GzColor& cMin, GzVector& nMin,
                         GzReal xMax, GzReal zMax, GzColor& cMax, GzVector& nMax,
                         GzFunctional status);

        void vectorInterpolate(GzReal key1, GzVector& val1,
                              GzReal key2, GzVector& val2,
                              GzReal key, GzVector& val);
//============================================================================
//End of Declarations in Assignment #4
//============================================================================



};
//----------------------------------------------------------------------------

#endif
