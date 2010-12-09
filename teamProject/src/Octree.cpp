#include "Octree.h"

 Node *BuildOctree(Vec3f center, float halfWidth, int stopDepth)
{
	if (stopDepth < 0)
		return NULL;
	else {
		// Construct and fill in root of this subtree
		Node *pNode = new Node;
		pNode->center = center;
		pNode->halfWidth = halfWidth;
		pNode->pObjList = NULL;
		// Recursively construct the eight children of the subtree
		Vec3f offset;
		float step = halfWidth * 0.5f;
		for (int i = 0; i < 8; i++)
		{
			offset[0] = ((i & 1) ? step : -step);
			offset[1] = ((i & 2) ? step : -step);
			offset[2] = ((i & 4) ? step : -step);
			pNode->pChild[i] = BuildOctree(center + offset, step, stopDepth - 1);
		}
	return pNode;
	}
}

void InsertObject(Node *pTree, Object *pObject)
{
	int index = 0, straddle = 0;
	// Compute the octant number [0..7] the object sphere center is in
	// If straddling any of the dividing x, y, or z planes, exit directly
	for (int i = 0; i < 3; i++) {
		float delta = pObject->particle->center[i] - pTree->center[i];
		if (abs(delta) < pTree->halfWidth + pObject->particle->radius) {
			straddle = 1;
			break;
		}
		if (delta > 0.0f) index |= (1 << i); // ZYX
	}
	if (!straddle && pTree->pChild[index]) {
	// Fully contained in existing child node; insert in that subtree
	InsertObject(pTree->pChild[index], pObject);
	} else {
		// Straddling, or no child node to descend into, so
		// link object into linked list at this node
		pObject->pNextObject = pTree->pObjList;
		pTree->pObjList = pObject;
	}
}

void InsertObject (Node *pTree, vector<Particle*> &particles)
{
    for(vector<Particle*>::iterator it = particles.begin(); it != particles.end(); it++)
    {
        Object *obj = new Object;
        obj->particle = *it;
        InsertObject(pTree,obj);
    }
}

void TestParticleCollision(Node *pTree, stack<ParticleCollision> &pStack)
{
    // Keep track of all ancestor object lists in a stack
    const int MAX_DEPTH = 5;
    static Node *ancestorStack[MAX_DEPTH];
    static int depth = 0; // Depth == 0 is invariant over calls

    // Check collision between all objects on this level and all
    // ancestor objects. The current level is included as its own
    // ancestor so all necessary pairwise tests are done
    ancestorStack[depth++] = pTree;
    for (int n = 0; n < depth; n++) {
        Object *pA, *pB;
        for (pA = ancestorStack[n]->pObjList; pA; pA = pA->pNextObject) {
            for (pB = pTree->pObjList; pB; pB = pB->pNextObject) {
                // Avoid testing both A->B and B->A
                if (pA == pB) break;
                // Now perform the collision test between pA and pB
                if(TestObjCollision(pA, pB))
                {
                    ParticleCollision pc;
                    pc.p1 = pA->particle;
                    pc.p2 = pB->particle;
                    pStack.push(pc);
                }
            }
        }
        // Recursively visit all existing children
        for (int i = 0; i < 8; i++)
            if (pTree->pChild[i])
                TestParticleCollision(pTree->pChild[i],pStack);
        // Remove current node from ancestor stack before returning
        depth--;
    }
}
bool TestObjCollision(Object *pA, Object *pB)
{
        return TestCollision(pA->particle, pB->particle);
}

void TestWallCollision(Node *pTree, stack<WallCollision> &collisions)
{
	potentialWallCollisions(pTree,collisions,LEFT,'x',0);
        potentialWallCollisions(pTree,collisions,RIGHT,'x',1);
	potentialWallCollisions(pTree,collisions,DOWN,'y',0);
	potentialWallCollisions(pTree,collisions,UP,'y',1);
	potentialWallCollisions(pTree,collisions,FAR,'z',0);
	potentialWallCollisions(pTree,collisions,NEAR,'z',1);

}

void potentialWallCollisions(Node *pTree,stack<WallCollision> &collisions, Wall w,char coord, int dir)
{
    Object *p;

    for (p = pTree->pObjList; p; p = p->pNextObject)
    {
        if(checkWallCollision(p->particle,w))
        {
            WallCollision wp;
            wp.wall = w;
            wp.p = p->particle;
            collisions.push(wp);
        }
    }

    for(int dir2 = 0; dir2 < 2; dir2++)
    {
        for(int dir3 = 0; dir3 < 2; dir3++)
        {
            int index = 0;
            switch(coord)
            {
            case 'x':
                index = dir + (dir2 << 1) + (dir3 << 2);
                break;
            case 'y':
                index = dir2 + (dir << 1) + (dir3 << 2);
                break;
            case 'z':
                index = dir2 + (dir3 << 1) + (dir << 2);
                break;
            }
            if(pTree->pChild[index])
                potentialWallCollisions(pTree->pChild[index],collisions, w, coord, dir);
        }
    }
}
