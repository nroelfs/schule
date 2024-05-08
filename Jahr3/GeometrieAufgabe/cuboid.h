#ifndef CUBOID_H
#define CUBOID_H

#include "rectangle.h"

class Cuboid : public IGeometryObject
{
private:
    Rectangle faces[3];

public:
    Cuboid(const Rectangle (&rects)[3]);
    float surfaceArea() const override;
    float volume() const override;

};

#endif // CUBOID_H
