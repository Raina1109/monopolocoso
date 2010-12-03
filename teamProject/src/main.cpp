<<<<<<< .mine


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
=======
#include <QtCore/QCoreApplication>

#include <cstdlib>
#include <ctime>
#include <iostream>
#include <set>
#include <stdlib.h>
#include <vector>

#include <GL/glut.h>
#include <GL.h>

using namespace std;

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    return a.exec();
}
>>>>>>> .r45
