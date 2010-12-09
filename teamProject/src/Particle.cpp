#include "Particle.h"



bool TestCollision(Particle *pA, Particle *pB)
{
        Vec3f L = pA->center - pB->center;
        float sumR = pA->radius + pB->radius;

        if(L.magnitude() < sumR)
                return true;
        else
                return false;
}
Vec3f wallDirection(Wall wall)
{
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

        return ((p->center.dot(dir) + p->radius) > (BOX_SIZE/2)) && (p->v.dot(dir) > 0);

}
