#ifndef OCTREE_H
#define OCTREE_H

#include <cstdlib.h>
#include <cmath>
#include <vector>
#include <stack>

#include "Particle.h"
#include "src\vec3f.h"

using namespace std;



struct Object {
	Particle* particle;
	Object *pNextObject; // Pointer to next object when linked into list
};


// Octree node data structure
struct Node {
	Vec3f center; // Center point of octree node 
	float halfWidth; // Half the width of the node volume 
	Node *pChild[8]; // Pointers to the eight children nodes
	Object *pObjList; // Linked list of objects contained at this node
};

// Preallocates an octree down to a specific depth
Node *BuildOctree(Vec3f center, float halfWidth, int stopDepth);

//inserting an object to a pre-defined octree
void InsertObject(Node *pTree, Object *pObject);

// Tests all objects that could possibly overlap due to cell ancestry and coexistence
// in the same cell. Assumes objects exist in a single cell only, and fully inside it
void TestParticleCollision(Node *pTree, stack<ParticleCollision> &pStack);

//Test collision between two object
bool TestCollision(Object *pA, Object *pB);

//Test all particle box wall collisions
void TestWallCollision(Node *pTree, stack<WallCollision> &collisions);

void potentialWallCollisions(Node *pTree,stack<WallCollision> &collisions,
										 Wall w,char coord, int dir);





#endif