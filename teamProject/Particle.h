#ifndef PARTICLE_H
#define PARTICLE_H

#include <cstdlib>
#include <cmath>

#include "src\vec3f.h"
#include "src\vector_types.h"
#include "src\vector_math.h"


class Particle
{
public:
	Particle(float3 c, float r, float3 co);
	~Particle(void);
	Vec3f position();
	float radius();
private:
	float3 Center;
	float Radius;
	float3 Color;
};

enum Wall {up, down, left, right, near, far}

struct particleCollision
{
	Particle *p1, *p2;
}

struct wallCollision
{
	Particle *p;
	Wall wall;
}

bool testCollide(Particle* p1, Particle p2);
#endif 