#-------------------------------------------------
#
# Project created by QtCreator 2010-11-15T09:39:15
#
#-------------------------------------------------

QT       -= core

QT       -= gui

TARGET = teamProject
CONFIG   += console
CONFIG   -= app_bundle

TEMPLATE = app

LIBS = -lglut -lGLU
SOURCES += \
    src/main.cpp \
    src/vec3f.cpp \
    src/Octree.cpp \
    src/Particle.cpp

HEADERS += \
    src/vec3f.h \
    src/Particle.h \
    src/Octree.h
