#include "Particle.h"


Particle::Particle(float3 c, float r, float3 co)
{
	Center = c;
	Radius = r;
	Color = co;
}


Particle::~Particle(void)
{
}
