#ifndef PARTICLE
#define PARTICLE

#include <stdlib.h>

#include "vec3f.h"

using namespace std;

const float BOX_SIZE = 12.0f;

struct Particle
{
	Vec3f center;
	float radius;
	Vec3f color;
	Vec3f v;
};

struct ParticleCollision
{
	Particle *p1, *p2;
};

enum Wall {UP, DOWN, LEFT, RIGHT, NEAR, FAR};

struct WallCollision
{
	Wall wall;
	Particle *p;
};

bool TestCollision(Particle *pA, Particle *pB);

//Returns the normal direction from the origin to the wall
Vec3f wallDirection(Wall wall);

bool checkWallCollision(Particle* p, Wall w);


#endif
