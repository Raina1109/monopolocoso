
#include <cstdlib>
#include <ctime>
#include <iostream>
#include <set>
#include <stdlib.h>
#include <vector>
#include "glut.h"

#ifdef __APPLE__
#include<OpenGL/OpenGL.h>
#include<GLUT/glut.h>
#else
#include <gl/glu.h>
#endif

using namespace std;

//View params
int ox, oy;
int buttonState = 0;
float camera_trans[] = {0, 0, 3};
float camera_rot[] = {0, 0 , 0};
float camera_trans_lag[] = { 0, 0, 0 };
float camera_rot_lag[] = { 0, 0 ,0 };

int mode = 0;
bool Pause = false;
bool wireframe = false;
bool HUDshow = true;

//Simulation params
float timestep = 0.5f;
float damping = 1.0f;
float gravity = 0.0003f;
float iterations = 1;
int ballr = 10;

float collideSpring = 0.5f;;
float collideDamping = 0.02f;;
float collideShear = 0.1f;
float collideAttraction = 0.0f;

void handleKeypress(unsigned char key, int x, int y)
{
	switch(key){
	case 27: //Escape key
		exit(0);
	}
}

void initRendering()
{
	glEnable(GL_DEPTH_TEST);
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

	glBegin(GL_QUADS);

	glEnd();

	glutSwapBuffers();
}
int main(int argc, char** argv)
{
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH);
	glutInitWindowSize(640, 480); //Set the window size
	
	//Create the window
	glutCreateWindow("Team project - collisions");
	initRendering(); //Initialize rendering
	
	//Set handler functions for drawing, keypresses, and window resizes
	glutDisplayFunc(drawScene);
	glutKeyboardFunc(handleKeypress);
	glutReshapeFunc(handleResize);
	
	glutMainLoop(); //Start the main loop.  glutMainLoop doesn't return.
	

    return 0;
}