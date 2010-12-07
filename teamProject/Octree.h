#ifndef OCTREE_H
#define OCTREE_H

#include <cstdlib>
#include <cmath>
#include <vector>

#include "src\Particle.h"
#include "src\vec3f.h"

using namespace std;


int maxDepth;
int minNumberOfParticle;
int maxNumberOfParticle;

class Octree
{
private:
	Vec3f corner1; //(minX, minY, minZ)
	Vec3f corner2; //(maxX, maxY, maxZ)
	Vec3f center;//((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2)

	Octree *pChid[8];
	//Is this a leaf node
	bool hasChildren;
	//List of Particle inside of this node
	vector<Particle*> pParticle;
	//The depth of this node in the tree
	int depth;
	
	//Clear all children nodes of the tree root at this node and add all their
	//Particles set into the node particles set
	void clearNode();
	
	

public:
	Octree(int d, Vec3f c1, Vec3f c2);
	~Octree(void);

	//Add Particle into this tree
	void add(Particle* particle);
	//Remove a Particle from this tree
	void remove(Particle* particle);
	//Change a Particle position
	void move(Particle*particle, Vec3f oldPos);


};

#endif