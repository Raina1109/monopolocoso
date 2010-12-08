#ifndef OCTREE_H
#define OCTREE_H

#include <cstdlib.h>
#include <cmath>
#include <vector>

#include "Particle.h"
#include "src\vec3f.h"

using namespace std;


int maxDepth = 10;
int minNumberOfParticle = 3;
int maxNumberOfParticle = 6;

class Octree
{
private:
	Vec3f corner1; //(minX, minY, minZ)
	Vec3f corner2; //(maxX, maxY, maxZ)
	Vec3f center;//((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2)
	Vec3f halfWidth;//(corner2 - corner1)/2

	Octree *pChild[8];
	//Is this a leaf node
	bool hasChildren;
	//List of Particle inside of this node
	vector<Particle*> pParticle;
	//The depth of this node in the tree
	int depth;
	
	//Clear all children nodes of the tree root at this node and add all their
	//Particles set into the node particles set
	void clearNode();
	
	//Add all particle pointer in the vector of particle store in the sub tree root
	//at this node to the vector in param
	void collectParticles(vector<Particle*> &pv);

	//Number of particle obj stored in the sub-tree root at this node
	int pCount;

	//Figure out which sub-tree of the current node does the current ball go into
	void branch (Particle* p, int &index, bool& straddle);

	void testParticlesCollision(Octree *pTree,vector<particleCollision> &cs);

public:

	
	Octree(int d, Vec3f c1, Vec3f c2);
	~Octree(void);

	//Add Particle into this tree
	void add(Particle* particle);
	//Remove a Particle from this tree
	void remove(Particle* particle);
	
	//Move a particle around the tree if needed
	//@param p: the particle with the new information
	//@param oldPos: previous world position used to compute tree position
	//void move (Particle* p, Vec3f oldPos);

	//Test particles collision
	//params cs: the list of particle collision we will use to paste the
	//collisions into
	void testParticlesCollision(vector<particleCollision> &cs);

	//Test particles collision with walls
	//@param cs: list of collision to paste back to collision handler
	//@param w: wall object, present the wall we want to test with.
	void testWallCollision(vector<wallCollision> &cs, Wall w);
	
	//rebuild the tree because the balls position has changed
	void rebuild()
};

#endif