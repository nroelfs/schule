TEMPLATE = app
CONFIG += console c++11
CONFIG -= app_bundle
CONFIG -= qt

SOURCES += \
        cuboid.cpp \
        main.cpp \
        rectangle.cpp

HEADERS += \
    IGeometryObject.h \
    cuboid.h \
    rectangle.h
