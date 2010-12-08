#ifndef PARTICLE
#define PARTICLE

#include <stdlib.h>

#include "src\vec3f.h"

const float BOX_SIZE = 2.0f;

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

enum Wall {UP, DOWN, LEFT, RIGHT, NEAR, FAR);

struct WallCollision
{
	Wall wall;
	Particle *p;
};

bool TestCollision(Particle *pA, Particle *pB)
{
	Vec3f L = pA->center - pB->center;
	float sumR = pA->radius + pB->radius;
	
	if(L.magnitude() < sumR)
		return true;
	else
		return false;
}
//Returns the normal direction from the origin to the wall
Vec3f wallDirection(Wall wall) {
	switch (wall) {
		case LEFT:
			return Vec3f(-1, 0, 0);
		case RIGHT:
			return Vec3f(1, 0, 0);
		case FAR:
			return Vec3f(0, 0, -1);
		case NEAR:
			return Vec3f(0, 0, 1);
		case UP:
			return Vec3f(0, 1, 0);
		case DOWN:
			return Vec3f(0, -1, 0);
		default:
			return Vec3f(0, 0, 0);
	}
}
bool checkWallCollision(Particle* p, Wall w)
{
	Vec3f dir = wallDirection(w);
	
	return ((p->center.dot(dir) + p->radius) > (BOX_SIZE/2)) && (p->v.dor(dir) > 0);

}
#endif