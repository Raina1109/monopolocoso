#ifndef PARTICLE_H
#define PARTICLE_H

#include <cstdlib>
#include <cmath>

#include "src\vector_types.h"
#include "src\vector_math.h"

class Particle
{
public:
	Particle(float3 c, float r, float3 co);
	~Particle(void);

private:
	float3 Center;
	float Radius;
	float3 Color;
};

#endif 