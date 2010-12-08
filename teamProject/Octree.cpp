#include "Octree.h"


Octree::Octree(int d, Vec3f c1, Vec3f c2)
{
	corner1 = c1;
	corner2 = c2;
	center = (c1 + c2)/2;
	halfWidth = (c2 - c1)/2;
	depth = d;
	hasChildren = false;
}


Octree::~Octree(void)
{
	if(hasChildren)
	{
		clearNode();
	}
}

void Octree::clearNode()
{
	collectParticles(pParticle);
	if(hasChildren)
	{
		for(int i = 0; i < 8; i++)
		{
			pChild[i]->clearNode();
		}
	}
}

void Octree::collectParticles(vector<Particle*> &pv)
{
	if(hasChildren)
	{
		for(int i = 0; i < 8; i++)
			pChild[i]->collectParticles(pv);
	}
	else
	{
		pv.insert(pv.end(),pParticle.begin(),pParticle.end());
	}
	
}

void Octree::remove(Particle* p)
{
	(pCount > 0) ? pCount-- : pCount;

	if (hasChildren && pCount < minNumberOfParticle)
	{
		clearNode();
	}

	if(hasChildren)
	{
		int index = 0;
		bool straddle = false;
		branch(p, index, straddle);
		if(straddle)
		{
			for (vector<Particle*>::iterator it = pParticle.begin(); it != pParticle.end(); it++)
			if (*it == p)
			{
				pParticle.erase(it);
				break;
			}
		}
		else
		{
			pChild[index]->remove(p);
		}
	}
	else
	{
		for (vector<Particle*>::iterator it = pParticle.begin(); it != pParticle.end(); it++)
			if (*it == p)
			{
				pParticle.erase(it);
				break;
			}
	}
}
void Octree::branch(Particle* p, int &index, bool& straddle)
{
	straddle = false;
	Vec3f pos = p->position();

	for (int i = 0; i < 3; i++) {
		float delta = pos[i] - center[i];

		if(abs(delta) < halfWidth[i] + p->radius())
		{
			straddle = true;
			break;
		}
		if (delta > 0.0f) index |= (1 << i); //ZYX
	}
}

void Octree::add(Particle* p)
{
	//index of the sub-tree that the ball belong to
	int index = 0;
	//is the ball hanging at the boundary of the grid
	bool straddle = false;
		
	branch(p, index, straddle);

	if(!straddle)
	{
		if (pChild[index] == NULL && depth < maxDepth)
		{
			Vec3f c1 = corner1;
			Vec3f c2 = corner2;
			if (index & 1){
				c1[0] = c1[0] + halfWidth[0];
				c2[0] = c2[0] - halfWidth[0];
			}
			if (index & 2) {
				c1[1] = c1[1] + halfWidth[1];
				c2[1] = c2[1] - halfWidth[1];
			}
			if (index & 4) {
				c1[2] = c1[2] + halfWidth[2];
				c2[2] = c2[2] - halfWidth[2];
			}
			pChild[index] = new Octree(depth+1,c1,c2);
		}
		else if ( depth >= maxDepth )
			pParticle.push_back(p);
		else
			pChild[index]->add(p);
	}
	else
		pParticle.push_back(p);
}

//void Octree::move(Particle* p, Vec3f oldPos)
//{
//
//}
void Octree::testParticlesCollision(vector<particleCollision> &cs)
{
	Octree* pTree = &this;
	testParticlesCollision(pTree,cs);
}

void Octree::testParticlesCollision(Octree *pTree, vector<particleCollision> &cs)
{
	//keep track of all ancestor object lists in a stack
	static Octree *ancestorStack[maxDepth];
	static int d = 0;

	// Check collision between all objects on this level and all
	// ancestor objects. The current level is included as its own
	// ancestor so all necessary pairwise tests are done
	ancestorStack[d++] = pTree;
	for(int n = 0; n < d; n++)
	{

	}

}

void Octree::rebuild()
{
	this->clearNode();
	vector<Particle*> pTemp = pParticle;
	pParticle.clear();
	for(vector<Particle*>::iterator it = pTemp.begin(); it != pTemp.end(); it++)
		this->add(*it);
}