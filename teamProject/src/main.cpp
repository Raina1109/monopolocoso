
#include <ctime>
#include <iostream>
#include <set>
#include <stdlib.h>
#include <cstdio>
#include <vector>

#ifdef WINDOWS
#include <GL/gl.h>
#include "glut.h"
#else
#include <GL/glut.h>
#include <GL/glu.h>
#endif

#include "Octree.h"
#include "vec3f.h"
#include "Particle.h"

using namespace std;

//Return a random float from 0 to 1
float randomFloat()
{
    return (float)rand() / ((float)RAND_MAX + 1);
}

//View params
int ox, oy;
int buttonState = 0;
float camera_trans[] = {0, 0, -25};
float camera_rot[] = {0, 0 , 0};
float angle = 0.0f;

int mode = 0;
bool Pause = false;
bool wireframe = true;
bool HUDshow = false;

//The amount of time between each time that we handle collisions and apply the
//effects of gravity
const float TIME_BETWEEN_UPDATES = 0.01f;
const int TIMER_MS = 25; //The number of milliseconds to which the timer is set

//Simulation params
float timestep = 0.5f;
Vec3f GRAVITY(0,-8,0);
float iterations = 1;
float ballr = 1.0f/64.0f;

float collideSpring = 0.5f;;
float collideDamping = 0.02f;;
float collideShear = 0.1f;
float boundaryDamping = -0.5f;



//Tree params
const int MAX_DEPTH = 5;
const Vec3f CENTER = Vec3f(0,0,0);
const float HALF_WIDTH = BOX_SIZE/2;

//datas
vector<Particle*> pList; //all the present particles
Node* Octree;
float timeUntilUpdate = 0;



Vec3f collideSpheres(Vec3f posA, Vec3f posB,
                     Vec3f velA, Vec3f velB,
                     float radiusA, float radiusB)
{
    Vec3f relPos = posB -posA;

    float dist = relPos.magnitude();
    float collideDist = radiusA + radiusB;

    Vec3f force = Vec3f(0,0,0);
    if (dist < collideDist)
    {
        Vec3f norm = relPos/dist;

        //relative velocity
        Vec3f relVel = velB - velA;
        //relative tangential velocity
        Vec3f tanVel = relVel - (relVel.dot(norm)*norm);
        //spring force
        force = -collideSpring*(collideDist - dist)*norm;
        //dashpot (damping) force
        force += collideDamping*relVel;
        // tangential shear force
        force += collideShear*tanVel;
        printf("force: %f %f %f\n",force[0],force[1],force[2]);
    }

    return force;
}

void handleParticleCollisions(vector<Particle*> &particles, Node* pTree)
{
    stack<ParticleCollision> pCollide;
    TestParticleCollision(pTree,pCollide,MAX_DEPTH);

    while(!pCollide.empty())
    {
        Particle* p1 = pCollide.top().p1;
        Particle* p2 = pCollide.top().p2;

        Vec3f force = collideSpheres(p1->center, p2->center,p1->v,p2->v,p1->radius,p2->radius);
        p1->v = p1->v + force*10;
        p2->v = p2->v - force*10;

        pCollide.pop();
    }
}

void handleWallCollisions(vector<Particle*> &particles, Node* pTree)
{
    stack<WallCollision> bwps;
    TestWallCollision(pTree,bwps);
    while(!bwps.empty())
    {
        WallCollision wc = bwps.top();
        Particle* p = wc.p;
        Wall w = wc.wall;

        Vec3f dir = (wallDirection(w)).normalize();
        p->v = boundaryDamping*dir*p->v.dot(dir);
        bwps.pop();
    }
}

void applyGravity(vector<Particle*> &particles)
{
    for(vector<Particle*>::iterator it = particles.begin(); it != particles.end();it++)
    {
        Particle* p = *it;
        p->v = p->v + (TIME_BETWEEN_UPDATES * GRAVITY);
    }
}

//Apply gravity and handles collisions
void performUpdate(vector<Particle*> &particles, Node* pTree)
{
   // applyGravity(particles);
    handleParticleCollisions(particles, pTree);
    handleWallCollisions(particles, pTree);
}

void moveParticles(vector<Particle*> &particles, float dt)
{
    for(vector<Particle*>::iterator it = particles.begin();it != particles.end();it++)
    {
        Particle* p = *it;
        p->center += p->v * dt;
    }
}

//Advances the state of the balls by t.  timeUntilUpdate is the amount of time
//until the next call to performUpdate.
void advance(vector<Particle*> &particles, Node* pTree,
                            float t, float &timeUntilUpdate)
{
    while (t > 0)
    {
        if (timeUntilUpdate <= t)
        {
            moveParticles(particles, timeUntilUpdate);

            clearObj(pTree,MAX_DEPTH);

            InsertObject(pTree, particles);

            performUpdate(particles,pTree);
            t -= timeUntilUpdate;
            timeUntilUpdate = TIME_BETWEEN_UPDATES;
        }
        else
        {
            moveParticles(particles, t);
            timeUntilUpdate -= t;
            t = 0;
        }
    }
}

void cleanup()
{
    pList.clear();
    delete Octree;
}

void handleKeypress(unsigned char key, int x, int y)
{
	switch(key){
	case 27: //Escape key
                cleanup();
		exit(0);
        case ' ': //space bar, add 20 balls with random position, velocity, radius and color
            for(int i = 0; i < 200; i++)
            {
                Particle* p = new Particle;
                p->center = Vec3f(8 * randomFloat() - 4,
                               8 * randomFloat() - 4,
                               8 * randomFloat() - 4);

                p->v = Vec3f(8 * randomFloat() - 4,
                             8 * randomFloat() - 4,
                             8 * randomFloat() - 4);

                p->radius = 0.1f * randomFloat() + 0.1f;

                p->color = Vec3f(0.6f * randomFloat() + 0.2f,
                                 0.6f * randomFloat() + 0.2f,
                                 0.6f * randomFloat() + 0.2f);

                pList.push_back(p);
                Object* obj = new Object;
                obj->particle = p;
                InsertObject(Octree,obj);
            }

	}
}

void initRendering()
{
	glEnable(GL_DEPTH_TEST);
        glEnable(GL_LIGHTING);
        glEnable(GL_LIGHT0);
        glEnable(GL_NORMALIZE);
        glEnable(GL_COLOR_MATERIAL);

}

void handleResize(int w, int h)
{
	glViewport(0,0,w,h);
	
	glMatrixMode(GL_PROJECTION);

	glLoadIdentity();

	gluPerspective(45.0,(double)w / (double)h,1.0,200.0);

}


void drawScene()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glMatrixMode(GL_MODELVIEW);

        glLoadIdentity();

        //translate the camera to gain full view of the scene
        glTranslatef(camera_trans[0],camera_trans[1],camera_trans[2]);
        glRotatef(-angle, 0.0f,1.0f,0.0f);

        //Light sources
        GLfloat ambientColor[] = {0.5f, 0.5f, 0.5f, 1.0f};
        glLightModelfv(GL_LIGHT_MODEL_AMBIENT, ambientColor);

        GLfloat lightColor[] = {0.7f, 0.7f, 0.7f, 1.0f};
        GLfloat lightPos[] = {1.0f, 0.2f, 0.0f, 0.0f};
        glLightfv(GL_LIGHT0, GL_DIFFUSE, lightColor);
        glLightfv(GL_LIGHT0, GL_POSITION, lightPos);

        //Draw the bounding box
        if(wireframe)
        glutWireCube(BOX_SIZE);

        //Draw the particles
        for(vector<Particle*>::iterator it = pList.begin(); it != pList.end();it++)
        {
            Particle* p = *it;
            glPushMatrix();
            glTranslatef(p->center[0],p->center[1],p->center[2]);
            glColor3f(p->color[0],p->color[1],p->color[2]);
            glutSolidSphere(p->radius,12,12);
            glPopMatrix();
        }
	glutSwapBuffers();
}

void update(int value) {
        advance(pList, Octree, (float)TIMER_MS / 1000.0f, timeUntilUpdate);
        angle += (float)TIMER_MS / 100;
        if (angle > 360) {
                angle -= 360;
        }

        glutPostRedisplay();
        glutTimerFunc(TIMER_MS, update, 0);
}

int main(int argc, char** argv)
{
    glutInit(&argc, argv);
    glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH);
    glutInitWindowSize(640, 480); //Set the window size

    //Create the window
    glutCreateWindow("Team project - collisions");
    initRendering(); //Initialize rendering

    Octree = BuildOctree(CENTER,HALF_WIDTH,MAX_DEPTH);

    glutWireCube(BOX_SIZE);

    //Set handler functions for drawing, keypresses, and window resizes
    glutDisplayFunc(drawScene);
    glutKeyboardFunc(handleKeypress);
    glutReshapeFunc(handleResize);
    glutTimerFunc(TIMER_MS, update, 0);

    glutMainLoop(); //Start the main loop.  glutMainLoop doesn't return.

    return 0;
}
